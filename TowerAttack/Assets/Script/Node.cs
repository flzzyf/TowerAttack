using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour 
{
    Animator animator;

    [HideInInspector]
    public GameObject tower;

	void Start () 
	{
        animator = GetComponent<Animator>();
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
}
