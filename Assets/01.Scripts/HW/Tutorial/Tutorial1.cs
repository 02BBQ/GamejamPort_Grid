using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial1 : Tutorial
{
    public override bool Excute()
    {
        return Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow);
    }

    public override void OnEnd()
    {
    }

    public override void OnStart()
    {
        _tutorialText.text = "방향키를 눌러 플레이어의 공격방향을 조종할 수 있습니다.";
    }
}
