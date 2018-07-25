using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour 
{
    public float speed = 1;

    public float damage = 1;
    public GameObject target;

    public void Launch(GameObject _target, float _damage)
    {
        target = _target;
        damage = _damage;

        StartCoroutine(LaunchMissile());
    }

    IEnumerator LaunchMissile()
    {

        while (target != null &&
                Vector2.Distance(target.transform.position, transform.position) > speed * Time.deltaTime)
        {
            Vector2 dir = target.transform.position - transform.position;
            dir.Normalize();
            transform.Translate(dir * speed * Time.deltaTime);

            yield return null;
        }

        if(target != null)
            target.GetComponent<Tower>().TakeDamage(damage);

        Destroy(gameObject);
    }
}
