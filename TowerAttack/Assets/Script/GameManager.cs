using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public int player = 1;

	void Start ()
    {
        MapManager.Instance().GenerateMap();
	}
	
	void Update () {
		
	}
}
