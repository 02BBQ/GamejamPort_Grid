using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy5 : Enemy
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
        for(int i = 0;i < 5;i++) 
        {
            int posX = PlayerMovement.Instance.posX;
            int posY = PlayerMovement.Instance.posY;
            Vector3 playerPos = GridManager.Instance.grid[posX, posY];
            foreach (Vector2 dir in Direction2D.nineDirectionList)
            {
                Vector3 pos = playerPos;
                pos += (Vector3)dir;
                if (pos.x < -5 || pos.y < -5 || pos.x + 5 > GridManager.Instance.gridSize.x || pos.y + 5 > GridManager.Instance.gridSize.y)
                    continue;
                PoolManager.Get(EnemySpawner.Instance.lineObj).GetComponent<DrawLine>().LineDraw(GridManager.Instance.grid[this.posX, this.posY] + GridManager.Instance.offset, pos + GridManager.Instance.offset, 10f, 10f);
                EventSquare ev = PoolManager.Get(InGameManager.Instance.eventSquare, pos + GridManager.Instance.offset, Quaternion.identity).GetComponent<EventSquare>();
                ev.Attack(1.5f, damage);
            }
            SoundManager.Instance.PlaySFX(4, true);
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator IEAttack()
    {
        yield return new WaitForSeconds(3f);

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
