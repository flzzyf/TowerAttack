using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeamlessMap : MonoBehaviour 
{
    public Transform worldObject;

    Vector2 mouseClickPoint;
    Vector3 cameraOriginPos;

    public float cameraSensitivity = 1;

    public Vector2Int mapCenter = new Vector2Int(2, 2);
    Vector2 mapOrigin;

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

            mapOrigin = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            Vector2 offset = (Vector2)Input.mousePosition - mouseClickPoint;
            offset *= zyf.GetWorldScreenSize().x / Screen.width;
            offset *= cameraSensitivity;
            worldObject.position = cameraOriginPos - (Vector3)offset;

            //移动到镜头边缘
            Vector2 offset2 = (Vector2)Input.mousePosition - mapOrigin;
            offset2 *= zyf.GetWorldScreenSize().x / Screen.width;
            offset2 *= cameraSensitivity;

            if (Mathf.Abs(offset2.x) > MapManager.Instance().nodePaddingX ||
                Mathf.Abs(offset2.y) > MapManager.Instance().nodePaddingY)
            {
                //print(offset2);
                mapOrigin = Input.mousePosition;

                if(offset2.x < 0)
                {
                    //镜头右移，方块向左移动
                    for (int i = 0; i < NodeManager.Instance().nodeCountY; i++)
                    {
                        MapManager.Instance().nodeItems[i, NodeManager.Instance().nodeCountX - 1].transform.Translate(
                            Vector2.left * MapManager.Instance().nodePaddingX * NodeManager.Instance().nodeCountX);
                    }

                    for (int i = 0; i < NodeManager.Instance().nodeCountY; i++)
                    {
                        GameObject temp = MapManager.Instance().nodeItems[i, NodeManager.Instance().nodeCountX - 1];
                        for (int j = NodeManager.Instance().nodeCountX - 1; j > 0; j--)
                        {
                            MapManager.Instance().nodeItems[i, j] = MapManager.Instance().nodeItems[i, j - 1];
                        }
                        MapManager.Instance().nodeItems[i, 0] = temp;
                    }
                }
                else
                {
                    for (int i = 0; i < NodeManager.Instance().nodeCountY; i++)
                    {
                        MapManager.Instance().nodeItems[i, 0].transform.Translate(Vector2.right * MapManager.Instance().nodePaddingX * NodeManager.Instance().nodeCountX);
                    }

                    for (int i = 0; i < NodeManager.Instance().nodeCountY; i++)
                    {
                        GameObject temp = MapManager.Instance().nodeItems[i, 0];
                        for (int j = 0; j < NodeManager.Instance().nodeCountX - 1; j++)
                        {
                            MapManager.Instance().nodeItems[i, j] = MapManager.Instance().nodeItems[i, j + 1];
                        }
                        MapManager.Instance().nodeItems[i, NodeManager.Instance().nodeCountX - 1] = temp;
                    }
                }
            }
        }
    }
}
