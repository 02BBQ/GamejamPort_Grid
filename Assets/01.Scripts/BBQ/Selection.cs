using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class Selection : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
{
    public BuffSO buffSO;
    public WeaponSO weaponSO;
    private new TMP_Text name;
    private TMP_Text desc;
    private float Amount;

    private UIHandler uiHandler;

    [SerializeField] private Image skin;
    [SerializeField] private Image skinRed;

    private bool checker;

    private float fadeTime = .1f;

    private void Awake()
    {
        name = transform.Find("Name").GetComponent<TMP_Text>();
        desc = transform.Find("Desc").GetComponent<TMP_Text>();
        uiHandler = transform.root.GetComponent<UIHandler>();
    }

    private void OnEnable()
    {
        checker = false;
        skin.transform.localScale = new Vector3 (1f, 1f, 1f);
        transform.localScale = new Vector3 (1f, 1f, 1f);
        if (buffSO != null)
        {
            name.text = buffSO.Name;
            if (buffSO.AttackSpeedUp)
            {
                Amount = Random.Range(2, 6) / 10f;
            }
            if (buffSO.DamageUp) Amount = Random.Range(2, 5) / 10f;
            if (buffSO.HealthUp) Amount = Random.Range(1, 3);
            if (buffSO.MaxHealthUp) Amount = 1;
            if (buffSO.MaxHealthUp && Player.Instance.MaxHealth >= 10)
            {
                desc.text = $"최대 체력에 도달하여 체력을 1 회복합니다!";
                buffSO.MaxHealthUp = true;
            }
            else if(buffSO.AttackSpeedUp && Player.Instance.AttackSpeed >= 10)
            {
                desc.text = $"최대 공격속도에 도달하여 공격력을 0.3 증가시킵니다!";
                Amount = 0.3f;
            }
            else
            {
                desc.text = $"{buffSO.Description}".Replace("$N", $"{Amount}");
            }
        }
        if (weaponSO != null) 
        {
            name.text = weaponSO.Name;
            desc.text = weaponSO.Description;
        }
    }

    public void Select()
    {
        if (weaponSO == null)
        {
            if (!uiHandler.Select(buffSO, Amount))
            {
                print("응어아잇");
                desc.text = "최대 스탯에 도달하였습니다!";
                return;
            }
            
        }
        else
        {
            uiHandler.Select(weaponSO);
        }
        buffSO = null;
        weaponSO = null;
        gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData pointClick)
    {
        Select();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        checker = true;
        skin.transform.DOScale(1.1f, fadeTime).SetUpdate(true);
        skinRed.transform.DOScale(1.1f, fadeTime).SetUpdate(true);
        transform.DOScale(1.1f, fadeTime).SetUpdate(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        checker = false;
        skin.transform.DOScale(1f, fadeTime).SetUpdate(true);
        skinRed.transform.DOScale(1f, fadeTime).SetUpdate(true);
        transform.DOScale(1f, fadeTime).SetUpdate(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!checker)
        {
            skin.transform.DOScale(1.1f, fadeTime).SetUpdate(true);
            skinRed.transform.DOScale(1.1f, fadeTime).SetUpdate(true);
            transform.DOScale(1.1f, fadeTime).SetUpdate(true);
            checker = true;
        }
    }
}
