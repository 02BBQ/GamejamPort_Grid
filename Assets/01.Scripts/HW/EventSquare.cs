using DG.Tweening;
using System.Collections;
using UnityEngine;

public class EventSquare : MonoBehaviour
{
    SpriteRenderer _spriteRenderer;
    public int posX, posY;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void Spawn(float cooltime, GameObject entity, int row, int column)
    {
        _spriteRenderer.color = Color.clear;
        StartCoroutine(IESpawn(cooltime, entity, row, column));
    }

    public void Attack(float cooltime, float damage)
    {
        _spriteRenderer.color = Color.clear;
        StartCoroutine(IEAttack(cooltime, damage));
    }

    public void Delete(float cooltime)
    {
        _spriteRenderer.color = Color.red;
        StartCoroutine(IEDelete(cooltime));
    }

    public void Create(float cooltime)
    {
        _spriteRenderer.color = Color.clear;
        StartCoroutine(IECreate(cooltime));
    }

    IEnumerator IESpawn(float cooltime, GameObject entity, int row, int column)
    {
        Vector3 pos = transform.position;
        _spriteRenderer.DOColor(Color.blue, cooltime);
        float t = 0;
        while (t < 3)
        {
            if (GridManager.Instance.isChanging)
            {
                PoolManager.Release(gameObject);
                yield break;
            }
            t += Time.deltaTime;
            yield return null;
        }
        Enemy e = PoolManager.Get(entity, pos, Quaternion.identity).GetComponent<Enemy>();
        e.posX = column;
        e.posY = row;
        PoolManager.Release(gameObject);
    }

    IEnumerator IEAttack(float cooltime, float damage)
    {
        _spriteRenderer.DOColor(Color.red, cooltime);
        yield return new WaitForSeconds(cooltime);
        _spriteRenderer.DOColor(Color.white, .05f).OnComplete(() => _spriteRenderer.DOColor(Color.clear, .4f).OnComplete(() => PoolManager.Release(gameObject)));
        if (transform.position == PlayerMovement.Instance.currentPos)
        {

            Player.Instance.Hit(damage);
        }
        else
        {
            CameraManager.Instance.CameraShake(1.3f, 0.4f);
        }
    }

    IEnumerator IEDelete(float cooltime)
    {
        _spriteRenderer.DOColor(Color.white, cooltime);
        yield return new WaitForSeconds(cooltime);
        _spriteRenderer.DOColor(Color.clear, .4f).OnComplete(() => PoolManager.Release(gameObject));
    }

    IEnumerator IECreate(float cooltime)
    {
        _spriteRenderer.DOColor(Color.cyan, cooltime);
        yield return null;
        _spriteRenderer.sortingOrder = -5;
        yield return new WaitForSeconds(cooltime);
        PoolManager.Release(gameObject);
    }
}
