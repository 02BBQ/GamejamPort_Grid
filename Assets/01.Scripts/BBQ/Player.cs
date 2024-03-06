using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoSingleton<Player>
{
    private PlayerMovement playerMovement;
    private SpriteRenderer _spriteRenderer;
    private Material _defaultMaterial;

    [Header("States")]
    [SerializeField] private bool dead = false;

    [Header("Stats")]
    public float maxHealth = 3;
    public float MaxHealth
    {
        get => maxHealth;
        set
        {
            maxHealth = Mathf.Clamp(value, 0, 10);
            uiHandler.HealthBarUpdate();
            Health++;
        }
    }
    private float health;
    [SerializeField] private float attackSpeed;
    private float lastHealth;
    public float Health
    {
        get => health;
        set
        {
            lastHealth = health;
            health = Mathf.Clamp(value, 0, MaxHealth);
            OnHealthChange();
        }
    }

    public float AttackSpeed
    {
        get => attackSpeed;
        set => attackSpeed = Mathf.Clamp(value, 1, 10);
    }
    private float Cooldown;
    private float BaseDamage;
    public float Damage;
    public int Level;
    public int MaxExp;
    public int exp;
    public int Exp
    {
        get { return exp; }
        set { exp = value; OnExpChanged(); }

    }

    [SerializeField] private WeaponSO weapon;
    public WeaponSO Weapon
    {
        get { return weapon; }
        set
        {
            weapon = value;
            OnWeaponChange();
        }
    }
    [Header("UI")]
    public UIHandler uiHandler;
    [SerializeField] private Image hitScreen;
    [SerializeField] private Transform healthParent;

    [SerializeField] private GameObject EmptyHeart;
    [SerializeField] private GameObject Heart;

    [Header("Instance")]
    private Transform _weaponRange;
    [SerializeField] private GameObject hitboxPrefab;
    [SerializeField] private GameObject noWeaponEffect;

    private Tween hitTween;

    public WeaponSO HoldingWeapon;

    //[Header("system")]
    //public WeaponSO WEAPON_TO_TEST;
    [Header("통계")]
    public float DealtDamage;


    private void Init()
    {
        AttackSpeed = 1f;
        Cooldown = 1.5f;
        BaseDamage = 1f;
        Damage = 1f;
        MaxHealth = 3;
        Health = 3;
        MaxExp = 5;
        Exp = 0;
    }

    private void Start()
    {
        uiHandler.HealthBarUpdate();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _defaultMaterial = _spriteRenderer.material;
        playerMovement = GetComponent<PlayerMovement>();
        _weaponRange = playerMovement._weaponTrm;
        Init();
        StartCoroutine("StartAttack");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && HoldingWeapon != null && !dead)
        {
            StopCoroutine("StartAttack");
            StartCoroutine("StartAttack");
            var temp = Weapon;
            Weapon = HoldingWeapon;
            HoldingWeapon = temp;
            uiHandler.transform.Find("HoldingOne").GetComponent<TMP_Text>().text = $"교대 가능 : {HoldingWeapon.Name}";
        }
    }

    //[ContextMenu("GIVEXP NOW")]
    //public void GiveExp()
    //{
    //    Exp += 100;
    //}

    private void OnHealthChange()
    {
        uiHandler.HealthBarUpdate();
        if (health <= 0)
        {
            Die();
        }
    }

    public void Hit(float amount)
    {
        if (dead) return;
        TimeController.Instance.SetTimeFreeze(0.2f, 0, 0.5f);
        CameraManager.Instance.CameraShake(4f, 1f);
        Health -= amount;
        StopCoroutine("IEHit");
        StartCoroutine("IEHit");
    }

    IEnumerator IEHit()
    {
        _spriteRenderer.material = EnemySpawner.Instance.whiteMat;
        if (hitTween != null && hitTween.IsPlaying())
            hitTween.Kill();
        hitTween = hitScreen.DOFade(.25f, .05f)
           .OnComplete(() => hitScreen.DOFade(0f, .3f));
        yield return new WaitForSeconds(0.3f);
        _spriteRenderer.material = _defaultMaterial;
    }

    private void Die()
    {
        if (dead) return;
        dead = true;
        StopCoroutine("StartAttack");
        playerMovement.enabled = false;
        PlayerPrefs.SetFloat("AliveTime", InGameManager.Instance.currentTime);
        PlayerPrefs.SetInt("CaughtEnemy", InGameManager.Instance.CaughtEnemy);
        PlayerPrefs.SetInt("MoveCount", playerMovement.MoveCount);
        PlayerPrefs.SetInt("MaxHealth", (int)MaxHealth);
        PlayerPrefs.SetFloat("AttackDamage", Damage);
        PlayerPrefs.SetFloat("AttackSpeed", AttackSpeed);
        PlayerPrefs.SetFloat("TotalDamage", DealtDamage);
        PlayerPrefs.SetString("UsedWeapon", Weapon != null ? Weapon.name : "없음");
        PlayerPrefs.SetString("UsedWeapon2", HoldingWeapon != null ? HoldingWeapon.name : "없음");

        PlayerDead.Instance.OnDead();
    }

    public void Attack()
    {
        Vector2 attackPos = playerMovement.AttackPos;

        List<Enemy> Enemies = new List<Enemy>(EnemySpawner.Instance.enemyList);
        if (Weapon != null)
        {
            if (Weapon.Effect != null)
                PoolManager.Get(weapon.Effect, _weaponRange.position, _weaponRange.rotation);
            if (Weapon.ShakePower != 0)
                CameraManager.Instance.CameraShake(Weapon.ShakePower, .3f);
            if (Weapon.AttackSound != null)
                SoundManager.Instance.PlaySFX(Weapon.AttackSound, true, 1f);

            Vector2 plrPos = new Vector2(playerMovement.posX, playerMovement.posY);
            float angle = GetAngleFromVector(playerMovement.attackDir);

            for (int i = 0; i < Weapon.Hitbox.Length; i++)
            {
                attackPos = plrPos + RotateVector(Weapon.Hitbox[i], angle - 90);
                foreach (Enemy enemy in EnemySpawner.Instance.enemyList)
                {
                    if (enemy.gameObject == null) continue;
                    if (attackPos == new Vector2(enemy.posX, enemy.posY))
                    {
                        enemy.Hit(BaseDamage * Damage);
                        DealtDamage += BaseDamage * Damage;
                        if (Weapon.Chain > 0)
                        {
                            Enemies.Remove(enemy);
                            Transform[] eNemies = VectorCalc.GetClosestTransforms(Enemies,enemy.transform, Weapon.Chain);
                            foreach (Transform t in eNemies)
                            {
                                t.GetComponent<Enemy>().Hit(BaseDamage * Damage);
                            }
                        }
                    }
                }
            }
        }
        else
        {
            attackPos = playerMovement.AttackPos;

            SoundManager.Instance.PlaySFX(13, true);

            PoolManager.Get(noWeaponEffect, _weaponRange.position, _weaponRange.rotation);

            foreach (Enemy enemy in Enemies)
            {
                if (attackPos == new Vector2(enemy.posX, enemy.posY))
                {
                    enemy.Hit(BaseDamage * Damage);
                    DealtDamage += BaseDamage * Damage;
                }
            }
        }
    }

    private float GetAngleFromVector(Vector2 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;
    }

    public Vector2 RotateVector(Vector2 v, float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        float _x = v.x * Mathf.Cos(radian) - v.y * Mathf.Sin(radian);
        float _y = v.x * Mathf.Sin(radian) + v.y * Mathf.Cos(radian);
        return new Vector2(_x, _y);
    }

    IEnumerator StartAttack()
    {
        while (true)
        {
            yield return new WaitForSeconds(Cooldown / AttackSpeed);
            Attack();
        }
    }

    public void OnExpChanged()
    {
        //uiHandler.OnExpChanged();
        StartCoroutine(uiHandler.CaculateEXP());
    }

    private void OnWeaponChange()
    {
        foreach (Transform t in _weaponRange)
        {
            Destroy(t.gameObject);
        }
        if (Weapon != null)
        {
            foreach (Vector2 hitbox in Weapon.Hitbox)
            {
                Instantiate(hitboxPrefab, hitbox, Quaternion.identity, _weaponRange).transform.localPosition = hitbox;
            }
            BaseDamage = Weapon.BaseDamage;
            Cooldown = Weapon.CoolDown;
        }
        else
        {
            Instantiate(hitboxPrefab, Vector2.up, Quaternion.identity, _weaponRange).transform.localPosition = Vector2.up;
            BaseDamage = 1f;
            Cooldown = 1.5f;
        }
        playerMovement.UpdateHitbox();

    }
}
