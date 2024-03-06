using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial3 : Tutorial
{
    float _time = 0;
    public override bool Excute()
    {
        _time += Time.deltaTime;
        return _time >= 4f;
    }

    public override void OnEnd()
    {
    }

    public override void OnStart()
    {
        _tutorialText.text = "적이 공격하는 칸은 빨간색으로 변하며 일정시간 내에 피하지 못하면 데미지를 입습니다.";
    }
}
