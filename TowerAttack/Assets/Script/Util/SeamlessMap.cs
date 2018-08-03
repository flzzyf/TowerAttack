using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeamlessMap : Singleton<SeamlessMap>
{
    public Transform worldObject;

    public float cameraSensitivity = 1;

    [HideInInspector]
    public bool even;

    Vector2 mouseOrigin;

    public void Init() 
	{
        worldObject = ParentManager.Instance().grandparent;
    }

	void Update () 
	{
        if (Input.GetMouseButtonDown(0))
        {
            mouseOrigin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mouseOffset = mousePos - mouseOrigin;
            mouseOffset *= cameraSensitivity;

            mouseOrigin = mousePos;
            MoveRight(mouseOffset.x);
            MoveUp(mouseOffset.y);
            
            //改变节点储存位置
            for (int i = 0; i < NodeManager.Instance().nodeCountY; i++)
            {
                for (int j = 0; j < NodeManager.Instance().nodeCountX; j++)
                {
                    MapManager.Instance().nodeItems[i, j].GetComponent<NodeItem>().pos = new Vector2Int(i, j);

                }
            }
        }
    }

    float offsetX;
    public void MoveRight(float _amount)
    {
        offsetX += _amount;

        //移动整个世界物体
        worldObject.Translate(-Vector2.right * _amount);

        //移动节点
        if (Mathf.Abs(offsetX) > MapManager.Instance().nodePaddingX)
        {
            int repeatTime = (int)(Mathf.Abs(offsetX) / MapManager.Instance().nodePaddingX);

            offsetX %= MapManager.Instance().nodePaddingX;

            for (int i = 0; i < repeatTime; i++)
            {
                if (_amount > 0)
                {
                    MoveRight();
                }
                else
                {
                    MoveLeft();
                }
            }
        }
    }

    float offsetY;
    public void MoveUp(float _amount)
    {
        offsetY += _amount;

        //移动整个世界物体
        worldObject.Translate(-Vector2.up * _amount);

        //移动节点
        if (Mathf.Abs(offsetY) > MapManager.Instance().nodePaddingY)
        {
            int repeatTime = (int)(Mathf.Abs(offsetY) / MapManager.Instance().nodePaddingY);

            offsetY %= MapManager.Instance().nodePaddingY;

            for (int i = 0; i < repeatTime; i++)
            {
                even = !even;

                if (_amount > 0)
                {
                    MoveUp();
                }
                else
                {
                    MoveDown();
                }
            }

            //改变节点层级
            for (int i = 0; i < NodeManager.Instance().nodeCountY; i++)
            {
                for (int j = 0; j < NodeManager.Instance().nodeCountX; j++)
                {
                    MapManager.Instance().nodeItems[i, j].GetComponent<NodeItem>().SetOrderInLayer(2 * (NodeManager.Instance().nodeCountY - i));

                    //改变塔层级
                    GameObject tower = MapManager.Instance().nodeItems[i, j].GetComponent<NodeItem>().tower;

                    if (tower != null)
                        tower.GetComponent<Tower>().SetOrderInLayer(NodeManager.Instance().nodeCountY - i);

                }
            }
        }
    }
    //方块向右移动
    void MoveRight()
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
    //方块向左移动
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
    //方块向上移动
    void MoveUp()
    {
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
    //方块向下移动
    void MoveDown()
    {
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
