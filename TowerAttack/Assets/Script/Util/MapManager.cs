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
        originGeneratePoint.x = originPoint.x - mapSizeX / 2 * nodePaddingX;
        if(mapSizeX % 2 == 0)
        {
            //originGeneratePoint.x -= nodePaddingX / 2;
        }
        originGeneratePoint.y = originPoint.y - mapSizeY / 2 * nodePaddingY;
        if (mapSizeY % 2 == 0)
        {
            //originGeneratePoint.y -= nodePaddingY / 2;
        }
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
                GameObject go = Instantiate(prefab_node, pos, Quaternion.identity, ParentManager.Instance().GetParent("Node"));
                go.name = "Node_" + i + "_" + j;
                nodeItems[i, j] = go;
                go.GetComponent<NodeItem>().pos = new Vector2Int(i, j);
                go.GetComponent<NodeItem>().absPos = new Vector2Int(i, j);
                go.GetComponent<NodeItem>().SetOrderInLayer(2 * (mapSizeY - i));

                go.GetComponent<NodeItem>().ToggleFogOfWar(true);

            }
        }
    }

    public GameObject GetNearbyNode(GameObject _go, int _index)
    {
        NodeItem nodeItem = _go.GetComponent<NodeItem>();
        Node node = NodeManager.Instance().GetNearbyNode(nodeItem.absPos, _index);
        return GetNodeItemFromAbsPos(node.pos);
    }

    public GameObject GetNodeItem(Vector2Int _pos)
    {
        return nodeItems[_pos.x, _pos.y];
    }

    public GameObject GetNodeItemFromAbsPos(Vector2Int _pos)
    {
        for (int i = 0; i < NodeManager.Instance().nodeCountY; i++)
        {
            for (int j = 0; j < NodeManager.Instance().nodeCountX; j++)
            {
                if (nodeItems[i, j].GetComponent<NodeItem>().absPos == _pos)
                    return nodeItems[i, j];
            }
        }
        return null;
    }
    //获取相邻所有节点
    public List<GameObject> GetNearbyNodeItems(GameObject _go)
    {
        Vector2Int pos = _go.GetComponent<NodeItem>().absPos;

        List<GameObject> nodeItemList = new List<GameObject>();
        foreach (var item2 in NodeManager.Instance().GetNearbyNodes(pos))
        {
            nodeItemList.Add(GetNodeItemFromAbsPos(item2.pos));
        }
    
        return nodeItemList;
    }

    //获取所有节点
    public List<GameObject> GetAllNodeItems()
    {
        List<GameObject> nodeItemList = new List<GameObject>();
        for (int i = 0; i < NodeManager.Instance().nodeCountY; i++)
        {
            for (int j = 0; j < NodeManager.Instance().nodeCountX; j++)
            {
                nodeItemList.Add(nodeItems[i, j]);
            }
        }

        return nodeItemList;
    }

    //获取范围内所有节点
    public List<GameObject> GetNodesWithinRange(GameObject _go, int _range)
    {
        List<GameObject> list = new List<GameObject>();
        Vector2Int pos = _go.GetComponent<NodeItem>().absPos;

        Node node = NodeManager.Instance().GetNode(pos);

        foreach (Node item in NodeManager.Instance().GetNodesWithinRange(node, _range))
        {
            GameObject go = GetNodeItemFromAbsPos(item.pos);
            list.Add(go);
        }

        return list;
    }

    public List<GameObject> GetNearbyNodesWithinRange(GameObject _go, int _range)
    {
        List<GameObject> list = GetNearbyNodesWithinRange(_go.GetComponent<NodeItem>().absPos, _range);
        list.Remove(_go);

        return list;
    }

    public List<GameObject> GetNearbyNodesWithinRange(Vector2Int _pos, int _range)
    {
        GameObject go = GetNodeItemFromAbsPos(_pos);
        return GetNodesWithinRange(go, _range);
    }

}
