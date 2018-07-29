using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : Singleton<AIManager>
{


	void Start () 
	{
		
	}
	

	void Update () 
	{
		
	}

    public void AIStart(int _player)
    {
        StartCoroutine(AIStartAction(_player));
    }

    IEnumerator AIStartAction(int _player)
    {
        while(GameManager.gaming)
        {


            yield return null;
        }
    }

    public class AI
    {
        public string style = "yandere";
        public float difficulty = 1;
    }
}
