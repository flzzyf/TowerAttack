using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour 
{
    public int range = 2;
    public float hp = 3;
    float currentHp;
    public int damage = 1;

    public GameObject prefab_bullet;

    public GameObject flag;

    public float searchTargetCD = 0.5f;

    public float attackCD = 0.5f;
    float currentAttackCD;

    public Transform launchPos;

    [HideInInspector]
    public int player;

    [HideInInspector]
    public GameObject target;

    [HideInInspector]
    public GameObject node;

    public Vector2 impactAreaCenter;
    public Vector2 impactAreaSize = new Vector2(1 ,1);

    void Start () 
	{
        currentHp = hp;

        InvokeRepeating("SearchTarget", 0, searchTargetCD);
    }
    void Update () 
	{
        if (currentAttackCD > 0)
            currentAttackCD -= Time.deltaTime;
        else
        {
            if (target != null)
            {
                currentAttackCD = attackCD;
                Attack(target);
            }

        }

    }

    public void Init()
    {
        flag.GetComponent<SpriteRenderer>().color = TeamManager.Instance().players[player].color;

    }

    public void SearchTarget()
    {
        if (target != null)
            return;

        foreach (var item in MapManager.Instance().GetNearbyNodeItems(node.GetComponent<NodeItem>().pos))
        {
            if (item.GetComponent<NodeItem>().tower != null)
                if (item.GetComponent<NodeItem>().tower.GetComponent<Tower>().player != player)
                    target = item.GetComponent<NodeItem>().tower;

        }

    }

    public void Attack(GameObject _target)
    {
        GameObject go = Instantiate(prefab_bullet, launchPos.position, Quaternion.identity);

        float fixedDamage = damage * attackCD;

        go.GetComponent<Bullet>().Launch(_target, fixedDamage);
    }


    public void TakeDamage(float _amount)
    {
        currentHp -= _amount;
        if(currentHp <= 0)
        {
            Death();
        }
    }

    public void Death()
    {
        node.GetComponent<NodeItem>().tower = null;
        node.GetComponent<NodeItem>().ChangeColor();

        Destroy(gameObject);
    }

    public void SetOrderInLayer(int _order)
    {
        foreach (var item in GetComponentsInChildren<SpriteRenderer>())
        {
            item.sortingOrder = _order;
        }
    }

    //获取轰击点
    public Vector2 GetImpactPoint()
    {
        Vector2 point;
        point.x = Random.Range(impactAreaCenter.x - impactAreaSize.x / 2, impactAreaCenter.x + impactAreaSize.x / 2);
        point.y = Random.Range(impactAreaCenter.y - impactAreaSize.y / 2, impactAreaCenter.y + impactAreaSize.y / 2);

        return point;
    }

    //绘制线框
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(impactAreaCenter, impactAreaSize);
    }

}
