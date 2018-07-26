﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : Singleton<TeamManager>
{
    public Player[] players;

    public int playerNumber
    {
        get { return players.Length; }
    }

}

[System.Serializable]
public class Player
{
    public string name = "unknown";
    public Color color;
}
