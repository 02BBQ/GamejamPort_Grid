using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial5 : Tutorial
{
    float _time = 0;
    public override bool Excute()
    {
        _time += Time.deltaTime;
        return _time >= 3f;
    }

    public override void OnEnd()
    {
    }

    public override void OnStart()
    {
        _tutorialText.text = "맵의 크기는 크리스탈을 부수었을때 바뀝니다.";
    }
}
