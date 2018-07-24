using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : Singleton<BuildManager>
{
    public GameObject prefab_tower;

	public GameObject Build(GameObject _node, int _player)
    {
        GameObject go = Instantiate(prefab_tower, _node.transform.position, Quaternion.identity, ParentManager.Instance().GetParent("Tower"));
        _node.GetComponent<NodeItem>().tower = go;
        go.GetComponent<Tower>().node = _node;
        go.GetComponent<Tower>().player =_player;

        _node.GetComponent<NodeItem>().ChangeColor(TeamManager.Instance().players[_player].color);

        GameManager.Instance().player = (GameManager.Instance().player + 1) % 3;

        return go;
    }
}
