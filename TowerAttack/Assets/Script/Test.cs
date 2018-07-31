using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour 
{
	void Start () 
	{
        NodeManager.Instance().GenerateNodes();
        MapManager.Instance().GenerateMap();

    }

    void Update () 
	{
		if(Input.GetKeyDown(KeyCode.Z))
        {
            CameraControl_Touch.Instance().enabled = false;
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            CameraControl_Touch.Instance().enabled = true;
        }
    }
}
