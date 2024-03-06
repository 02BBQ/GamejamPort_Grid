using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleSceneUIManager : MonoBehaviour
{
    [SerializeField]
    Image _blackPanel;
    Sequence _titleSeq;
    [SerializeField]
    CinemachineVirtualCamera _cam;

    private void Awake()
    {
        Screen.SetResolution(1920, 1080, true);
    }

    private void Start()
    {
        SoundManager.Instance.BGMVolumeDown(1, false, 0);
        _cam.m_Lens.OrthographicSize = 1000;
        _titleSeq = DOTween.Sequence();
        _titleSeq.Append(DOTween.To(() => _cam.m_Lens.OrthographicSize, v => _cam.m_Lens.OrthographicSize = v, 10.43246f, 1.5f).SetEase(Ease.OutCubic));
        _titleSeq.AppendCallback(() => SoundManager.Instance.BGMChange(1));
    }

    private void Update()
    {
        if(Input.anyKey && _titleSeq.IsActive() && _titleSeq != null) 
        {
            _titleSeq.Complete();
            SoundManager.Instance.BGMChange(1);
        }
    }

    public void MoveScene(string name)
    {
        _blackPanel.gameObject.SetActive(true);
        SoundManager.Instance.PlaySFX(0);
        DOTween.To(() => _cam.m_Lens.OrthographicSize, v => _cam.m_Lens.OrthographicSize = v, 0, 2f).SetEase(Ease.InCubic).OnComplete(() => SceneManager.LoadScene(name)); ;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
