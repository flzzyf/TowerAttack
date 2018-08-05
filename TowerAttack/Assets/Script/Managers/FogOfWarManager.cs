using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWarManager : Singleton<FogOfWarManager>
{
    public List<GameObject>[] playerVisionNodes;

    public void Init()
    {
        playerVisionNodes = new List<GameObject>[PlayerManager.Instance().playerNumber];
        for (int i = 0; i < PlayerManager.Instance().playerNumber; i++)
        {
            playerVisionNodes[i] = new List<GameObject>();
        }
    }
    //添加节点到玩家视野
    public void AddNodeToPlayerVision(int _player, GameObject _go)
    {
        playerVisionNodes[_player].Add(_go);

        if(_player == GameManager.Instance().player)
        {
            _go.GetComponent<NodeItem>().ToggleFogOfWar(false);
        }
    }

    //添加范围内节点到玩家视野
    public void AddNodesWithinRangeToPlayerVision(int _player, GameObject _go, int _range)
    {
        foreach (GameObject item in MapManager.Instance().GetNodesWithinRange(_go, _range))
        {
            item.GetComponent<NodeItem>().playerVision[_player]++;
            //目前未包含
            if (!playerVisionNodes[_player].Contains(item))
            {
                AddNodeToPlayerVision(_player, item);
            }
        }
    }

    public void RemoveNodesWithinRangeToPlayerVision(int _player, GameObject _go, int _range)
    {
        foreach (GameObject item in MapManager.Instance().GetNodesWithinRange(_go, _range))
        {
            item.GetComponent<NodeItem>().playerVision[_player]--;
            if (playerVisionNodes[_player].Contains(item))
            {
                if(item.GetComponent<NodeItem>().playerVision[_player] <= 0)
                {
                    playerVisionNodes[_player].Remove(item);
                    if (_player == GameManager.Instance().player)
                        item.GetComponent<NodeItem>().ToggleFogOfWar(true);

                }
            }
        }
    }

    //节点对玩家可见
    public bool NodeVisible(GameObject _go, int _player)
    {
        if (playerVisionNodes[_player].Contains(_go))
        {
            return true;
        }

        return false;
    }
}
