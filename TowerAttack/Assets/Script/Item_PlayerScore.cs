using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item_PlayerScore : MonoBehaviour
{
    public Text text_player;
    public Text text_score;

    public void SetItem(int _player)
    {
        text_player.text = PlayerManager.Instance().players[_player].username;
        text_player.color = PlayerManager.Instance().players[_player].color;
        text_score.text = ScoreManager.Instance().score[_player] + "";
    }
}
