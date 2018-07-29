using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl_RTS : Singleton<CameraControl_RTS>
{
    public float panSpeed = 20f;
    public float panBorderThickness = 10f;
    public Vector2 panLimit;

    public bool mode_2D;

    void Update()
    {
        float x = 0, y = 0;

        if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panBorderThickness)
        {
            y += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("s") || Input.mousePosition.y <= panBorderThickness)
        {
            y -= panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("a") || Input.mousePosition.x <= panBorderThickness)
        {
            x -= panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            x += panSpeed * Time.deltaTime;
        }

        x = Mathf.Clamp(x, -panLimit.x, panLimit.x);
        y = Mathf.Clamp(y, -panLimit.y, panLimit.y);

        if (mode_2D)
            transform.Translate(new Vector3(x, y, 0));
        else
            transform.Translate(new Vector3(x, 0, y));

    }
}
