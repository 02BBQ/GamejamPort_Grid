using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI.Table;

public abstract class Enemy : MonoBehaviour
{
    SpriteRenderer _spriteRenderer;
    public EnemySO enemySO;
    public float hp;
    public float attackDelay;
    public float damage;
    public int posX;
    public int posY;
    public int exp;
    protected GameObject _effect;
    protected virtual void OnEnable()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        EnemySpawner.Instance.enemyList.Add(this);
        hp = enemySO.hp + Mathf.FloorToInt(InGameManager.Instance.currentTime / 60);
        attackDelay = enemySO.attackDelay;
        damage = enemySO.damage;
        exp = enemySO.exp;
        _spriteRenderer.material = EnemySpawner.Instance.defaultMat;
    }

    protected virtual void OnDisable()
    {
        StopAllCoroutines();
    }

    public abstract void Attack();

    public abstract void Hit(float amount);

    protected virtual IEnumerator IEHit()
    {
        SoundManager.Instance.PlaySFX(14, true);
        _spriteRenderer.material = EnemySpawner.Instance.whiteMat;
        yield return new WaitForSeconds(0.3f);
        _spriteRenderer.material = EnemySpawner.Instance.defaultMat;
    }
    public abstract void Dead();

    protected virtual IEnumerator IEDead()
    {
        //yield return new WaitUntil(() => !GridManager.Instance.isChanging);
        yield return null;
        InGameManager.Instance.CaughtEnemy++;
        Player.Instance.Exp += exp;
        EnemySpawner.Instance.enemyList.Remove(this);
        PoolManager.Release(gameObject);
    }
}
