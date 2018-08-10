using System.Collections;
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

    public Building[] buildings;

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
        GameObject go = Instantiate(prefab_tower, _node.transform.position, Quaternion.identity, _node.GetComponent<NodeItem>().invisibleThingsParent);
        _node.GetComponent<NodeItem>().tower = go;
        _node.GetComponent<NodeItem>().BuildSetting(_player);
        go.GetComponent<Tower>().node = _node;
        go.GetComponent<Tower>().player = _player;
        go.GetComponent<Tower>().SetOrderInLayer(_node.GetComponent<NodeItem>().gfx.sortingOrder);
        
        towers.Add(go);

        //开视野
        FogOfWarManager.Instance().AddNodesWithinRangeToPlayerVision(_player, _node, 2);

        return go;
    }

    public GameObject Build(GameObject _node, int _player)
    {
        //创建塔
        GameObject go = Instantiate(prefab_tower, _node.transform.position, Quaternion.identity, _node.GetComponent<NodeItem>().invisibleThingsParent);
        _node.GetComponent<NodeItem>().tower = go;
        go.GetComponent<Tower>().node = _node;
        go.GetComponent<Tower>().player = _player;
        go.GetComponent<Tower>().SetOrderInLayer(_node.GetComponent<NodeItem>().gfx.sortingOrder);

        towers.Add(go);

        StartCoroutine(IEBuild(go, _player, _node));

        return go;
    }

    //开始建造
    IEnumerator IEBuild(GameObject _tower, int _player, GameObject _node)
    {
        AudioSource source = null;

        if (_player == GameManager.Instance().player)
        {
            SoundManager.Instance().Play("Construct_Start");
            source = SoundManager.Instance().Play("Construct_Loop");
        }

        ScoreManager.Instance().ModifyWorker(_player, 5);

        _tower.GetComponent<Tower>().Building();

        float buildingProgress = 0;

        while (_tower != null && buildingProgress < buildingTime)
        {
            buildingProgress += buildingSpeed[0] *  Time.deltaTime;

            yield return null;
        }
        //建造完成，还没被打爆
        if(_tower != null)
        {
            if (_player == GameManager.Instance().player)
                SoundManager.Instance().Play("JobDone");

            _tower.GetComponent<Tower>().BuildingFinish();

            _node.GetComponent<NodeItem>().BuildSetting(_player);
            //开视野
            FogOfWarManager.Instance().AddNodesWithinRangeToPlayerVision(_player, _node, 2);

        }

        if(source != null)
        {
            source.Stop();
            Destroy(source);
        }

        ScoreManager.Instance().ModifyWorker(_player, -5);
    }

    public void ClickNode(GameObject _node, int _player)
    {
        //需要确认第二次点击
        if (desiredBuildTarget != _node)
        {
            SoundManager.Instance().Play("Placement");

            if (desiredBuildTarget != null)
            {
                ToggleHighlightNode(desiredBuildTarget, false);

            }

            desiredBuildTarget = _node;

            ToggleHighlightNode(_node, true);

        }
        else
        {
            if(ScoreManager.Instance().population_farmer[_player] < 5)
            {
                print("工人不足");
                return;
            }

            ToggleHighlightNode(_node, false);

            desiredBuildTarget = null;

            //确认建造
            Build(_node, GameManager.Instance().player);
        }
    }
    public void ToggleHighlightNode(GameObject _node, bool _on)
    {
        _node.GetComponent<NodeItem>().towerPlacement.SetActive(_on);
        foreach (var item in MapManager.Instance().GetNodesWithinRange(_node, 1))
        {
            item.GetComponent<NodeItem>().highlight.SetActive(_on);
        }
    }

    //建造可占领建筑
    public void BuildOccupiableBuilding(string _name, GameObject _node)
    {
        GameObject prefab = GetBuilding(_name);
        if(prefab == null)
        {
            print("无可建造物体");
            return;
        }

        GameObject go = Instantiate(prefab, _node.transform.position, Quaternion.identity, _node.GetComponent<NodeItem>().invisibleThingsParent);
        go.GetComponent<OccupiableBuilding>().BuildSetting(_node);
        _node.GetComponent<NodeItem>().building = go;
        //设置图层
        foreach (var item in go.GetComponentsInChildren<SpriteRenderer>(true))
        {
            int order = _node.GetComponent<NodeItem>().gfx.sortingOrder;
            item.sortingOrder = order;
        }
    }

    GameObject GetBuilding(string _name)
    {
        for (int i = 0; i < buildings.Length; i++)
        {
            if (buildings[i].name == _name)
            {
                return buildings[i].prefab;
            }
        }
        return null;
    }

    [System.Serializable]
    public class Building
    {
        public string name;
        public GameObject prefab;
    }

    //所有塔搜索敌人
    public void AllTowerStartSearching()
    {
        for (int i = 0; i < towers.Count; i++)
        {
            GameObject tower = towers[i];
            if (!tower.GetComponent<Tower>().building)
                tower.GetComponent<Tower>().SearchTarget();
        }
    }
}
