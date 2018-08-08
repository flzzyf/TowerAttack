using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeItem : MonoBehaviour
{
    public Transform invisibleThingsParent;
    [HideInInspector]
    public GameObject tower;

    public Vector2Int pos;
    public Vector2Int absPos;

    public GameObject text_force;

    //在此节点的玩家战力
    public int[] playerForce;
    public GameObject[] borders;

    public SpriteRenderer gfx;

    float clickConfirmDelay = .2f;
    float mouseDownTime;

    public GameObject towerPlacement;

    public GameObject fog;

    //在此节点的玩家视野
    public int[] playerVision;

    Animator animator;

    //可占领节点的父级物体，农场之类
    public GameObject occupiableNodeParent;

    public NodeItemAppearence[] appearences;

    public GameObject highlight;

    public void Init()
    {
        animator = GetComponent<Animator>();

        towerPlacement.SetActive(false);
        highlight.SetActive(false);

        playerForce = new int[PlayerManager.Instance().playerNumber];
        playerVision = new int[PlayerManager.Instance().playerNumber];
    }

    private void OnMouseEnter()
    {
#if UNITY_STANDALONE || UNITY_WEBGL
        animator.SetBool("hovered", true);
#endif
    }

    private void OnMouseExit()
    {
#if UNITY_STANDALONE || UNITY_WEBGL
        animator.SetBool("hovered", false);
#endif
    }

    private void OnMouseDown()
    {
        mouseDownTime = Time.time;
    }

    private void OnMouseUp()
    {
        //非拖动的快速点击
        if (Time.time - mouseDownTime < clickConfirmDelay)
        {
            //节点可见
            if(FogOfWarManager.Instance().NodeVisible(gameObject, GameManager.Instance().player))
            {
                if (tower == null)
                {
                    //未有塔，建造
                    BuildManager.Instance().ClickNode(gameObject, GameManager.Instance().player);
                }
                else
                {
                    //已有塔，升级
                    tower.GetComponent<Tower>().Upgrade_Vision();
                    tower.GetComponent<Tower>().Upgrade_Range();
                }
            }
        }
    }

    public void BuildSetting(int _player)
    {
        //增加周围节点战力
        foreach (var item in MapManager.Instance().GetNodesWithinRange(gameObject, 1))
        {
            item.GetComponent<NodeItem>().GetComponent<NodeItem>().playerForce[_player]++;
        }
        //更新边界和战力数字
        foreach (var item in MapManager.Instance().GetNodesWithinRange(gameObject, 2))
        {
            item.GetComponent<NodeItem>().UpdateBorders();
            item.GetComponent<NodeItem>().UpdateForceText(_player);
        }

        //可占领物体
        if(occupiableNodeParent != null)
        {
            occupiableNodeParent.GetComponent<OccupiableBuilding>().Occupy(_player);
        }
    }
    //塔被打掉
    public void TowerDestoryed(int _player)
    {
        foreach (var item in MapManager.Instance().GetNodesWithinRange(gameObject, 1))
        {
            item.GetComponent<NodeItem>().GetComponent<NodeItem>().playerForce[_player]--;
        }
        //更新边界和战力数字
        foreach (var item in MapManager.Instance().GetNodesWithinRange(gameObject, 2))
        {
            item.GetComponent<NodeItem>().UpdateBorders();
            item.GetComponent<NodeItem>().UpdateForceText(_player);
        }

        FogOfWarManager.Instance().RemoveNodesWithinRangeToPlayerVision(_player, gameObject, 2);

        //可占领物体
        if (occupiableNodeParent != null)
        {
            occupiableNodeParent.GetComponent<OccupiableBuilding>().Liberate(_player);
        }
    }

    int[] borderIndex = { 7, 1, 3, 5 };
    //更新边界
    public void UpdateBorders()
    {
        if(occupiableNodeParent == null)
        {
            //如果不是可占领建筑
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < PlayerManager.Instance().playerNumber; j++)
                {
                    if (playerForce[j] != 0 && MapManager.Instance().GetNearbyNode(gameObject, borderIndex[i]).GetComponent<NodeItem>().playerForce[j] == 0)
                    {
                        borders[i].SetActive(true);
                        borders[i].GetComponent<SpriteRenderer>().color = PlayerManager.Instance().players[j].color;
                        break;
                    }
                    else
                    {
                        borders[i].SetActive(false);
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < 4; i++)
            {
                //为可占领节点，周围不是
                if (MapManager.Instance().GetNearbyNode(gameObject, borderIndex[i]).GetComponent<NodeItem>().occupiableNodeParent == null)
                {
                    borders[i].SetActive(true);
                    borders[i].GetComponent<SpriteRenderer>().color = Color.white;
                }
                else
                {
                    borders[i].SetActive(false);
                }
            }
        }
        
    }
    //可占领建筑，节点初始化设置
    public void OccupiableBuildSetting()
    {
        UpdateBorders();
    }

    public void UpdateForceText(int _player)
    {
        if(tower != null)
        {
            //有塔
            text_force.SetActive(false);
        }
        else
        {
            //统计其他玩家在该节点的总战力
            int power = 0;
            for (int i = 0; i < PlayerManager.Instance().playerNumber; i++)
            {
                //非当前玩家
                if (i != GameManager.Instance().player)
                    power += playerForce[i];
            }
            if (_player != GameManager.Instance().player)
                text_force.GetComponent<Text>().text = power.ToString();

            if (power == 0)
                text_force.SetActive(false);
            else
                if (!text_force.activeSelf)
                text_force.SetActive(true);
        }
    }

    public void ToggleFogOfWar(bool _show)
    {
        fog.SetActive(_show);

        invisibleThingsParent.gameObject.SetActive(!_show);

    }

    public void SetOrderInLayer(int _order)
    {
        foreach (var item in GetComponentsInChildren<SpriteRenderer>(true))
        {
            item.sortingOrder = _order;
        }

        fog.GetComponentInChildren<SpriteRenderer>().sortingOrder = _order + 1;
    }

    [System.Serializable]
    public class NodeItemAppearence
    {
        public string name;
        public GameObject appearence;
    }

    public void ChangeNodeAppearence(string _name)
    {
        for (int i = 0; i < appearences.Length; i++)
        {
            if(appearences[i].name == _name)
            {
                appearences[i].appearence.SetActive(true);
            }
            else
            {
                appearences[i].appearence.SetActive(false);
            }
        }
    }
}
