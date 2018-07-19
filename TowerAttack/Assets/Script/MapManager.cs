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
        for (int i = 0; i < mapSizeY; i++)
        {
            for (int j = 0; j < mapSizeX; j++)
            {
                Vector2 pos = new Vector2(i * nodePaddingX, j * nodePaddingY);
                pos += originPoint;
                GameObject go = Instantiate(prefab_node, pos, Quaternion.identity);
            }
        }
    }
}
