using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IncomeManager : Singleton<IncomeManager>
{
    public Text text_money;
    public Text text_farmer;
    public Text text_worker;

    public float globalIncomeTime = 10;
    float[] incomeTimeCounter;
    public float[] incomeTimeIncreaseRates;

    public Slider slider_paybackTime;
    public Slider slider_farmer;
    public Slider slider_worker;

    int[] income;

    [HideInInspector]
    public int[] population;
    [HideInInspector]
    public int[] population_worker;

    public float sliderSpeed = 1;

    public void Init()
    {
        incomeTimeCounter = new float[PlayerManager.Instance().playerNumber];
        incomeTimeIncreaseRates = new float[PlayerManager.Instance().playerNumber];
        income = new int[PlayerManager.Instance().playerNumber];
        population = new int[PlayerManager.Instance().playerNumber];
        population_worker = new int[PlayerManager.Instance().playerNumber];

        for (int i = 0; i < PlayerManager.Instance().playerNumber; i++)
        {
            population[i] = 10;
        }

        slider_farmer.value = 1;
        slider_worker.value = 0;
    }

    void Update()
    {
        for (int i = 0; i < PlayerManager.Instance().playerNumber; i++)
        {
            if (i == GameManager.Instance().player)
                slider_paybackTime.value = incomeTimeCounter[i];

            if (incomeTimeCounter[i] >= 1)
            {
                incomeTimeCounter[i] = 0;

                IncreaseMoney(i);
            }
            else
            {
                incomeTimeCounter[i] += (1 + incomeTimeIncreaseRates[i]) * Time.deltaTime / globalIncomeTime;
            }

            if(i == GameManager.Instance().player)
            {
                //修改UI值
                text_farmer.text = population[i] - population_worker[i] + "";
                text_worker.text = population_worker[i] + "";
                float ratio_farmer = (float)(population[i] - population_worker[i]) / population[i];
                slider_farmer.value = Mathf.Lerp(slider_farmer.value, ratio_farmer, sliderSpeed * Time.deltaTime);
                float ratio_worker = (float)population_worker[i] / population[i];
                slider_worker.value = Mathf.Lerp(slider_worker.value, ratio_worker, sliderSpeed * Time.deltaTime);
            }
        }
    }

    public void ModifyWorker(int _player, int _number)
    {
        population_worker[_player] += _number;
    }

    public void ModifyPopulation(int _player, int _number)
    {
        population[_player] += _number;
    }

    void IncreaseMoney(int _player)
    {
        ModifyMoney(_player, income[_player]);

    }

    //设置玩家金钱
    public void SetMoney(int _player, int _amount)
    {
        PlayerManager.Instance().players[_player].money = _amount;

        if (_amount <= 1)
            income[_player] = 1;
        else
            income[_player] = (int)(_amount * 0.5f);

        //现在只考虑单机模式，即玩家1才更新UI
        if (_player == GameManager.Instance().player)
        {
            text_money.text = _amount.ToString();
            text_farmer.text = income[_player] + "";

        }
    }
    public void ModifyMoney(int _player, int _amount)
    {
        SetMoney(_player, (PlayerManager.Instance().players[_player].money + _amount));

    }
    public void ModifyMoney(int _player, float _multiple)
    {
        SetMoney(_player, (int)(PlayerManager.Instance().players[_player].money * _multiple));
    }

    public void SetIncomeRate(int _player, float _amount)
    {
        incomeTimeIncreaseRates[_player] += _amount;
    }

}
