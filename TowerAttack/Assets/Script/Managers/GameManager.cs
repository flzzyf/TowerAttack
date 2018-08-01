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

        GameStart();

    }

    private void Update()
    {

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
