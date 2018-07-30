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

    public bool even;

    void Start () 
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
            Vector2 offset = (Vector2)Input.mousePosition - mouseClickPoint;
            offset *= zyf.GetWorldScreenSize().x / Screen.width;
            offset *= cameraSensitivity;
            worldObject.position = cameraOriginPos - (Vector3)offset;

            //移动到镜头边缘
            Vector2 offset2 = (Vector2)Input.mousePosition - mapOrigin;
            offset2 *= zyf.GetWorldScreenSize().x / Screen.width;
            offset2 *= cameraSensitivity;
            //镜头右移
            if (Mathf.Abs(offset2.x) > MapManager.Instance().nodePaddingX)
            {
                print("右移");
                mapOrigin.x = Input.mousePosition.x;

                if(offset2.x < 0)
                {
                    //方块向右移动
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
                else
                {
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
            }

            //镜头上移
            if (Mathf.Abs(offset2.y) > MapManager.Instance().nodePaddingY)
            {
                print("上移");
                mapOrigin.y = Input.mousePosition.y;

                even = !even;

                if (offset2.y > 0)
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
                else
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

            for (int i = 0; i < NodeManager.Instance().nodeCountY; i++)
            {
                for (int j = 0; j < NodeManager.Instance().nodeCountX; j++)
                {
                    //改变节点储存位置
                    MapManager.Instance().nodeItems[i, j].GetComponent<NodeItem>().pos = new Vector2Int(i, j);

                    //Node temp = NodeManager.Instance().nodes[i, 0];
                    //NodeManager.Instance().nodes[i, j] = MapManager.Instance().nodeItems[i, j].GetComponent<NodeItem>().pos;

                    NodeManager.Instance().nodes[i, j].pos = MapManager.Instance().nodeItems[i, j].GetComponent<NodeItem>().pos;
                }
            }
        }
    }
}
