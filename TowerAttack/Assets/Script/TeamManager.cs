using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : Singleton<TeamManager>
{
    public Player[] players;
	void Start () 
	{
		
	}
	

	void Update () 
	{
		
	}
}

[System.Serializable]
public class Player
{
    public string name = "unknown";
    public Color color;
}
