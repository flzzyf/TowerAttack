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
        Vector2Int startingPoint = PlayerManager.Instance().players[_player].startingPoint;

        while(GameManager.gaming)
        {
            foreach (GameObject item in MapManager.Instance().GetNearbyNodesInRange(MapManager.Instance().GetNodeItem(startingPoint), 2))
            {
                if (item.GetComponent<NodeItem>().tower == null)
                {
                    BuildManager.Instance().Build(item, _player);

                    break;
                }
            }


            yield return new WaitForSeconds(3);
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
