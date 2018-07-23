using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : Singleton<BuildManager>
{
    public GameObject prefab_tower;

	public void Build(GameObject _node, int _player)
    {
        GameObject go = Instantiate(prefab_tower, _node.transform.position, Quaternion.identity, ParentManager.Instance().GetParent("Tower"));
        _node.GetComponent<NodeItem>().tower = go;
        go.GetComponent<Tower>().node = _node;
        go.GetComponent<Tower>().player =_player;

        _node.GetComponent<NodeItem>().ChangeColor(TeamManager.Instance().players[_player].color);

        foreach (Transform item in ParentManager.Instance().GetParent("Tower"))
        {
            //无目标则搜索目标
            // if(item.gameObject.GetComponent<Tower>().target == null)
            //     item.gameObject.GetComponent<Tower>().SearchTarget();
            // else
            //     item.gameObject.GetComponent<Tower>().Attack();
        }

        GameManager.Instance().player = (GameManager.Instance().player + 1) % 3;
    }
}
