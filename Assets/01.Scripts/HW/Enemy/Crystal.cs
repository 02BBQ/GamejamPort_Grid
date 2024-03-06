using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : Enemy
{
    public GameObject crystalEffect;
    bool isStarted = false;

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    private void Update()
    {
        if (!isStarted)
        {
            transform.position += new Vector3(0, 0.5f, 0);
            isStarted = true;
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        isStarted = false;
    }
    public override void Attack()
    {
        throw new System.NotImplementedException();
    }

    public override void Hit(float amount)
    {
        EnemySpawner.Instance.CurrentPhaseIndex++;
        SoundManager.Instance.PlaySFX(Random.Range(7, 12));
        TimeController.Instance.SetTimeFreeze(0.6f, 0, 0.6f);
        CameraManager.Instance.CameraShake(5f, 0.7f);
        Dead();
    }

    public override void Dead()
    {
        StartCoroutine(IEDead());
    }

    new IEnumerator IEDead()
    {
        yield return new WaitUntil(() => !GridManager.Instance.isChanging);
        if (GridManager.Instance.isFirst)
        {
            GridManager.Instance.ChangeGrid(5, 5);
            GridManager.Instance.isFirst = false;
            EnemySpawner.Instance.wait = false;
        }
        else
        {
            int row = Random.Range(GridManager.Instance.row - 2, GridManager.Instance.row + 5);
            int column = Random.Range(GridManager.Instance.column - 2, GridManager.Instance.column + 5);
            GridManager.Instance.ChangeGrid(row, column);
        }
        EnemySpawner.Instance.enemyList.Remove(this);
        PoolManager.Get(crystalEffect, transform.position, Quaternion.identity);
        PoolManager.Release(gameObject);
    }
}
