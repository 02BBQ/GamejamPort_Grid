using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy0 : Enemy
{
    protected override void OnEnable()
    {
        base.OnEnable();
    }

    public override void Hit(float amount)
    {
        hp -= amount;
        StopCoroutine("IEHit");
        StartCoroutine("IEHit");
        CameraManager.Instance.CameraShake(1.5f, 0.5f);
    }

    public override void Dead()
    {
        _effect = PoolManager.Get(EnemySpawner.Instance.enemyDeadEffect, transform.position, Quaternion.identity);
        EnemySpawner.Instance.availableGridList.Add(new int[] { posX, posY });

        StartCoroutine(IEDead());
    }

    public override void Attack()
    {
        throw new System.NotImplementedException();
    }
}
