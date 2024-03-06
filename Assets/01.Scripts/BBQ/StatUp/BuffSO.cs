using UnityEngine;

[CreateAssetMenu(fileName = "Buff Data", menuName = "Scriptable Object/Buff Data")]
public class BuffSO : ScriptableObject
{
    [SerializeField] private new string name;
    [SerializeField] private string description;

    public string Name => name;
    public string Description => description;

    public bool HealthUp;
    public bool MaxHealthUp;
    public bool AttackSpeedUp;
    public bool DamageUp;
}
