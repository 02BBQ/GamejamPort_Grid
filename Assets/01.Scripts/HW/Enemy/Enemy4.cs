using System.Collections;
using UnityEngine;

public class Enemy4 : Enemy
{
    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(IEAttack());
    }

    public override void Attack()
    {
        for (int i = 0; i < 4; i++)
        {
            Vector2 pos = transform.position - GridManager.Instance.offset;
            Vector2 dir = Direction2D.fourDirectionList[i];
            for (int j = 0; j < 10; j++)
            {
                pos += dir;
                if (pos.x < -5 || pos.y < -5 || pos.x + 5 > GridManager.Instance.gridSize.x || pos.y + 5 > GridManager.Instance.gridSize.y)
                    continue;
                EventSquare ev = PoolManager.Get(InGameManager.Instance.eventSquare, pos + (Vector2)GridManager.Instance.offset, Quaternion.identity).GetComponent<EventSquare>();
                ev.Attack(1.5f, damage);
            }
        }
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
