using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 1;

    public float damage = 1;
    public GameObject target;

    public GameObject gfx;

    public void Launch(GameObject _target, float _damage)
    {
        target = _target;
        damage = _damage;

        StartCoroutine(LaunchMissile());
    }

    void Update()
    {
        if (target == null)
            return;

        if (Mathf.Abs(target.transform.position.x - transform.position.x) > zyf.GetWorldScreenSize().y ||
            Mathf.Abs(target.transform.position.y - transform.position.y) > zyf.GetWorldScreenSize().x)
        {
            gfx.SetActive(false);
        }
    }

    IEnumerator LaunchMissile()
    {
        Vector2 targetPoint = (Vector2)target.transform.position + target.GetComponent<Tower>().GetImpactPoint();

        FaceTarget2D(targetPoint);

        while (target != null &&
                Vector2.Distance(targetPoint, transform.position) > speed * Time.deltaTime)
        {
            Vector2 dir = targetPoint - (Vector2)transform.position;
            dir.Normalize();
            transform.Translate(dir * speed * Time.deltaTime, Space.World);

            yield return null;
        }

        SoundManager.Instance().Play("Arrow_Impact");

        if (target != null)
            Hit();

        Destroy(gameObject);
    }

    void Hit()
    {
        target.GetComponent<Tower>().TakeDamage(damage);
    }

    void FaceTarget2D(Vector2 _target)
    {
        Vector3 direction = _target - (Vector2)transform.position;
        direction.z = 0f;
        direction.Normalize();
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        targetAngle -= 90;
        transform.rotation = Quaternion.Euler(0, 0, targetAngle);
    }



}
