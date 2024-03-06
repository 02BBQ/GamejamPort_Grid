using System.Collections;
using UnityEngine;

public class Enemy1 : Enemy
{

    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(IEAttack());
    }

    public override void Attack()
    {
        int posX = PlayerMovement.Instance.posX;
        int posY = PlayerMovement.Instance.posY;
        Vector3 playerPos = GridManager.Instance.grid[posX, posY] + GridManager.Instance.offset;
        transform.rotation = Quaternion.Euler(0,0,Mathf.Atan2(playerPos.y,playerPos.x) * Mathf.Rad2Deg - 90);
        PoolManager.Get(EnemySpawner.Instance.lineObj).GetComponent<DrawLine>().LineDraw(GridManager.Instance.grid[this.posX, this.posY] + GridManager.Instance.offset, playerPos, 10f, 10f);
        EventSquare ev = PoolManager.Get(InGameManager.Instance.eventSquare, playerPos, Quaternion.identity).GetComponent<EventSquare>();
        ev.posX = posX;
        ev.posY = posY;
        SoundManager.Instance.PlaySFX(4, true);
        ev.Attack(1.5f, damage);
    }

    IEnumerator IEAttack()
    {
        yield return null;

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
