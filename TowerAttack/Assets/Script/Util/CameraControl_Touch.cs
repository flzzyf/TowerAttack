using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl_Touch : MonoBehaviour 
{
    Vector2 mouseClickPoint;
    Vector3 cameraOriginPos;

    public float cameraSensitivity = 1;

	void Start () 
	{
#if UNITY_STANDALONE
        Debug.Log("PC版");

        //this.enabled = false;
#endif
    }

    void Update () 
	{
        if (GameManager.Instance().cameraControlMode != 1)
            return;

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
