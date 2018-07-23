using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : Singleton<MapManager>
{
    public GameObject prefab_node;
    public float nodePaddingX = 1;
    public float nodePaddingY = 1;
    public Vector2 originPoint;
    //偶数行X偏移
    public float evenLineOffsetX;

    [HideInInspector]
    public GameObject[,] nodeItems;

    public void GenerateMap()
    {
        int mapSizeX = NodeManager.Instance().nodeCountX;
        int mapSizeY = NodeManager.Instance().nodeCountY;

        nodeItems = new GameObject[mapSizeY, mapSizeX];

        // 生成节点的真正源点
        Vector2 originGeneratePoint;
        originGeneratePoint.x = originPoint.x - (float)mapSizeX / 2 * nodePaddingX + nodePaddingX / 2;
        originGeneratePoint.y = originPoint.y - (float)mapSizeY / 2 * nodePaddingY + nodePaddingY / 2;

        for (int i = 0; i < mapSizeY; i++)
        {
            float specialX = 0;
            if (evenLineOffsetX != 0)
            {
                specialX = i % 2 == 0 ? 0 : nodePaddingX * evenLineOffsetX;
            }

            for (int j = 0; j < mapSizeX; j++)
            {
                float y = i * nodePaddingY;
                Vector2 pos = new Vector2(j * nodePaddingX + specialX, y);
                pos += originGeneratePoint;
                GameObject go = Instantiate(prefab_node, pos, Quaternion.identity, ParentManager.Instance().GetParent("Nodes"));
                go.name = "Node_" + i + "_" + j;
                nodeItems[i, j] = go;
                go.GetComponent<NodeItem>().pos = new Vector2Int(i, j);

                go.GetComponentInChildren<SpriteRenderer>().sortingOrder = mapSizeY - i;
            }
        }
    }

    public GameObject GetNearbyNode(GameObject _go, int _index)
    {
        NodeItem nodeItem = _go.GetComponent<NodeItem>();
        Node node = NodeManager.Instance().GetNearbyNode(nodeItem.pos, _index);
        return GetNodeItem(node.pos);
    }

    public GameObject GetNodeItem(Vector2Int _pos)
    {
        return nodeItems[_pos.x, _pos.y];
    }

    public GameObject[] GetNearbyNodeItems(Vector2Int _pos)
    {
        int a = 0;
        GameObject[] nodeItemList = new GameObject[8];
        foreach (var item in NodeManager.Instance().GetNearbyNodes(_pos))
        {
            nodeItemList[a] = GetNodeItem(item.pos);
            a++;
        }
    

        return nodeItemList;
    }

    public List<GameObject> GetAllNodeItems()
    {
        List<GameObject> nodeItemList = new List<GameObject>();
        Debug.Log(nodeItems.Length);
        for (int i = 0; i < NodeManager.Instance().nodeCountY; i++)
        {
            for (int j = 0; j < NodeManager.Instance().nodeCountX; j++)
            {
                nodeItemList.Add(nodeItems[i, j]);
            }
        }

        return nodeItemList;
    }

}
