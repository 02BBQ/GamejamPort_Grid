using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Enemy3 : Enemy
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
        int size = (int)(GridManager.Instance.gridSize.x * GridManager.Instance.gridSize.y);
        int count = Random.Range(size / 6, size / 2);
        StartCoroutine(Rotate());
        for (int i = 0; i < count; i++)
        {
            int x = Random.Range(0, (int)GridManager.Instance.gridSize.x + 1);
            int y = Random.Range(0, (int)GridManager.Instance.gridSize.y + 1);
            PoolManager.Get(EnemySpawner.Instance.lineObj).GetComponent<DrawLine>().LineDraw(GridManager.Instance.grid[this.posX, this.posY] + GridManager.Instance.offset, GridManager.Instance.grid[x, y] + GridManager.Instance.offset, 10f, 10f);
            EventSquare ev = PoolManager.Get(InGameManager.Instance.eventSquare, GridManager.Instance.grid[x, y] + GridManager.Instance.offset, Quaternion.identity).GetComponent<EventSquare>();
            ev.posX = x;
            ev.posY = y;
            ev.Attack(1.5f, damage);
            SoundManager.Instance.PlaySFX(4, true);

            yield return new WaitForSeconds(1f/count);
        }
    }

    IEnumerator Rotate()
    {
        transform.rotation = Quaternion.identity;
        float angle = 0;
        while(angle < 180)
        {
            angle += Time.deltaTime * 360;
            transform.rotation = Quaternion.Euler(0, 0, angle);
            yield return null;
        }
        transform.rotation = Quaternion.identity;
        transform.DORotate(new Vector3(0, 0, 180), 2f).SetEase(Ease.OutBack);
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
