using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class Tutorial9 : Tutorial
{
    float _time = 0;
    public override bool Excute()
    {
        _time += Time.deltaTime;
        return _time > 3f;
    }

    public override void OnEnd()
    {
        CinemachineVirtualCamera cam = TutorialManager.Instance.cam;
        DOTween.To(() => cam.m_Lens.OrthographicSize, v => cam.m_Lens.OrthographicSize = v, 0, 1.5f).OnComplete(() => SceneManager.LoadScene("Title"));
    }

    public override void OnStart()
    {
        _tutorialText.text = "튜토리얼을 완료하셨습니다.";
    }
}
