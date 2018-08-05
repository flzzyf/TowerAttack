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
        //Vector2Int startingPoint = PlayerManager.Instance().players[_player].startingPoint;

        while(GameManager.gaming)
        {
            yield return new WaitForSeconds(1);

            //获取视野内随机节点
            int index = Random.Range(0, FogOfWarManager.Instance().playerVisionNodes[_player].Count - 1);
            GameObject node = FogOfWarManager.Instance().playerVisionNodes[_player][index];
            //在它周围可用点建造
            foreach (GameObject item in MapManager.Instance().GetNearbyNodesWithinRange(node, 2))
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
