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
            for (int j = 0; j < mapSizeX; j++)
            {
                Vector2 pos = new Vector2(i * nodePaddingX, j * nodePaddingY);
                pos += originGeneratePoint;
                GameObject go = Instantiate(prefab_node, pos, Quaternion.identity, ParentManager.Instance().GetParent("Nodes"));
            }
        }
    }
}
