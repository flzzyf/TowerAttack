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
        IncomeManager.Instance().Init();
        SoundManager.Instance().Init();
        SeamlessMap.Instance().Init();
        FogOfWarManager.Instance().Init();

        GameStart();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.D))
        {
            foreach (GameObject item in MapManager.Instance().GetNearbyNodesWithinRange(new Vector2Int(6, 6), 2))
            {
                item.SetActive(false);
            }
        }
        
    }

    public void GameStart()
    {
        gaming = true;

        NodeManager.Instance().nodeCountX = PlayerManager.Instance().playerNumber * 7;
        NodeManager.Instance().nodeCountY = PlayerManager.Instance().playerNumber * 12;

        NodeManager.Instance().GenerateNodes();
        MapManager.Instance().GenerateMap();

        //设置玩家初始金钱
        for (int i = 0; i < PlayerManager.Instance().playerNumber; i++)
        {
            IncomeManager.Instance().SetMoney(i, 10);
        }

        //建造初始炮塔
        for (int i = 0; i < PlayerManager.Instance().playerNumber; i++)
        {
            Vector2Int pos = PlayerManager.Instance().players[i].startingPoint;
            BuildManager.Instance().BuildInstantly(MapManager.Instance().GetNodeItem(pos), i);
        }

        //移动镜头到出生点

        Vector2Int pos2 = PlayerManager.Instance().players[0].startingPoint;
        float centerPosX = NodeManager.Instance().nodeCountX / 2;
        float x = centerPosX - pos2.x;
        x *= MapManager.Instance().nodePaddingX;
        SeamlessMap.Instance().MoveRight(-x);

        float centerPosY = NodeManager.Instance().nodeCountY / 2;
        float y = centerPosX - pos2.y;
        y *= MapManager.Instance().nodePaddingY;
        SeamlessMap.Instance().MoveUp(-y);

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
