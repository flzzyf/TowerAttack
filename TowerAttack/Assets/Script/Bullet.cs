using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 1;

    public float damage = 1;

    public GameObject gfx;
    GameObject target;

    public void Launch(GameObject _target, float _damage)
    {
        target = _target;
        damage = _damage;

        Vector2 impactPos = (Vector2)_target.transform.position + _target.GetComponent<Tower>().GetImpactPoint();
        StartCoroutine(LaunchMissile(impactPos));
    }

    IEnumerator LaunchMissile(Vector2 _pos)
    {
        Vector2 targetDir = _pos - (Vector2)transform.position;
        float time = targetDir.magnitude / speed;
        targetDir.Normalize();
        print(time);
        Vector2 movement = targetDir;

        FaceTarget2D(targetDir);

        while (target != null && time > 0)
        {
            time -= Time.deltaTime;
            
            transform.Translate(movement * speed * Time.deltaTime, Space.World);

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
