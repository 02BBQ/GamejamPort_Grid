using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class SelectionHandler : MonoBehaviour
{
    public BuffSO[] baseBuffSO;
    private BuffSO[] buffSO;

    public WeaponSO[] baseWeaponSO;
    private List<WeaponSO> weaponSO;

    public bool isWeaponSelect = false;

    public WeaponSO selectedWeapon;

    public TMP_Text title;

    [SerializeField] private Transform Selections;
    [SerializeField] private Transform Skins;
    [SerializeField] private Transform SkinsRed;

    private void Init()
    {
        SkinsRed.gameObject.SetActive(false);
        Skins.gameObject.SetActive(false);
        if (!isWeaponSelect)
        {
            title.text = "획득할 버프를 선택하십시오.";
            buffSO = baseBuffSO.ToArray();
            BuffSO[] selected = new BuffSO[3];
            List<int> selectedIndexes = new List<int>();

            for (int i = 0; i <= 2; i++)
            {
                int selectedIndex;
                do
                {
                    selectedIndex = Random.Range(0, baseBuffSO.Length);
                } while (selectedIndexes.Contains(selectedIndex));

                selectedIndexes.Add(selectedIndex);

                selected[i] = buffSO[selectedIndex];
                buffSO[selectedIndex] = null; // 선택된 파워업 제거
            }
            foreach (Transform tr in Selections)
            {
                tr.gameObject.GetComponent<Selection>().weaponSO = null;
                tr.gameObject.GetComponent<Selection>().buffSO = selected[tr.GetSiblingIndex()];
                tr.gameObject.SetActive(true);
                //tr.gameObject.GetComponent<UISpriteAnimation>().
            }
            Skins.gameObject.SetActive(true);
            
        }
        else if(baseWeaponSO.Length > 2)
        {
            weaponSO = baseWeaponSO.ToList();
            WeaponSO[] selected = new WeaponSO[3];
            List<int> selectedIndexes = new List<int>();

            title.text = "획득할 무기를 선택하십시오.";

            if (weaponSO.Contains(Player.Instance.Weapon))
            {
                title.text = "획득할 두 번째 무기를 선택하십시오.";
                weaponSO.Remove(Player.Instance.Weapon);
            }

            for (int i = 0; i <= 2; i++)
            {
                int selectedIndex;
                do
                {
                    selectedIndex = Random.Range(0, weaponSO.Count);
                } while (selectedIndexes.Contains(selectedIndex));

                selectedIndexes.Add(selectedIndex);

                selected[i] = weaponSO[selectedIndex];
                weaponSO[selectedIndex] = null; // 선택된 파워업 제거
            }
            foreach (Transform tr in Selections)
            {
                tr.gameObject.GetComponent<Selection>().buffSO = null;
                tr.gameObject.GetComponent<Selection>().weaponSO = selected[tr.GetSiblingIndex()];
                tr.gameObject.SetActive(true);
            }

            SkinsRed.gameObject.SetActive(true);
        }
    }
    private void Awake()
    {
        Init();
    }
    private void OnEnable()
    {
        Init();
    }
    private void OnDisable()
    {
        foreach (Transform tr in Selections)
        {
            tr.gameObject.SetActive(false);
        }
    }
}
