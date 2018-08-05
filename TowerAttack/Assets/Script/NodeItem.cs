﻿using System.Collections;
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

    public void Init()
    {
        animator = GetComponent<Animator>();

        towerPlacement.SetActive(false);

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
            if(FogOfWarManager.Instance().NodeVisible(gameObject, GameManager.Instance().player))
            {
                if (tower == null)
                {
                    BuildManager.Instance().ClickNode(gameObject, GameManager.Instance().player);

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

    }

    int[] borderIndex = { 7, 1, 3, 5 };
    //更新边界
    public void UpdateBorders()
    {
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
        foreach (var item in GetComponentsInChildren<SpriteRenderer>())
        {
            item.sortingOrder = _order;
        }

        fog.GetComponentInChildren<SpriteRenderer>().sortingOrder = _order + 1;
    }
}
