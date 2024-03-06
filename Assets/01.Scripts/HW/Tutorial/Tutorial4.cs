using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial4 : Tutorial
{
    public override bool Excute()
    {
        return EnemySpawner.Instance.enemyList.Count <= 0;
    }

    public override void OnEnd()
    {
    }

    public override void OnStart()
    {
        _tutorialText.text = "적을 공격하여 처치해보세요.";
    }
}
