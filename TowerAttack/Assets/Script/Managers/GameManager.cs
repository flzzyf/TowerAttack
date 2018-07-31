using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public int player = 0;
    [HideInInspector]
    public static bool gaming;

	void Start ()
    {
        //CameraControl_RTS.Instance().enabled = false;
        //CameraControl_Touch.Instance().enabled = false;

    }

    private void Update()
    {
        //延时开始游戏，等其他Manager初始化完成
       if(!gaming && Time.timeSinceLevelLoad > Time.deltaTime)
        {
            GameStart();
        }
    }

    public void GameStart()
    {
        gaming = true;

        NodeManager.Instance().nodeCountX = PlayerManager.Instance().playerNumber * 7;
        NodeManager.Instance().nodeCountY = PlayerManager.Instance().playerNumber * 13;

        NodeManager.Instance().GenerateNodes();
        MapManager.Instance().GenerateMap();

        //建造初始炮塔
        for (int i = 0; i < PlayerManager.Instance().playerNumber; i++)
        {
            BuildManager.Instance().Build(MapManager.Instance().GetNodeItem(PlayerManager.Instance().players[i].startingPoint), i);
        }

        //开启AI
        for (int i = 0; i < PlayerManager.Instance().playerNumber; i++)
        {
            if(PlayerManager.Instance().players[i].isAI)
            {
                AIManager.Instance().AIStart(PlayerManager.Instance().players[i].id);
            }
        }
    }

}
