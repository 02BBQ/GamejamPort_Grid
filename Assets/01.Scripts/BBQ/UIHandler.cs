using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using Unity.VisualScripting;
using TMPro;
using UnityEngine.SceneManagement;

public class UIHandler : MonoSingleton<UIHandler>
{
    public Image ExpBar;
    public GameObject Selection;

    public bool isSelected = false;
    public bool LevelingFinished = false;

    [SerializeField] private Transform healthMother;
    [SerializeField] private Transform healthFather;

    [SerializeField] private GameObject EmptyHeart;
    [SerializeField] private GameObject Heart;

    [SerializeField] private GameObject PauseUI;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUI.SetActive(!PauseUI.activeSelf);
            if(PauseUI.activeSelf)
                Time.timeScale = 0f;
            else
            {
                Time.timeScale = 1f;
                SoundSetting.Instance.Close();
            }
        }
    }

    public void Resume()
    {
        PauseUI.SetActive(false);
        Time.timeScale = 1.0f;
    }
    public void Retry()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("InGame");
    }
    public void Lobby()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Title");
    }
    public void Setting()
    {
        SoundSetting.Instance.Open();
    }

    public void OnExpChanged()
    {
        ExpBar.DOFillAmount(Mathf.Clamp((float)Player.Instance.Exp / Player.Instance.MaxExp, 0, 1), 0.5f).SetEase(Ease.OutQuad);//.SetUpdate(true);
    }
    public IEnumerator CaculateEXP()
    {
        OnExpChanged();
        if(Player.Instance.Exp >= Player.Instance.MaxExp && !PlayerDead.Instance.isDead && !LevelingFinished)
        {
            LevelingFinished = true;
            Time.timeScale = 0;
            ExpBar.DOFillAmount(Mathf.Clamp((float)Player.Instance.Exp / Player.Instance.MaxExp, 0, 1), 0.5f).SetEase(Ease.OutQuad).SetUpdate(true);
            while (Player.Instance.Exp >= Player.Instance.MaxExp)
            {
                Player.Instance.Level++;
                if (Player.Instance.Level == 5 || Player.Instance.Level == 10)
                {
                    Selection.GetComponent<SelectionHandler>().isWeaponSelect = true;
                    Selection.SetActive(true);
                    while (!isSelected)
                    {
                        yield return null;
                    }
                    Selection.SetActive(false);
                }
                Selection.GetComponent<SelectionHandler>().isWeaponSelect = false;
                Selection.SetActive(true);
                while (!isSelected)
                {
                    yield return null;
                }
                Selection.SetActive(false);
                isSelected = false;
                Player.Instance.exp -= Player.Instance.MaxExp;
                Player.Instance.MaxExp += Player.Instance.Level >= 20 ? 15 : Player.Instance.Level >= 10 ? 10 : 5;
            }
            Time.timeScale = 1;
            LevelingFinished = false;
        }
        
        
        OnExpChanged();
    }
    public bool Select(BuffSO buff, float amount)
    {
        isSelected = true;
        Selection.SetActive(false);
        if (buff.AttackSpeedUp)
        {
            if (Player.Instance.AttackSpeed >= 10)
            {
                Player.Instance.Damage += amount;
            }
            else
            {
                Player.Instance.AttackSpeed += amount;
            }
        }
        
        if (buff.DamageUp) Player.Instance.Damage += amount;
        if (buff.HealthUp) Player.Instance.Health += amount;
        if (buff.MaxHealthUp)
        {
            if (Player.Instance.MaxHealth < 10)
            {
                amount = 1;
                Player.Instance.MaxHealth += amount;
            }
            else
            {
                Player.Instance.Health++;
            }
            //return false;
        };
        return true;
    }

    public void HealthBarUpdate()
    {
        float Health = Player.Instance.Health;
        float MaxHealth = Player.Instance.MaxHealth;
        int currentHearts = (int)Health; // 현재 체력에 맞는 하트의 수
        foreach (Transform heart in healthMother)
            PoolManager.Release(heart);
        foreach (Transform heart in healthFather)
            PoolManager.Release(heart);
        for (int i = 0; i < (int)MaxHealth; i++)
        {
            PoolManager.Get(EmptyHeart, healthMother);
        }
        for (int i = 0; i < currentHearts; i++)
        {
            PoolManager.Get(Heart, healthFather);
        }
    }

    public void Select(WeaponSO weaponSO)
    {
        isSelected = true;
        Selection.SetActive(false);
        if (Player.Instance.Weapon != null)
        {
            Player.Instance.HoldingWeapon = weaponSO;
            transform.Find("HoldingOne").gameObject.SetActive(true);
            transform.Find("HoldingOne").GetComponent<TMP_Text>().text = $"교대 가능 : {Player.Instance.HoldingWeapon.Name}";
        }
        else
        {
            Player.Instance.Weapon = weaponSO;
        }
    }
}
