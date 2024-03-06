using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using Unity.VisualScripting;

public class CameraManager : MonoSingleton<CameraManager>
{
    Camera _mainCam;
    CinemachineVirtualCamera _gameCam;
    CinemachineBasicMultiChannelPerlin _gPerlin;
    Tween _shakeTween;
    private void Awake()
    {
        _mainCam = Camera.main;
        _gameCam = transform.Find("GameCamera").GetComponent<CinemachineVirtualCamera>();
        _gPerlin = _gameCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Start()
    {
        _gameCam.m_Lens.OrthographicSize = 300f;
        DOTween.To(() => _gameCam.m_Lens.OrthographicSize, v => _gameCam.m_Lens.OrthographicSize = v, 6f, 1.5f).SetEase(Ease.OutCubic);
    }

    public void CameraShake(float amount, float time)
    {
        if (_gPerlin.m_AmplitudeGain > amount)
        {
            return;
        }
        _gPerlin.m_AmplitudeGain = amount;
        if (_shakeTween != null && _shakeTween.IsActive())
        {
            _shakeTween.Kill();
        }
        _shakeTween = DOTween.To(() => _gPerlin.m_AmplitudeGain, v => _gPerlin.m_AmplitudeGain = v, 0, time);
    }

    public void MoveCamera(Transform trm)
    {
        _gameCam.Follow = trm;
    }

    public void ZoomCamera(float amount, float duration, Ease ease = Ease.OutSine)
    {
        DOTween.To(() => _gameCam.m_Lens.OrthographicSize, v => _gameCam.m_Lens.OrthographicSize = v, amount, duration).SetEase(ease);
    }
}
