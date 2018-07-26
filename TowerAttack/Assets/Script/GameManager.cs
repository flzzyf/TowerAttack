using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public int player = 1;

	void Start ()
    {
     

    }
    int a = 0;

    private void Update()
    {
       
    }

    public void GameStart()
    {
        NodeManager.Instance().GenerateNodes();
        MapManager.Instance().GenerateMap();

        //BuildManager.Instance().Build(MapManager.Instance().nodeItems[2, 3], 1);
        //BuildManager.Instance().Build(MapManager.Instance().nodeItems[8, 6], 2);
    }

}
