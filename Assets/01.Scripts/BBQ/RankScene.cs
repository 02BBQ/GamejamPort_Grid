using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RankScene : MonoBehaviour
{
    public float aliveTime;
    public float totalDamage;
    public float ratio;

    public Image image;

    public int timeRank, dmgRank, ratioRank;

    public TMP_Text text;
    public TMP_Text totalT;
    public TMP_Text time;
    public TMP_Text ratioT;

    [SerializeField]
    CinemachineVirtualCamera _cam;
    private void Start()
    {
        image.DOFade(0, 1.5f);
        aliveTime = PlayerPrefs.GetFloat("AliveTime");
        totalDamage = PlayerPrefs.GetFloat("TotalDamage");
        ratio = aliveTime == 0 || totalDamage == 0 ? 0 : totalDamage / aliveTime;
        if (aliveTime >= 350)
            timeRank = 1;
        else if (aliveTime >= 240)
            timeRank = 2;
        else if (aliveTime >= 180)
            timeRank = 3;
        else if (aliveTime >= 120)
            timeRank = 4;
        else if (aliveTime >= 60)
            timeRank = 5;
        else
            timeRank = 6;

        if (totalDamage >= 1600)
            dmgRank = 1;
        else if (totalDamage >= 900)
            dmgRank = 2;
        else if (totalDamage >= 550)
            dmgRank = 3;
        else if (totalDamage >= 300)
            dmgRank = 4;
        else if (totalDamage >= 100)
            dmgRank = 5;
        else
            dmgRank = 6;

        if (ratio >= 7f)
            ratioRank = 1;
        else if (ratio >= 5f)
            ratioRank = 2;
        else if (ratio >= 3.5f)
            ratioRank = 3;
        else if (ratio >= 2f)
            ratioRank = 4;
        else if (ratio >= 1.5f)
            ratioRank = 5;
        else
            ratioRank = 6;

        int total = (int)((ratioRank + timeRank + dmgRank) / (float)3);
        print($"{total}  {ratioRank}  {timeRank}  {dmgRank}");
        switch (dmgRank)
        {
            case 1:
                totalT.text = "S";
                break;
            case 2:
                totalT.text = "A";
                break;
            case 3:
                totalT.text = "B";
                break;
            case 4:
                totalT.text = "C";
                break;
            case 5:
                totalT.text = "D";
                break;
            case 6:
                totalT.text = "E";
                break;
            default:
                text.text = "ERROR";
                break;
        }

        switch (timeRank)
        {
            case 1:
                time.text = "S";
                break;
            case 2:
                time.text = "A";
                break;
            case 3:
                time.text = "B";
                break;
            case 4:
                time.text = "C";
                break;
            case 5:
                time.text = "D";
                break;
            case 6:
                time.text = "E";
                break;
            default:
                time.text = "ERROR";
                break;
        }

        switch (ratioRank)
        {
            case 1:
                ratioT.text = "S";
                break;
            case 2:
                ratioT.text = "A";
                break;
            case 3:
                ratioT.text = "B";
                break;
            case 4:
                ratioT.text = "C";
                break;
            case 5:
                ratioT.text = "D";
                break;
            case 6:
                ratioT.text = "E";
                break;
            default:
                ratioT.text = "ERROR";
                break;
        }

        switch (total)
        {
            case 1:
                text.text = "S";
                break;
            case 2:
                text.text = "A";
                break;
            case 3:
                text.text = "B";
                break;
            case 4:
                text.text = "C";
                break;
            case 5:
                text.text = "D";
                break;
            case 6:
                text.text = "E";
                break;
            default:
                text.text = "ERROR";
                break;
        }
    }

    public void Lobby()
    {
        DOTween.To(() => _cam.m_Lens.OrthographicSize, v => _cam.m_Lens.OrthographicSize = v, 1000, 1.5f).SetEase(Ease.InCubic).OnComplete(() => SceneManager.LoadScene("Title"));
    }
}
