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

    public Vector2Int pos;
    public Vector2Int absPos;

    public GameObject text_force;

    [HideInInspector]
    public int[] playerForce = new int[8];
    public GameObject[] borders;

    public SpriteRenderer gfx;

    float clickConfirmDelay = .2f;
    float mouseDownTime;

    public GameObject towerPlacement;

    public GameObject fog;

    void Start()
    {
        animator = GetComponent<Animator>();

        originColor = GetComponentInChildren<SpriteRenderer>().color;

        towerPlacement.SetActive(false);

        //ToggleFogOfWar(true);
    }

    private void OnMouseEnter()
    {
        if (animator != null)
            animator.SetBool("hovered", true);
    }

    private void OnMouseExit()
    {
        animator.SetBool("hovered", false);
    }

    private void OnMouseDown()
    {
        mouseDownTime = Time.time;
    }

    private void OnMouseUp()
    {
        //非拖动的快速点击
        if (Time.time - mouseDownTime < clickConfirmDelay)
        {
            if(FogOfWarManager.Instance().NodeVisible(gameObject, GameManager.Instance().player))
            {
                if (tower == null)
                {
                    BuildManager.Instance().ClickNode(gameObject, GameManager.Instance().player);

                }
            }
          
        }
    }

    public void BuildSetting()
    {
        playerForce[0]++;

        foreach (var item in MapManager.Instance().GetNearbyNodeItems(gameObject))
        {
            item.GetComponent<NodeItem>().GetComponent<NodeItem>().playerForce[0]++;
        }

        foreach (var item in MapManager.Instance().GetNearbyNodeItems(gameObject))
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

    public void ToggleFogOfWar(bool _show)
    {
        fog.SetActive(_show);

    }

    public void SetOrderInLayer(int _order)
    {
        foreach (var item in GetComponentsInChildren<SpriteRenderer>())
        {
            item.sortingOrder = _order;
        }

        fog.GetComponentInChildren<SpriteRenderer>().sortingOrder = _order + 1;
    }
}
