using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public int player = 1;

	public Text text_money;
	int money = 0;

	public float moneyIncreaseTime = 10;
	float moneyIncreaseCD = 0;

	void Start ()
    {
        NodeManager.Instance().GenerateNodes();
        MapManager.Instance().GenerateMap();
    }
	
	void Update () 
	{
		if(moneyIncreaseCD <= 0)
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
		if(money == 0)
		{
			money++;
		}
		else
		{
			money *= 2;
		}

		text_money.text = money.ToString();
	}
}
