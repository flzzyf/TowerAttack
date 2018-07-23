using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node 
{
    [HideInInspector]
    public Vector2Int pos;  //行列

    public Node(int _x, int _y)
    {
        pos.x = _x;
        pos.y = _y;
    }

}
