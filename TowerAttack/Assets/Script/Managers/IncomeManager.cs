using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IncomeManager : Singleton<IncomeManager>
{
    public Text text_money;

    public float globalPayBackTime = 10;
    float[] paybackTime;
    public float[] paybackTimeIncreaseAmount;

    public Slider paybackTimeSlider;

    public void Init()
    {
        paybackTime = new float[PlayerManager.Instance().playerNumber];
        paybackTimeIncreaseAmount = new float[PlayerManager.Instance().playerNumber];
    }

    void Update()
    {
        for (int i = 0; i < PlayerManager.Instance().playerNumber; i++)
        {
            paybackTimeSlider.value = paybackTime[i];

            if (paybackTime[i] >= 1)
            {
                paybackTime[i] = 0;

                IncreaseMoney(i);
            }
            else
            {
                paybackTime[i] += (1 + paybackTimeIncreaseAmount[i]) * Time.deltaTime / globalPayBackTime;
            }
        }
        
    }

    void IncreaseMoney(int _player)
    {
        if (PlayerManager.Instance().players[_player].money == 0)
        {
            PlayerManager.Instance().players[_player].money++;
        }
        else
        {
            PlayerManager.Instance().players[_player].money *= 2;
        }
        //现在只考虑单机模式，即玩家1才更新UI
        if(_player == GameManager.Instance().player)
            text_money.text = PlayerManager.Instance().players[_player].money.ToString();
    }


}
