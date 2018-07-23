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
    GameObject[,] nodeItems;

    public void GenerateMap()
    {
        int mapSizeX = NodeManager.Instance().nodeCountX;
        int mapSizeY = NodeManager.Instance().nodeCountY;

        nodeItems = new GameObject[mapSizeX, mapSizeY];

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
                nodeItems[j, i] = go;

                go.GetComponentInChildren<SpriteRenderer>().sortingOrder = mapSizeY - i;
            }
        }
    }
    
}
