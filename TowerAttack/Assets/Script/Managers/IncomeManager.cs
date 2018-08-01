using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IncomeManager : Singleton<IncomeManager>
{
    public Text text_money;
    public Text text_rate;
    public Text text_income;

    public float globalIncomeTime = 10;
    float[] incomeTimeCounter;
    public float[] incomeTimeIncreaseRates;

    public Slider paybackTimeSlider;

    public void Init()
    {
        incomeTimeCounter = new float[PlayerManager.Instance().playerNumber];
        incomeTimeIncreaseRates = new float[PlayerManager.Instance().playerNumber];

    }

    void Update()
    {
        for (int i = 0; i < PlayerManager.Instance().playerNumber; i++)
        {
            if (i == GameManager.Instance().player)
                paybackTimeSlider.value = incomeTimeCounter[i];

            if (incomeTimeCounter[i] >= 1)
            {
                incomeTimeCounter[i] = 0;

                IncreaseMoney(i);
            }
            else
            {
                incomeTimeCounter[i] += (1 + incomeTimeIncreaseRates[i]) * Time.deltaTime / globalIncomeTime;
            }
        }
        
    }

    void IncreaseMoney(int _player)
    {
        if (PlayerManager.Instance().players[_player].money == 0)
        {
            SetMoney(_player, 1);
        }
        else
        {
            ModifyMoney(_player, 1.5f);
        }
        
    }

    //设置玩家金钱
    public void SetMoney(int _player, int _amount)
    {
        PlayerManager.Instance().players[_player].money = _amount;

        //现在只考虑单机模式，即玩家1才更新UI
        if (_player == GameManager.Instance().player)
        {
            text_money.text = _amount.ToString();
            text_income.text = (int)(_amount * 0.5f) + "";

        }
    }

    public void ModifyMoney(int _player, float _multiple)
    {
        SetMoney(_player, (int)(PlayerManager.Instance().players[_player].money * _multiple));
    }

    public void SetIncomeRate(int _player, float _amount)
    {
        incomeTimeIncreaseRates[_player] += _amount;
        text_rate.text = (1 + incomeTimeIncreaseRates[_player]) * 100 + "%";
    }

}
