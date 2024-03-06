using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAttack : MonoBehaviour
{
    public virtual void LineAttack(int x, int y, int damage, Color diceColor)
    {
        float size = GridManager.Instance.gridSize.x * GridManager.Instance.gridSize.y;
        for (int i = 0; i < size; i++)
        {
            if (x != -1)
            {
                EventSquare ev = PoolManager.Get(InGameManager.Instance.eventSquare, GridManager.Instance.grid[x, i] + GridManager.Instance.offset, Quaternion.identity).GetComponent<EventSquare>();
                ev.Attack(1.5f, damage);
            }
                
            if (y != -1)
            {
                EventSquare ev = PoolManager.Get(InGameManager.Instance.eventSquare, GridManager.Instance.grid[i, y] + GridManager.Instance.offset, Quaternion.identity).GetComponent<EventSquare>();
                ev.Attack(1.5f, damage);
            }

        }
    }
    public virtual void CrossAttack(Vector2Int pos, int damage)
    {
        LineAttack(pos.x, pos.y, damage, Color.red);
    }
    public virtual void SquareRangeAttack(Vector2Int pos, int range, int damage)
    {
        for (int i = -range; i <= range; i++)
        {
            for (int j = -range; j <= range; j++)
            {
                EventSquare ev = PoolManager.Get(InGameManager.Instance.eventSquare, (Vector3)(pos + Vector2Int.up * i + Vector2Int.right * j +(Vector2) GridManager.Instance.offset), Quaternion.identity).GetComponent<EventSquare>();
                ev.Attack(1.5f, damage);
            }
        }
    }
    public virtual void RangeWireAttack(Vector2Int pos, int range, int damage)
    {
        for (int i = -range; i <= range; i++)
        {
            for (int j = -range; j <= range; j++)
            {
                if (Mathf.Abs(i) == range || Mathf.Abs(j) == range)
                {
                    EventSquare ev = PoolManager.Get(InGameManager.Instance.eventSquare, (Vector3)(pos + Vector2Int.up * i + Vector2Int.right * j + (Vector2)GridManager.Instance.offset), Quaternion.identity).GetComponent<EventSquare>();
                    ev.Attack(1.5f, damage);
                }
            }
        }
    }
}
