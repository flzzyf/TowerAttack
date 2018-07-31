using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : Singleton<BuildManager>
{
    public GameObject prefab_tower;

    public float buildingTime = 1f;

    [HideInInspector]
    public float[] buildingSpeed;

    [HideInInspector]
    public GameObject desiredBuildTarget;

    private void Start()
    {
        //初始化所有玩家建造速度
        int playerNumber = PlayerManager.Instance().playerNumber;
        buildingSpeed = new float[playerNumber];

        for (int i = 0; i < playerNumber; i++)
        {
            buildingSpeed[i] = 1;
        }
    }

    public GameObject Build(GameObject _node, int _player)
    {
        SoundManager.Instance().Play("Shoot");

        GameObject go = Instantiate(prefab_tower, _node.transform.position, Quaternion.identity, _node.transform);
        _node.GetComponent<NodeItem>().tower = go;
        _node.GetComponent<NodeItem>().BuildSetting();
        go.GetComponent<Tower>().node = _node;
        go.GetComponent<Tower>().player =_player;
        go.GetComponent<Tower>().SetOrderInLayer(_node.GetComponentInChildren<SpriteRenderer>().sortingOrder);

        StartCoroutine(Building(go));

        //切换玩家
        //GameManager.Instance().player = (GameManager.Instance().player + 1) % 3;

        return go;
    }

    //开始建造
    IEnumerator Building(GameObject _tower)
    {
        _tower.GetComponent<Tower>().Building();

        float buildingProgress = 0;

        while (buildingProgress < buildingTime)
        {
            buildingProgress += buildingSpeed[0] *  Time.deltaTime;

            yield return null;
        }

        _tower.GetComponent<Tower>().BuildingFinish();


    }
}
