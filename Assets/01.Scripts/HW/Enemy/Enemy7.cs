using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy7 : Enemy
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
        yield return new WaitForSeconds(3f);
        while(true)
        {
            int posX = PlayerMovement.Instance.posX;
            int posY = PlayerMovement.Instance.posY;
            Vector3 playerPos = GridManager.Instance.grid[posX, posY];
            PoolManager.Get(EnemySpawner.Instance.lineObj).GetComponent<DrawLine>().LineDraw(GridManager.Instance.grid[this.posX, this.posY] + GridManager.Instance.offset, playerPos + GridManager.Instance.offset, 10f, 10f);
            EventSquare ev = PoolManager.Get(InGameManager.Instance.eventSquare, playerPos + GridManager.Instance.offset, Quaternion.identity).GetComponent<EventSquare>();
            ev.Attack(1.5f, damage);
            SoundManager.Instance.PlaySFX(4, true);
            yield return new WaitForSeconds(0.4f);
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
