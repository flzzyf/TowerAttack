using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : Singleton<BuildManager>
{
    public GameObject prefab_tower;

	void Start () 
	{
		
	}

	public void Build(GameObject _node, int _player)
    {
        GameObject go = Instantiate(prefab_tower, _node.transform.position, Quaternion.identity, ParentManager.Instance().GetParent("Tower"));
        _node.GetComponent<Node>().tower = go;
        go.GetComponent<Tower>().node = _node;
        go.GetComponent<Tower>().player =_player;

        _node.GetComponentInChildren<SpriteRenderer>().color = TeamManager.Instance().players[_player].color;

        foreach (Transform item in ParentManager.Instance().GetParent("Tower"))
        {
            item.gameObject.GetComponent<Tower>().SearchTarget();
        }

        GameManager.Instance().player = (GameManager.Instance().player + 1) % 2;
    }
}
