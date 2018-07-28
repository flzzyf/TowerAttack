using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public int player = 1;
    [HideInInspector]
    public bool gaming;

    //镜头操控方式：RTS式，触屏拖动式
    public int cameraControlMode = 0;

	void Start ()
    {

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

        NodeManager.Instance().GenerateNodes();
        MapManager.Instance().GenerateMap();

        BuildManager.Instance().Build(MapManager.Instance().nodeItems[2, 3], 1);
        BuildManager.Instance().Build(MapManager.Instance().nodeItems[8, 6], 2);
    }

}
