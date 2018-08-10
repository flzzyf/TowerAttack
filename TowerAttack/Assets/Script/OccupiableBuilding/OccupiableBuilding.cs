using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OccupiableBuilding : MonoBehaviour
{
    [HideInInspector]
    public int player = -1;
    [HideInInspector]
    public GameObject node;
    //已经被占领
    bool isOccupied;
    //争夺中
    bool isInConflict;
    [HideInInspector]
    public List<GameObject> nearbyNodes;
    [HideInInspector]
    public int[] occupiedCountByPlayer;

    public virtual void BuildSetting(GameObject _node)
    {
        node = _node;

        occupiedCountByPlayer = new int[PlayerManager.Instance().playerNumber];

        nearbyNodes = new List<GameObject>();
        foreach (var item in MapManager.Instance().GetNodesWithinRange(_node, 1))
        {
            //添加到周围节点列表，以备后用
            nearbyNodes.Add(item);
            //周围节点父级可占领物体设为自己
            item.GetComponent<NodeItem>().occupiableNodeParent = gameObject;
        }
        foreach (var item in nearbyNodes)
        {
            item.GetComponent<NodeItem>().OccupiableBuildSetting();
        }

    }

    //占领
    public virtual void Occupy(int _player)
    {
        occupiedCountByPlayer[_player]++;

        if(!isOccupied)
        {
            //未被占领
            if(!isInConflict)
            {
                //未处于争夺中，占领
                player = _player;
                OccupiedEffect();
            }
        }
        else
        {
            //已被占领
            if (player != _player)
            {
                //被不同玩家占领，取消其占领
                LiberatedEffect();
                //设为处于争夺中
                isInConflict = true;
            }
        }
    }
    public virtual void OccupiedEffect()
    {
        //print("占领:" + player);
        isOccupied = true;

        foreach (var item in nearbyNodes)
        {
            item.GetComponent<NodeItem>().OccupiableBuildSetting();
        }
    }

    //被解放
    public virtual void Liberate(int _player)
    {
        occupiedCountByPlayer[_player]--;

        //该玩家已经没有点数
        if(occupiedCountByPlayer[_player] == 0)
        {
            if(isOccupied)
            {
                //被占领
                LiberatedEffect();
            }
            else
            {
                if(isInConflict)
                {
                    //争夺中
                    //只被一个玩家占领
                    int p = OnlyOccupiedByOnePlayer();
                    if (p != -1)
                    {
                        player = p;
                        OccupiedEffect();
                    }
                }
            }
        }
    }

    public virtual void LiberatedEffect()
    {
        //print("解放:" + player);
        isOccupied = false;
        player = -1;

        foreach (var item in nearbyNodes)
        {
            item.GetComponent<NodeItem>().OccupiableBuildSetting();
        }
    }

    //只被一个玩家占领则返回他的ID，否则返回-1（被多个玩家占领)
    public int OnlyOccupiedByOnePlayer()
    {
        int p = -1;
        for (int i = 0; i < PlayerManager.Instance().playerNumber; i++)
        {
            if (occupiedCountByPlayer[i] > 0)
            {
                if(p == -1)
                    p = i;
                else
                    return -1;
            }
        }

        return p;
    }

    public int OccupiedPlayerCount()
    {
        int count = 0;
        for (int i = 0; i < PlayerManager.Instance().playerNumber; i++)
        {
            if (occupiedCountByPlayer[i] > 0)
                count++;
        }

        return count;
    }
}
