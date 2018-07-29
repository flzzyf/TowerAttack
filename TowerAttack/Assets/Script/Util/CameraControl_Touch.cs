using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl_Touch : Singleton<CameraControl_Touch>
{
    Vector2 mouseClickPoint;
    Vector3 cameraOriginPos;

    public float cameraSensitivity = 1;

    void Update () 
	{
        if(Input.GetMouseButtonDown(0))
        {
            mouseClickPoint = Input.mousePosition;
            cameraOriginPos = transform.position;
        }

        if(Input.GetMouseButton(0))
        {
            Vector2 offset = (Vector2)Input.mousePosition - mouseClickPoint;
            offset *= zyf.GetWorldScreenSize().x / Screen.width;
            offset *= cameraSensitivity;
            transform.position = cameraOriginPos + (Vector3)offset;
        }

    }
}
