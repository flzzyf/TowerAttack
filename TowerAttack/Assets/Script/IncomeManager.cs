using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IncomeManager : Singleton<IncomeManager>
{
    public Text text_money;
    int[] money;

    public float moneyIncreaseTime = 10;
    float moneyIncreaseCD = 0;

    void Start () 
	{
        money = new int[TeamManager.Instance().playerNumber];
	}

    void Update()
    {
        if (moneyIncreaseCD <= 0)
        {
            moneyIncreaseCD = moneyIncreaseTime;

            IncreaseMoney();
        }
        else
        {
            moneyIncreaseCD -= Time.deltaTime;
        }
    }

    void IncreaseMoney()
    {
        //if (money == 0)
        //{
        //    money++;
        //}
        //else
        //{
        //    money *= 2;
        //}

        //text_money.text = money.ToString();
    }
}
