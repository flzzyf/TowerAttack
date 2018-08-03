using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    public List<Player> players;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public int playerNumber
    {
        get { return players.Count; }
    }
}

[System.Serializable]
public class Player
{
    public int id;
    public string username;
    public int skin;
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