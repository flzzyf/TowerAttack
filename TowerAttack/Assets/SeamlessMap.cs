using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeamlessMap : MonoBehaviour 
{
    public Transform worldObject;

    Vector2 mouseClickPoint;
    Vector3 cameraOriginPos;

    public float cameraSensitivity = 1;

    void Start () 
	{
        worldObject = ParentManager.Instance().grandparent;
	}
	

	void Update () 
	{
        if (Input.GetMouseButtonDown(0))
        {
            mouseClickPoint = Input.mousePosition;
            cameraOriginPos = worldObject.position;
        }

        if (Input.GetMouseButton(0))
        {
            Vector2 offset = (Vector2)Input.mousePosition - mouseClickPoint;
            offset *= zyf.GetWorldScreenSize().x / Screen.width;
            offset *= cameraSensitivity;
            print(offset);
            worldObject.position = cameraOriginPos - (Vector3)offset;
        }
    }
}
