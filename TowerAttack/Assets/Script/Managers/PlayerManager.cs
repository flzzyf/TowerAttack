using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour 
{


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
    public int id;
    public string userID;
    public bool isAI;
    public Vector2Int startingPoint;
    public string name = "unknown";
    public Color color;
}