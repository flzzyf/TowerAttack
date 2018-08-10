using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NodeItem : MonoBehaviour
{
    public Transform invisibleThingsParent;
    [HideInInspector]
    public GameObject tower;
    [HideInInspector]
    public GameObject building;

    public Vector2Int pos;
    public Vector2Int absPos;

    public GameObject text_force;

    //在此节点的玩家战力
    public int[] playerForce;
    public GameObject[] borders;

    public GameObject gfx;

    float clickConfirmDelay = .2f;
    float mouseDownTime;

    public GameObject towerPlacement;
    public GameObject towerUpgrade_Range;
    public GameObject towerUpgrade_Vision;

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
        towerUpgrade_Range.SetActive(false);
        towerUpgrade_Vision.SetActive(false);
        highlight.SetActive(false);

        playerForce = new int[PlayerManager.Instance().playerNumber];
        playerVision = new int[PlayerManager.Instance().playerNumber];
    }

    private void OnMouseEnter()
    {
#if UNITY_STANDALONE || UNITY_WEBGL
        //点击在按UI钮上
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
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
        //点击在按UI钮上
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

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
                    if (tower.GetComponent<Tower>().building)
                        return;

                    //已有塔，升级
                    if(!tower.GetComponent<Tower>().upgraded[0])
                        towerUpgrade_Range.SetActive(true);
                    if(!tower.GetComponent<Tower>().upgraded[1])
                        towerUpgrade_Vision.SetActive(true);
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

    //设置图层顺序
    public void SetOrderInLayer(int _order)
    {
        int order = _order * 5;
        foreach (var item in GetComponentsInChildren<SpriteRenderer>(true))
        {
            item.sortingOrder = order;
        }

        fog.GetComponentInChildren<SpriteRenderer>().sortingOrder = order + 1;

        if (tower != null)
            tower.GetComponent<Tower>().SetOrderInLayer(order);

        if (building != null)
        {
            foreach (var item in building.GetComponentsInChildren<SpriteRenderer>(true))
            {
                item.sortingOrder = order;
            }
        }
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

    //升级射程
    public void Upgrade_Range()
    {
        if (ScoreManager.Instance().population_farmer[tower.GetComponent<Tower>().player] < 5)
        {
            print("工人不足");
            return;
        }
        towerUpgrade_Range.SetActive(false);
        towerUpgrade_Vision.SetActive(false);

        tower.GetComponent<Tower>().Upgrade(0);
    }
    //升级视野
    public void Upgrade_Vision()
    {
        if (ScoreManager.Instance().population_farmer[tower.GetComponent<Tower>().player] < 5)
        {
            print("工人不足");
            return;
        }
        towerUpgrade_Range.SetActive(false);
        towerUpgrade_Vision.SetActive(false);

        tower.GetComponent<Tower>().Upgrade(1);
    }

}
