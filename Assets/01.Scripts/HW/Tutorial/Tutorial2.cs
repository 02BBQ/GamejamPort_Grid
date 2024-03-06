using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial2 : Tutorial
{
    float _time = 0;
    public override bool Excute()
    {
        _time += Time.deltaTime;
        return _time >= 2f;
    }

    public override void OnEnd()
    {
    }

    public override void OnStart()
    {
        _tutorialText.text = "랜덤한 칸에 적이 소환되며 파란색으로 경고표시 됩니다.";
        EnemySpawner.Instance.EnemySpawn(0);
    }
}
