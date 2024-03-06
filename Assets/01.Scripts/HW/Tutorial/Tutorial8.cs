using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial8 : Tutorial
{
    public override bool Excute()
    {
        return !GridManager.Instance.isChanging;
    }

    public override void OnEnd()
    {
    }

    public override void OnStart()
    {
        _tutorialText.text = "줄어드는 맵 위에 있으면 사망합니다.";
    }
}
