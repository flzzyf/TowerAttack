﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : IPooledObject
{
    public float speed = 1;

    public float damage = 1;

    public GameObject gfx;
    GameObject target;

    [HideInInspector]
    public int player;

    public override void OnObjectSpawned()
    {
        base.OnObjectSpawned();

        gfx.SetActive(true);
    }

    public void Launch(GameObject _target, float _damage)
    {
        target = _target;
        damage = _damage;

        Vector2 impactPos = target.GetComponent<Tower>().GetImpactPoint();
        if (Vector2.Distance(transform.position, impactPos) > 4)
            gfx.SetActive(false);

        StartCoroutine(LaunchMissile(impactPos));
    }

    IEnumerator LaunchMissile(Vector2 _pos)
    {
        FaceTarget2D(_pos);
        Vector2 targetDir = _pos - (Vector2)transform.position;
        float time = targetDir.magnitude / speed;
        targetDir.Normalize();

        while (target != null && time > 0)
        {
            time -= Time.deltaTime;
            
            transform.Translate(targetDir * speed * Time.deltaTime, Space.World);

            yield return null;
        }

        SoundManager.Instance().Play("Arrow_Impact");

        if (target != null)
            Hit();

        gameObject.SetActive(false);
    }

    void Hit()
    {
        target.GetComponent<Tower>().TakeDamage(damage, player);
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
