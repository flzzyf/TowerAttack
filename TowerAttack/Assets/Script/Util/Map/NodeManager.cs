using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : Singleton<NodeManager>
{
    public int nodeCountX = 20;
    public int nodeCountY = 20;

    [HideInInspector]
    Node[,] nodes;

    Vector2[] nearbyNodeOffset = { new Vector2(-1, 0) };

    public void GenerateNodes()
    {
        nodes = new Node[nodeCountX, nodeCountY];
        
        //生成节点
        for (int i = 0; i < nodeCountY; i++)
        {
            for (int j = 0; j < nodeCountX; j++)
            {
                nodes[j, i] = new Node(j, i);
            }
        }
    }

    public void GetNode()
    {

    }
}
