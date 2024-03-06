using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy6 : Enemy
{

    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(IEAttack());
    }

    public override void Attack()
    {
        StartCoroutine(AttackCor());
    }

    IEnumerator AttackCor()
    {
        transform.DOMoveY(transform.position.y + 0.6f, 1f);
        yield return new WaitForSeconds(1f);
        transform.DOMoveY(transform.position.y - 0.6f, 0.1f).OnComplete(() =>
        {
            SoundManager.Instance.PlaySFX(12);
            CameraManager.Instance.CameraShake(3f, 0.6f);
            foreach (Vector2 dir in Direction2D.eightDirectionList)
            {
                Vector2 pos = transform.position - GridManager.Instance.offset;
                pos += dir;
                if (pos.x < -5 || pos.y < -5 || pos.x + 5 > GridManager.Instance.gridSize.x || pos.y + 5 > GridManager.Instance.gridSize.y)
                    continue;
                EventSquare ev = PoolManager.Get(InGameManager.Instance.eventSquare, pos + (Vector2)GridManager.Instance.offset, Quaternion.identity).GetComponent<EventSquare>();
                ev.Attack(0.3f, damage);
            }
        });
    }

    IEnumerator IEAttack()
    {
        yield return new WaitForSeconds(2f);

        while (true)
        {
            Attack();
            yield return new WaitForSeconds(attackDelay);
        }
    }

    public override void Hit(float amount)
    {
        hp -= amount;
        StopCoroutine("IEHit");
        StartCoroutine("IEHit");
        CameraManager.Instance.CameraShake(1.5f, 0.5f);
        if (hp <= 0)
            Dead();
    }

    public override void Dead()
    {
        _effect = PoolManager.Get(EnemySpawner.Instance.enemyDeadEffect, transform.position, Quaternion.identity);
        EnemySpawner.Instance.availableGridList.Add(new int[] { posX, posY });

        StartCoroutine(IEDead());
    }
}
