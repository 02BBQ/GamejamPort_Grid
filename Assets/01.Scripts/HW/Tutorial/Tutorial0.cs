using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial0 : Tutorial
{
    public override bool Excute()
    {
        return Input.GetKeyDown(KeyCode.W) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyUp(KeyCode.D);
    }

    public override void OnEnd()
    {
    }

    public override void OnStart()
    {
        _tutorialText.text = "WASD를 눌러 플레이어를 움직일 수 있습니다.";
    }
}
