using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoSingleton<EnemySpawner>
{
    public GameObject[] enemyPrefabs;
    public GameObject lineObj;
    public GameObject enemyDeadEffect;
    public Material whiteMat;
    public Material defaultMat;
    public List<Enemy> enemyList = new();
    public List<int[]> availableGridList = new();
    public PlayerMovement playerMovement;
    public List<PhaseSO> phases = new();
    private int _currentPhaseIndex;
    public int CurrentPhaseIndex
    {
        get => _currentPhaseIndex;
        set
        {
            _currentPhaseIndex = Mathf.Clamp(value, 0, phases.Count - 1);
        }
    }
    public bool wait = true;
    private float maxTime = 10;
    public int phaseCount = 0;
    public float MaxTime
    {
        get => maxTime;
        set
        {
            maxTime = Mathf.Clamp(value, 3, 10);
        }
    }
    private void Awake()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                availableGridList.Add(new int[] { i, j });
            }
        }
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "Tutorial") return;
        StartCoroutine(IESpawn());
    }

    public void EnemySpawn(int id)
    {
        if (GridManager.Instance.isChanging) return;
        int size = ((GridManager.Instance.row + 1) * (GridManager.Instance.column + 1));
        if (size - availableGridList.Count > size / 2) return;
        int[] g = availableGridList[Random.Range(0, availableGridList.Count)];
        availableGridList.Remove(g);
        int row = g[1];
        int column = g[0];
        Vector3 spawnPos = GridManager.Instance.grid[column, row];
        EventSquare ev = PoolManager.Get(InGameManager.Instance.eventSquare, spawnPos + GridManager.Instance.offset, Quaternion.identity).GetComponent<EventSquare>();
        ev.Spawn(3f, enemyPrefabs[id], row, column);
    }

    public Enemy GetEnemy(Vector3 pos)
    {
        foreach (Enemy e in enemyList)
        {
            if (e.transform.position == pos)
            {
                return e;
            }
        }
        return null;
    }

    public void StopSpawn()
    {
        StopAllCoroutines();
    }

    IEnumerator IESpawn()
    {
        if (SceneManager.GetActiveScene().name != "Tutorial")
        {
            EnemySpawn(6);
            yield return new WaitUntil(() => !wait);
        }
        while (true)
        {
            int length = phases[CurrentPhaseIndex].phase.Length;
            phaseCount++;
            if (phaseCount >= 10)
            {
                phaseCount = 0;
                EnemySpawn(6);
            }
            float time = 0;
            int rand = Random.Range(0, length);
            for (int i = 0; i < length - 1; i++)
            {
                for (int j = 0; j < phases[CurrentPhaseIndex].phase[rand].enemies[i]; j++)
                {
                    EnemySpawn(i);
                    yield return new WaitForSeconds(phases[CurrentPhaseIndex].phase[rand].spawnDelay);
                }
            }
            while (time < MaxTime)
            {
                time += Time.deltaTime;
                if (enemyList.Count <= 2)
                {
                    break;
                }
                yield return null;
            }
        }
    }
}

[System.Serializable]
public class Phase
{
    public int[] enemies = new int[6];
    public float spawnDelay;
}
