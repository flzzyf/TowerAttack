using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public int player = 0;
    public static bool gaming;
    public static bool paused;

    public GameObject panel_menu;

	void Start ()
    {
        BuildManager.Instance().Init();
        ScoreManager.Instance().Init();
        SeamlessMap.Instance().Init();
        FogOfWarManager.Instance().Init();

        GameStart();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            //BuildManager.Instance().BuildOccupiableBuilding("WatchTower", MapManager.Instance().GetNodeItemFromAbsPos(new Vector2Int(0, 0)));
            BuildManager.Instance().BuildOccupiableBuilding("Farm", MapManager.Instance().GetNodeItemFromAbsPos(new Vector2Int(0, 0)));
            //BuildManager.Instance().BuildInstantly(MapManager.Instance().GetNodeItemFromAbsPos(new Vector2Int(0, 1)), 0);
            BuildManager.Instance().BuildInstantly(MapManager.Instance().GetNodeItemFromAbsPos(new Vector2Int(2, 0)), 0);

        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            BuildManager.Instance().BuildInstantly(MapManager.Instance().GetNodeItemFromAbsPos(new Vector2Int(1, 0)), 1);

            //所有塔开始搜索目标
            BuildManager.Instance().AllTowerStartSearching();
        }

    }
    //移动镜头到节点
    public void CameraMoveToPoint(Vector2Int _pos)
    {
        Vector2Int center = new Vector2Int(NodeManager.Instance().nodeCountY / 2, NodeManager.Instance().nodeCountX / 2);
        float x = (_pos.x - center.x) * MapManager.Instance().nodePaddingY;
        float y = (_pos.y - center.y) * MapManager.Instance().nodePaddingX;
        x += 2 * MapManager.Instance().nodePaddingY;

        SeamlessMap.Instance().MoveRight(y);
        SeamlessMap.Instance().MoveUp(x);
    }

    public void GameStart()
    {
        gaming = true;

        int mapSize;
        if (PlayerManager.Instance().playerNumber == 1)
            mapSize = 1;
        else
             mapSize = (int)Mathf.Sqrt(PlayerManager.Instance().playerNumber) + 1;
        NodeManager.Instance().nodeCountX = mapSize * 7;
        NodeManager.Instance().nodeCountY = mapSize * 12;

        NodeManager.Instance().GenerateNodes();
        MapManager.Instance().GenerateMap();

        //设置出生点，在所有可能点中取玩家数个
        List<int> startingPointList = new List<int>();
        for (int i = 0; i < mapSize * mapSize; i++)
        {
            startingPointList.Add(i);
        }
        for (int i = 0; i < PlayerManager.Instance().playerNumber; i++)
        {
            //随机取一个出生点
            int a = startingPointList[Random.Range(0, startingPointList.Count - 1)];
            startingPointList.Remove(a);
            int y = a / mapSize;
            int x = a % mapSize;

            PlayerManager.Instance().players[i].startingPoint = new Vector2Int(12 * y, 7 * x);
        }

        //建造初始炮塔
        for (int i = 0; i < PlayerManager.Instance().playerNumber; i++)
        {
            Vector2Int pos = PlayerManager.Instance().players[i].startingPoint;
            BuildManager.Instance().BuildInstantly(MapManager.Instance().GetNodeItem(pos), i);

            BuildManager.Instance().BuildOccupiableBuilding("Farm", MapManager.Instance().GetNodeItem(pos + new Vector2Int(6, 0)));
            BuildManager.Instance().BuildOccupiableBuilding("WatchTower", MapManager.Instance().GetNodeItem(pos + new Vector2Int(0, 4)));

        }

        //移动镜头到出生点
        Vector2Int startingPoint = PlayerManager.Instance().players[0].startingPoint;
        CameraMoveToPoint(startingPoint);

        //开启AI
        for (int i = 0; i < PlayerManager.Instance().playerNumber; i++)
        {
            if (PlayerManager.Instance().players[i].isAI)
            {
                AIManager.Instance().AIStart(PlayerManager.Instance().players[i].id);
            }
        }

    }

    public void OpenMenu()
    {
        Time.timeScale = 0;
        panel_menu.SetActive(true);
        paused = true;
    }

    public void CloseMenu()
    {
        Time.timeScale = 1;
        panel_menu.SetActive(false);
        paused = false;
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1;

        Destroy(GameObject.Find("DontDestroyOnLoad"));

        UnityEngine.SceneManagement.SceneManager.LoadScene("Intro");

    }

}
