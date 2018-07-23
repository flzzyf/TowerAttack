using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour 
{
    public float range = 3;
    public int hp = 3;
    int currentHp;
    public int damage = 1;

    public float rotationSpeed = 3;

    public GameObject prefab_bullet;

    [HideInInspector]
    public int player;

    [HideInInspector]
    public GameObject target;

    [HideInInspector]
    public GameObject node;

	void Start () 
	{
        currentHp = hp;
	}
	void Update () 
	{
        if(target != null)
        {
            //FaceTarget2D(target.transform.position);

        }
    }

    public void SearchTarget()
    {
        foreach (Transform item in ParentManager.Instance().GetParent("Tower"))
        {
            //不同玩家
            if(item.gameObject.GetComponent<Tower>().player != player)
                //距离可攻击
                if (Vector2.Distance(transform.position, item.position) < range)
                {

                    LockTarget(item.gameObject);
                }
        }
    }

    void LockTarget(GameObject _target)
    {
        target = _target;

        //FaceTarget2D(_target.transform.position);
    }

    public void Attack()
    {
        //GameObject go = Instantiate(prefab_bullet, transform.position, Quaternion.identity);

        target.GetComponent<Tower>().TakeDamage(damage);
    }

    public void TakeDamage(int _amount)
    {
        currentHp -= _amount;
        if(currentHp <= 0)
        {
            Death();
        }
    }

    public void Death()
    {
        node.GetComponent<Node_Ground>().tower = null;
        node.GetComponent<Node_Ground>().ChangeColor();

        Destroy(gameObject);
    }

    void FaceTarget2D(Vector2 _target)
    {
        Vector3 direction = _target - (Vector2)transform.position;
        direction.z = 0f;
        direction.Normalize();
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        targetAngle -= 90;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, targetAngle), rotationSpeed * Time.deltaTime);

        //StartCoroutine(RotateToAngle(targetAngle));
    }

    IEnumerator RotateToAngle(float _angle)
    {
        while(Mathf.Abs(transform.rotation.eulerAngles.z - _angle) > 1)
        {
            //Debug.Log(transform.rotation.eulerAngles.z);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, _angle), rotationSpeed * Time.deltaTime);

            yield return null;
        }
    }

}
