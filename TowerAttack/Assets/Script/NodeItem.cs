using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeItem : MonoBehaviour
{
    Animator animator;

    [HideInInspector]
    public GameObject tower;

    Color originColor;

    [HideInInspector]
    public Vector2Int pos;

    public GameObject text_force;

    [HideInInspector]
    public int[] playerForce = new int[8];
    public GameObject[] borders;

    void Start()
    {
        animator = GetComponent<Animator>();

        originColor = GetComponentInChildren<SpriteRenderer>().color;
    }

    private void OnMouseEnter()
    {
        animator.SetBool("hovered", true);
    }

    private void OnMouseExit()
    {
        animator.SetBool("hovered", false);
    }

    private void OnMouseDown()
    {
        if (tower == null)
        {
            BuildManager.Instance().Build(MapManager.Instance().GetNodeItem(pos), GameManager.Instance().player);

        }
    }

    public void BuildSetting()
    {
        playerForce[0]++;

        foreach (var item in MapManager.Instance().GetNearbyNodeItems(pos))
        {
            item.GetComponent<NodeItem>().GetComponent<NodeItem>().playerForce[0]++;
        }

        foreach (var item in MapManager.Instance().GetNearbyNodeItems(pos))
        {
            item.GetComponent<NodeItem>().UpdateBorders();
            if (item.GetComponent<NodeItem>().tower == null)
                item.GetComponent<NodeItem>().UpdateForceText();
        }

        UpdateBorders();
    }

    int[] borderIndex = { 7, 1, 3, 5 };
    //更新边界
    public void UpdateBorders()
    {
        for (int i = 0; i < 4; i++)
        {
            if (NodeManager.Instance().GetNearbyNode(pos, borderIndex[i]) == null ||
                MapManager.Instance().GetNearbyNode(gameObject, borderIndex[i]).GetComponent<NodeItem>().playerForce[0] == 0)
            {
                borders[i].SetActive(true);
            }
            else
            {
                borders[i].SetActive(false);
            }
        }
    }
    public void UpdateForceText()
    {
        if (!text_force.activeSelf)
            text_force.SetActive(true);

        text_force.GetComponent<Text>().text = playerForce[0].ToString();

    }

    public void ChangeColor(Color _color = default(Color))
    {
        if (_color == default(Color))
        {
            GetComponentInChildren<SpriteRenderer>().color = originColor;
        }
        else
        {
            GetComponentInChildren<SpriteRenderer>().color = _color;

        }
    }
}
