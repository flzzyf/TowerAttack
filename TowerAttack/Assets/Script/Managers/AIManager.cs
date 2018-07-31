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
        /*AI战斗风格
            探险家：随机八个方向建造
            野性生长：永远只朝一个方向前进
        */
        public string style = "yandere";
        public float difficulty = 1;
    }
}
