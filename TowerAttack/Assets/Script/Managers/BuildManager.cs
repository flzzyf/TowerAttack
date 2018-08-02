﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : Singleton<BuildManager>
{
    public GameObject prefab_tower;

    public float buildingTime = 1f;

    [HideInInspector]
    public float[] buildingSpeed;

    public static GameObject desiredBuildTarget;
    [HideInInspector]
    public List<GameObject> towers = new List<GameObject>();

    public int towerPrice = 5;

    public void Init()
    {
        //初始化所有玩家建造速度
        int playerNumber = PlayerManager.Instance().playerNumber;
        buildingSpeed = new float[playerNumber];

        for (int i = 0; i < playerNumber; i++)
        {
            buildingSpeed[i] = 1;
        }
    }

    public GameObject BuildInstantly(GameObject _node, int _player)
    {
        GameObject go = Instantiate(prefab_tower, _node.transform.position, Quaternion.identity, _node.transform);
        _node.GetComponent<NodeItem>().tower = go;
        _node.GetComponent<NodeItem>().BuildSetting();
        go.GetComponent<Tower>().node = _node;
        go.GetComponent<Tower>().player = _player;
        go.GetComponent<Tower>().SetOrderInLayer(_node.GetComponent<NodeItem>().gfx.sortingOrder);

        towers.Add(go);

        FogOfWarManager.Instance().AddNodesWithinRangeToPlayerVision(_player, _node, 2);

        return go;
    }

    public GameObject Build(GameObject _node, int _player)
    {
        SoundManager.Instance().Play("Shoot");

        GameObject go = BuildInstantly(_node, _player);

        StartCoroutine(Building(go, _player));

        //所有塔开始搜索目标
        for (int i = 0; i < towers.Count; i++)
        {
            if (!towers[i].GetComponent<Tower>().building)
                towers[i].GetComponent<Tower>().SearchTarget();
        }

        return go;
    }

    //开始建造
    IEnumerator Building(GameObject _tower, int _player)
    {
        IncomeManager.Instance().SetIncomeRate(_tower.GetComponent<Tower>().player, -0.5f);

        _tower.GetComponent<Tower>().Building();

        float buildingProgress = 0;

        while (_tower != null && buildingProgress < buildingTime)
        {
            buildingProgress += buildingSpeed[0] *  Time.deltaTime;

            yield return null;
        }
        //还没被打爆
        if(_tower != null)
            _tower.GetComponent<Tower>().BuildingFinish();

        IncomeManager.Instance().SetIncomeRate(_player, 0.5f);
    }

    public void ClickNode(GameObject _node, int _player)
    {
        //确认二次点击
        if (desiredBuildTarget != gameObject)
        {
            if (desiredBuildTarget != null)
            {
                desiredBuildTarget.GetComponent<NodeItem>().towerPlacement.SetActive(false);
            }

            desiredBuildTarget = gameObject;

            _node.GetComponent<NodeItem>().towerPlacement.SetActive(true);
        }
        else
        {
            if (PlayerManager.Instance().players[_player].money < towerPrice)
            {
                print("金钱不足!");
                return;
            }

            IncomeManager.Instance().ModifyMoney(_player, -towerPrice);

            _node.GetComponent<NodeItem>().towerPlacement.SetActive(false);

            desiredBuildTarget = null;

            //确认建造
            Build(_node, GameManager.Instance().player);
        }
    }
}
