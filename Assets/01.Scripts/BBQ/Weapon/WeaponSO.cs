using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Data", menuName = "Scriptable Object/Weapon Data")]
public class WeaponSO : ScriptableObject
{
    [SerializeField] private new string name;
    [SerializeField] private string description;
    [Tooltip("정정 기준 : 위")]
    [SerializeField] private Vector2[] hitbox;

    [SerializeField] private float baseDamage;
    [SerializeField] private float coolDown;

    [SerializeField] private GameObject effect;

    [SerializeField] private float shakePower;
    [SerializeField] private AudioClip attackSound;

    public string Name => name;
    public string Description => description;
    public Vector2[] Hitbox => hitbox;
    public float BaseDamage => baseDamage;
    public float CoolDown => coolDown;
    public GameObject Effect => effect;
    public float ShakePower => shakePower;
    public AudioClip AttackSound => attackSound;

    public int Chain;
}
