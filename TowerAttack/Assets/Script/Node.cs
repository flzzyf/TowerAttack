using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour 
{
    Animator animator;

    [HideInInspector]
    public GameObject tower;

    Color originColor;

	void Start () 
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
        if(tower == null)
        {
            BuildManager.Instance().Build(gameObject, GameManager.Instance().player);
        }
    }

    public void ChangeColor(Color _color = default(Color))
    {
        if(_color == default(Color))
        {
            GetComponentInChildren<SpriteRenderer>().color = originColor;
        }
        else
        {
            GetComponentInChildren<SpriteRenderer>().color = _color;

        }
    }
}
