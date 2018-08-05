using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    public List<Player> players;

    public int playerNumber
    {
        get { return players.Count; }
    }

    //判断两个玩家为敌对
    public bool isEnemy(int _player1, int _player2)
    {
        //只要有一个是中立，则不是敌对
        if(players[_player1].team == 0 || players[_player2].team == 0)
        {
            return false;
        }
        //只要有一个是敌对，则敌对
        if (players[_player1].team == 1 || players[_player2].team == 1)
        {
            return true;
        }
        //队伍不同则敌对
        if (players[_player1].team != players[_player2].team)
            return true;

        return false;
    }
}

[System.Serializable]
public class Player
{
    public int id;
    public string username;
    public int skin;
    //队伍：0中立，1敌对
    public int team;
    public Color color;
    public bool isAI;
    public Vector2Int startingPoint;

    public int money;

    public Player(int _id, int _team, Color _color, Vector2Int _startingPoint, bool _isAI)
    {
        id = _id;
        team = _team;
        color = _color;
        startingPoint = _startingPoint;
        isAI = _isAI;
    }
}