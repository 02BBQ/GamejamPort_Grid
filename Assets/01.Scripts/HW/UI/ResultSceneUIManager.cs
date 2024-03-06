using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultSceneUIManager : MonoBehaviour
{
    TextMeshProUGUI _aliveTime;
    TextMeshProUGUI _caughtEnemy;
    TextMeshProUGUI _moveCount;
    TextMeshProUGUI _totalDamage;
    TextMeshProUGUI _maxHealth;
    TextMeshProUGUI _attackDamage;
    TextMeshProUGUI _attackSpeed;
    TextMeshProUGUI _usedWeapon;
    [SerializeField]
    CinemachineVirtualCamera _cam;

    private void Awake()
    {
        Transform trm = transform.Find("Result");
        _aliveTime = trm.Find("AliveTime").GetComponent<TextMeshProUGUI>();
        _caughtEnemy = trm.Find("CaughtEnemy").GetComponent<TextMeshProUGUI>();
        _moveCount = trm.Find("MoveCount").GetComponent<TextMeshProUGUI>();
        _totalDamage = trm.Find("TotalDamage").GetComponent<TextMeshProUGUI>();
        _maxHealth = trm.Find("MaxHealth").GetComponent<TextMeshProUGUI>();
        _attackDamage = trm.Find("AttackDamage").GetComponent<TextMeshProUGUI>();
        _attackSpeed = trm.Find("AttackSpeed").GetComponent<TextMeshProUGUI>();
        _usedWeapon = trm.Find("UsedWeapon").GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        _aliveTime.text = PlayerPrefs.GetFloat("AliveTime").ToString();
        _caughtEnemy.text = PlayerPrefs.GetInt("CaughtEnemy").ToString();
        _moveCount.text = PlayerPrefs.GetInt("MoveCount").ToString();
        _totalDamage.text = PlayerPrefs.GetFloat("TotalDamage").ToString();
        _maxHealth.text = PlayerPrefs.GetInt("MaxHealth").ToString();
        _attackDamage.text = PlayerPrefs.GetFloat("AttackDamage").ToString();
        _attackSpeed.text = PlayerPrefs.GetFloat("AttackSpeed").ToString();
        _usedWeapon.text = $"{PlayerPrefs.GetString("UsedWeapon")}, {PlayerPrefs.GetString("UsedWeapon2")}";
        _cam.m_Lens.OrthographicSize = 0f;
        DOTween.To(() => _cam.m_Lens.OrthographicSize, v => _cam.m_Lens.OrthographicSize = v, 5.76176f, 1.5f).SetEase(Ease.OutCubic);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("Rank");
        }
    }
}
