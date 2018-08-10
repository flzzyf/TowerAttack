using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : Singleton<ScoreManager>
{
    public Text text_score;
    public Text text_farmer;
    public Text text_worker;
    public Slider slider_score;
    public Slider slider_farmer;
    public Slider slider_worker;
    //人口滑动条速度
    public float sliderSpeed = 1;
    //分数增加间隔
    public float scoreIncreaseInterval = 2;
    float scoreIncreaseCounter;

    [HideInInspector]
    public int[] score;
    [HideInInspector]
    public int[] population_farmer;
    [HideInInspector]
    public int[] population_worker;

    public GameObject panel_score;
    public GameObject item_playerScore;
    List<int> playersOnScoreboard = new List<int>();

    public void Init()
    {
        score = new int[PlayerManager.Instance().playerNumber];
        population_farmer = new int[PlayerManager.Instance().playerNumber];
        population_worker = new int[PlayerManager.Instance().playerNumber];

        slider_score.value = 0;
        slider_farmer.value = 1;
        slider_worker.value = 0;
        //设置初始工人为10
        for (int i = 0; i < PlayerManager.Instance().playerNumber; i++)
        {
            population_farmer[i] = 10;
        }

        Destroy(panel_score.transform.GetChild(0).gameObject);
    }

    void Update()
    {
        if(scoreIncreaseCounter <= 0)
        {
            scoreIncreaseCounter = scoreIncreaseInterval;
            //增加得分
            for (int i = 0; i < PlayerManager.Instance().playerNumber; i++)
            {
                ModifyScore(i, population_farmer[i]);
            }
        }
        else
        {
            scoreIncreaseCounter -= Time.deltaTime;
        }

        for (int i = 0; i < PlayerManager.Instance().playerNumber; i++)
        {
            //为本地玩家
            if (i == GameManager.Instance().player)
            {
                //修改UI值
                text_score.text = score[i] + "";
                slider_score.value = Mathf.Lerp(slider_score.value, (float)score[i] / 1000, sliderSpeed * Time.deltaTime);

                float ratio_farmer = (float)population_farmer[i] / (population_farmer[i] + population_worker[i]);
                slider_farmer.value = Mathf.Lerp(slider_farmer.value, ratio_farmer, sliderSpeed * Time.deltaTime);

                float ratio_worker = (float)population_worker[i] / (population_farmer[i] + population_worker[i]);
                slider_worker.value = Mathf.Lerp(slider_worker.value, ratio_worker, sliderSpeed * Time.deltaTime);
            }
        }

        //更新快完成玩家的分数
        UpdateScoreboard();
    }

    //更新计分板
    void UpdateScoreboard()
    {
        int[] s = new int[playersOnScoreboard.Count];

        for (int i = 0; i < playersOnScoreboard.Count; i++)
        {
            s[i] = i;
        }

        for (int i = 0; i < playersOnScoreboard.Count - 1; i++)
        {
            for (int j = i; j < playersOnScoreboard.Count - 1; j++)
            {
                if (score[s[j]] < score[s[j + 1]])
                {
                    int temp = s[j];
                    s[j] = s[j + 1];
                    s[j + 1] = temp;
                }
            }
        }

        for (int i = 0; i < playersOnScoreboard.Count; i++)
        {
            panel_score.transform.GetChild(i).GetComponent<Item_PlayerScore>().SetItem(s[i]);
        }

    }
    //添加玩家到计分板
    void AddPlayerToScoreboard(int _player)
    {
        playersOnScoreboard.Add(_player);

        Instantiate(item_playerScore, panel_score.transform);
    }

    public void ModifyWorker(int _player, int _number)
    {
        ModifyFarmer(_player, -1 * _number);
        population_worker[_player] += _number;

        if (_player == GameManager.Instance().player)
            text_worker.text = population_worker[_player] + "";
    }

    public void ModifyFarmer(int _player, int _number)
    {
        population_farmer[_player] += _number;

        if (_player == GameManager.Instance().player)
            text_farmer.text = population_farmer[_player] + "";

    }

    public void ModifyScore(int _player, int _number)
    {
        score[_player] += _number;

        if(GameManager.gaming && score[_player] >= 1000)
        {
            GameManager.Instance().GameOver(_player);
        }
        else
        {
            if(score[_player] >= 0)
            {
                //显示计分板
                if (!panel_score.activeSelf)
                    panel_score.SetActive(true);
                //玩家不再计分板上
                if (!playersOnScoreboard.Contains(_player))
                    AddPlayerToScoreboard(_player);
            }
        }
    }
}
