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

    public Text text_force;

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
            BuildManager.Instance().Build(gameObject, GameManager.Instance().player);

            playerForce[0]++;

            foreach (var item in MapManager.Instance().GetNearbyNodeItems(pos))
            {
                item.GetComponent<NodeItem>().GetComponent<NodeItem>().playerForce[0]++;
            }

            foreach (var item in MapManager.Instance().GetNearbyNodeItems(pos))
            {
                item.GetComponent<NodeItem>().UpdateBorders();
                item.GetComponent<NodeItem>().text_force.enabled = true;

                
            }

            UpdateBorders();
            //foreach (var item2 in MapManager.Instance().GetAllNodeItems())
            //{
            //    item2.GetComponent<NodeItem>().UpdateBorders();
            //}

        }
    }
    int[] borderIndex = { 7, 1, 3, 5 };
    public void UpdateBorders()
    {
        text_force.text = playerForce[0].ToString();

        for (int i = 0; i < 4; i++)
        {
            if (NodeManager.Instance().GetNearbyNode(pos, borderIndex[i]) == null)
                continue;

            if (MapManager.Instance().GetNearbyNode(gameObject, borderIndex[i]).GetComponent<NodeItem>().playerForce[0] == 0)
            {
                borders[i].SetActive(true);
            }
            else
            {
                borders[i].SetActive(false);
            }
        }
        
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
