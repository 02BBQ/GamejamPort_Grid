using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/EnemySO")]
public class EnemySO : ScriptableObject
{
    public float hp;
    public float attackDelay;
    public float damage;
    public int exp;
}
