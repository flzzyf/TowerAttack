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

    public void AddNodeToPlayerVision(int _player, GameObject _go)
    {
        playerVisionNodes[_player].Add(_go);

        if(_player == GameManager.Instance().player)
        {
            _go.GetComponent<NodeItem>().ToggleFogOfWar(false);
        }
    }

    public void AddNodesWithinRangeToPlayerVision(int _player, GameObject _go, int _range)
    {
        foreach (GameObject item in MapManager.Instance().GetNearbyNodesWithinRange(_go, _range))
        {
            if(!playerVisionNodes[_player].Contains(item))
            {
                AddNodeToPlayerVision(_player, item);
            }
        }
    }
}
