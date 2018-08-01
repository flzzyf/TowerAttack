using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeamlessMap : Singleton<SeamlessMap>
{
    public Transform worldObject;

    Vector2 mouseClickPoint;
    Vector3 cameraOriginPos;

    public float cameraSensitivity = 1;

    Vector2 mapOrigin;
    [HideInInspector]
    public bool even;

    public void Init() 
	{
        worldObject = ParentManager.Instance().grandparent;
        //worldObject = ParentManager.Instance().GetParent("Node");
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
            //移动世界物体
            Vector2 offset = (Vector2)Input.mousePosition - mouseClickPoint;
            offset *= zyf.GetWorldScreenSize().x / Screen.width;
            offset *= cameraSensitivity;
            //worldObject.position = cameraOriginPos - (Vector3)offset;

            //移动到镜头边缘
            Vector2 offset2 = (Vector2)Input.mousePosition - mapOrigin;
            offset2 *= zyf.GetWorldScreenSize().x / Screen.width;
            offset2 *= cameraSensitivity;
            //worldObject.position = cameraOriginPos + (Vector3)Vector2.left * (offset2.x % MapManager.Instance().nodePaddingX) * MapManager.Instance().nodePaddingX;
            //worldObject.position = cameraOriginPos - (Vector3)offset;

            //镜头左右移动
            if (Mathf.Abs(offset2.x) > MapManager.Instance().nodePaddingX)
            {
                mapOrigin.x = Input.mousePosition.x;

                int repeatTime = (int)Mathf.Abs((offset2.x / MapManager.Instance().nodePaddingX));

                for (int i = 0; i < repeatTime; i++)
                {
                    if (offset2.x > 0)
                    {
                        MoveRight();
                    }
                    else
                    {
                        MoveLeft();
                    }
                }
                worldObject.Translate(-Mathf.Sign(offset2.x) * Vector2.right * repeatTime * MapManager.Instance().nodePaddingX);
            }

            //镜头上下移动
            if (Mathf.Abs(offset2.y) > MapManager.Instance().nodePaddingY)
            {
                //print("上移");
                mapOrigin.y = Input.mousePosition.y;


                int repeatTime = (int)Mathf.Abs((offset2.y / MapManager.Instance().nodePaddingY));
                for (int i = 0; i < repeatTime; i++)
                {
                even = !even;
                    if (offset2.y > 0)
                    {
                        MoveUp();
                    }
                    else
                    {
                        MoveDown();
                    }
                }
                worldObject.Translate(-Mathf.Sign(offset2.y) * Vector2.up * repeatTime * MapManager.Instance().nodePaddingY);

                //改变节点层级
                for (int i = 0; i < NodeManager.Instance().nodeCountY; i++)
                {
                    for (int j = 0; j < NodeManager.Instance().nodeCountX; j++)
                    {
                        MapManager.Instance().nodeItems[i, j].GetComponent<NodeItem>().gfx.sortingOrder = NodeManager.Instance().nodeCountY - i;

                        //改变塔层级
                        if(MapManager.Instance().nodeItems[i, j].GetComponent<NodeItem>().tower != null)
                            MapManager.Instance().nodeItems[i, j].GetComponent<NodeItem>().tower.GetComponent<Tower>().SetOrderInLayer(NodeManager.Instance().nodeCountY - i);
                        
                    }
                }
            }

            //改变节点储存位置
            for (int i = 0; i < NodeManager.Instance().nodeCountY; i++)
            {
                for (int j = 0; j < NodeManager.Instance().nodeCountX; j++)
                {
                    MapManager.Instance().nodeItems[i, j].GetComponent<NodeItem>().pos = new Vector2Int(i, j);

                    //Node temp = NodeManager.Instance().nodes[i, 0];
                    //NodeManager.Instance().nodes[i, j] = MapManager.Instance().nodeItems[i, j].GetComponent<NodeItem>().pos;

                    NodeManager.Instance().nodes[i, j].pos = new Vector2Int(i, j);
                }
            }
        }
    }


    void MoveRight()
    {
        //方块向右移动
        for (int i = 0; i < NodeManager.Instance().nodeCountY; i++)
        {
            Vector2 movingDir = Vector2.right * MapManager.Instance().nodePaddingX * NodeManager.Instance().nodeCountX;
            MapManager.Instance().nodeItems[i, 0].transform.Translate(movingDir);
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

    void MoveLeft()
    {
        for (int i = 0; i < NodeManager.Instance().nodeCountY; i++)
        {
            Vector2 movingDir = -Vector2.right * MapManager.Instance().nodePaddingX * NodeManager.Instance().nodeCountX;
            MapManager.Instance().nodeItems[i, NodeManager.Instance().nodeCountX - 1].transform.Translate(movingDir);
        }
        //改变地图数组中节点序号
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

    void MoveUp()
    {
        //方块向上移动
        for (int i = 0; i < NodeManager.Instance().nodeCountX; i++)
        {
            Vector2 movingDir = Vector2.up * MapManager.Instance().nodePaddingY * NodeManager.Instance().nodeCountY;
            MapManager.Instance().nodeItems[0, i].transform.Translate(movingDir);
        }

        for (int i = 0; i < NodeManager.Instance().nodeCountX; i++)
        {
            GameObject temp = MapManager.Instance().nodeItems[0, i];
            for (int j = 0; j < NodeManager.Instance().nodeCountY - 1; j++)
            {
                MapManager.Instance().nodeItems[j, i] = MapManager.Instance().nodeItems[j + 1, i];
            }
            MapManager.Instance().nodeItems[NodeManager.Instance().nodeCountY - 1, i] = temp;
        }
    }

    void MoveDown()
    {
        //方块向下移动
        for (int i = 0; i < NodeManager.Instance().nodeCountX; i++)
        {
            Vector2 movingDir = -Vector2.up * MapManager.Instance().nodePaddingY * NodeManager.Instance().nodeCountY;
            MapManager.Instance().nodeItems[NodeManager.Instance().nodeCountY - 1, i].transform.Translate(movingDir);
        }

        for (int i = 0; i < NodeManager.Instance().nodeCountX; i++)
        {
            GameObject temp = MapManager.Instance().nodeItems[NodeManager.Instance().nodeCountY - 1, i];
            for (int j = NodeManager.Instance().nodeCountY - 1; j > 0; j--)
            {
                MapManager.Instance().nodeItems[j, i] = MapManager.Instance().nodeItems[j - 1, i];
            }
            MapManager.Instance().nodeItems[0, i] = temp;
        }
    }
}
