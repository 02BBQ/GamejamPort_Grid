using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using Unity.VisualScripting;

public class PlayerMovement : MonoSingleton<PlayerMovement> 
{
    private Vector2 _moveInput;
    public int posX;
    public int posY;
    public Vector3 currentPos;
    private Tween tween;
    private Tween tweenRotate;

    public Vector2 attackDir;
    private Vector2 lastInput = new Vector2(0, 1);
    public Vector2 AttackPos;

    public int MoveCount;

    [SerializeField] private Transform aim;

    public Transform _weaponTrm;

    private void Awake()
    {
        posX = 2;
        posY = 2;
        currentPos = GridManager.Instance.grid[posX,posY] + GridManager.Instance.offset;
        attackDir = Vector2.right;
        AttackPos = new Vector2(posX, posY) + attackDir;
    }

    private void Start()
    {
        lastInput = new Vector2(0, 1);
        posX = 2; posY = 2;
        currentPos = GridManager.Instance.grid[posX,posY] + GridManager.Instance.offset;
    }
    private void Update()
    {
        Movement();
    }

    public void UpdateHitbox()
    {

        foreach (Transform hitbox in _weaponTrm)
        {
            Vector2 hitboxGridPos = hitbox.TransformPoint(Vector3.zero) - GridManager.Instance.offset + new Vector3(5, 5);
            if ((int)hitboxGridPos.x < 0 || (int)hitboxGridPos.x > GridManager.Instance.gridSize.x
                || (int)hitboxGridPos.y < 0 || (int)hitboxGridPos.y > GridManager.Instance.gridSize.y)
            {
                hitbox.gameObject.SetActive(false);
            }
            else hitbox.gameObject.SetActive(true);

        }
    }

    private void OnMove()
    {
        MoveCount++;
        currentPos = GridManager.Instance.grid[posX, posY];
        currentPos += GridManager.Instance.offset;

        AttackPos = new Vector2(posX, posY) + attackDir;

        if (tween != null && tween.IsActive())
        {
            tween.Kill();
        }
        tween = transform.DOMove(currentPos, .15f);
        _weaponTrm.position = currentPos;
        UpdateHitbox();

    }
    private void Movement()
    {
        if (Time.timeScale == 0) return;
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))
        {
            posY += Input.GetKeyDown(KeyCode.W) ? 1 : -1;

            posY = Mathf.Clamp(posY, 0, (int)GridManager.Instance.gridSize.y);

            OnMove();
        }
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        {
            posX += Input.GetKeyDown(KeyCode.D) ? 1 : -1;

            posX = Mathf.Clamp(posX, 0, (int)GridManager.Instance.gridSize.x);

            OnMove();
        }
    }
    public void OnAim(InputValue inputAction)
    {
        if (inputAction.Get<Vector2>() == Vector2.zero || inputAction.Get<Vector2>() == null) return;
        attackDir = inputAction.Get<Vector2>();
        if (attackDir.x != 0 && attackDir.y != 0)
        {
            attackDir -= lastInput;
        }
        lastInput = attackDir;

        AttackPos = new Vector2(posX, posY) + attackDir;

        if (tweenRotate != null)
        {
            tweenRotate.Kill();
        }

        float angle = Mathf.Atan2(attackDir.y, attackDir.x) * Mathf.Rad2Deg;

        _weaponTrm.rotation = quaternion.Euler(new Vector3(0,0, (angle - 90) * Mathf.Deg2Rad));

        tweenRotate = aim.DORotate(new Vector3(0, 0, angle - 90), .125f);

        UpdateHitbox();
    }
}
