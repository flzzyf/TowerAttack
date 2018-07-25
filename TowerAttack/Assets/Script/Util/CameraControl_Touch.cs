using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl_Touch : MonoBehaviour 
{


	void Start () 
	{
#if UNITY_STANDALONE
        Debug.Log("PC版");

        this.enabled = false;
#endif
    }


    void Update () 
	{
		
	}
}
