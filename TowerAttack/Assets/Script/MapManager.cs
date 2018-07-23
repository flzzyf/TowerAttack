using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : Singleton<MapManager>
{
    public GameObject prefab_node;
    public int mapSizeX = 20;
    public int mapSizeY = 20;
    public float nodePaddingX = 1;
    public float nodePaddingY = 1;
    public Vector2 originPoint;

    public void GenerateMap()
    {
        //生成节点的真正源点
        Vector2 originGeneratePoint;
        originGeneratePoint.x = originPoint.x - (float)mapSizeX / 2 * nodePaddingX + nodePaddingX / 2;
        originGeneratePoint.y = originPoint.y - (float)mapSizeY / 2 * nodePaddingY + nodePaddingY / 2;
        //生成节点
        for (int i = 0; i < mapSizeY; i++)
        {
            float specialX = i % 2 == 0 ? 0 : nodePaddingX / 2;

            for (int j = 0; j < mapSizeX; j++)
            {
                float y = i * nodePaddingY;
                Vector2 pos = new Vector2(j * nodePaddingX + specialX, y);
                pos += originGeneratePoint;
                GameObject go = Instantiate(prefab_node, pos, Quaternion.identity, ParentManager.Instance().GetParent("Nodes"));
                go.name = "Node_" + i + "_" + j;

                go.GetComponentInChildren<SpriteRenderer>().sortingOrder = mapSizeY - i;
            }
        }
    }
}
