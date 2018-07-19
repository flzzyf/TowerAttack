using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : Singleton<BuildManager>
{
    public GameObject prefab_tower;

	void Start () 
	{
		
	}

	public void Build(GameObject _node)
    {
        GameObject go = Instantiate(prefab_tower, _node.transform.position, Quaternion.identity, ParentManager.Instance().GetParent("Tower"));
        _node.GetComponent<Node>().tower = go;
        go.GetComponent<Tower>().node = _node;
    }
}
