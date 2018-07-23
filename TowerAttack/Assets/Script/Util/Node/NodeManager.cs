using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : Singleton<NodeManager>
{
    public int nodeCountX = 20;
    public int nodeCountY = 20;

    [HideInInspector]
    Node[,] nodes;

    //顺序：从12点钟开始顺时针
    Vector2Int[,] nearbyNodeOffset = { { new Vector2Int(2, 0), new Vector2Int(1, 0), new Vector2Int(0, 1), new Vector2Int(-1, 0),
                                   new Vector2Int(-2, 0), new Vector2Int(-1, -1), new Vector2Int(0, -1), new Vector2Int(1, -1)},
                                    { new Vector2Int(2, 0), new Vector2Int(1, 1), new Vector2Int(0, 1), new Vector2Int(-1, 1),
                                   new Vector2Int(-2, 0), new Vector2Int(-1, 0), new Vector2Int(0, -1), new Vector2Int(1, 0) }};

    public void GenerateNodes()
    {
        nodes = new Node[nodeCountY, nodeCountX];
        
        //生成节点
        for (int i = 0; i < nodeCountY; i++)
        {
            for (int j = 0; j < nodeCountX; j++)
            {
                nodes[i, j] = new Node(i, j);
            }
        }
    }
    
    //获取节点周围节点
    public Node GetNode(Node _node, int _index)
    {
        int evenOrUneven = _node.pos.x % 2 == 0 ? 0 : 1;
        Vector2Int targetOffset = _node.pos + nearbyNodeOffset[evenOrUneven, _index];

        Node targetNode = nodes[targetOffset.x, targetOffset.y];
        return targetNode;
    }

    public Node GetNode(int _x, int _y, int _index)
    {
        return GetNode(nodes[_x, _y], _index);
    }

    public Node GetNode(Vector2Int _pos, int _index)
    {
        return GetNode(_pos.x, _pos.y, _index);
    }

}
