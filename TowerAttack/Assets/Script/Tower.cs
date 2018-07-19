using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour 
{
    public float range = 3;
    public float hp = 3;
    public float damage = 1;

    public float rotationSpeed = 3;

    [HideInInspector]
    public int player;

    [HideInInspector]
    public GameObject target;

    [HideInInspector]
    public GameObject node;

	void Start () 
	{
        
	}

	void Update () 
	{
        if(target != null)
        {
            FaceTarget2D(target.transform.position);

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
                    target = item.gameObject;

                    Attack(item.gameObject);
                }
        }
    }

    void Attack(GameObject _target)
    {
        FaceTarget2D(_target.transform.position);

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
