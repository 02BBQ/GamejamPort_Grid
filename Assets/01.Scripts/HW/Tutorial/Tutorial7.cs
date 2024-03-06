using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial7 : Tutorial
{
    public override bool Excute()
    {
        return Time.timeScale > 0;
    }

    public override void OnEnd()
    {
    }

    public override void OnStart()
    {
        _tutorialText.text = "적을 처치해 경험치를 얻어 레벨업 할 수 있습니다.";
    }
}
