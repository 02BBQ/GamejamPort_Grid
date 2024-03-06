using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : MonoSingleton<InGameManager>
{
    public GameObject eventSquare;
    public float currentTime;
    public int CaughtEnemy;

    private void Update()
    {
        if (EnemySpawner.Instance.wait) return;
        currentTime += Time.deltaTime;
    }
}
