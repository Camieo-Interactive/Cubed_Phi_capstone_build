using System;
using UnityEngine;

/// <summary>
/// Defines the core statistics for an enemy.
/// </summary>
/// <remarks>
/// Used as a ScriptableObject to store and manage enemy attributes such as movement speed, health, and tier.
/// </remarks>
[CreateAssetMenu(fileName = "New Enemy Attack Stats", menuName = "Enemy/Enemy Attack Stats")]
[Serializable]
public class EnemyAttackStats : ScriptableObject
{
    //  ------------------ Public ------------------

    [Header("Attack Enemy Stats Stats")]
    [Space(8)]
    [Tooltip("The attack range of the enemy.")]
    public float AttackRange = 30f;

    [Tooltip("The attack duration. [Fire rate or duration]")]
    public float AttackDuration = 10f;
    
    [Tooltip("The attack cooldown.")]
    public float AttackCooldownDuration = 10f;

    [Tooltip("The layer in which the attacks work.")]
    public LayerMask AttackMask;

}
