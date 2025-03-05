using System;
using UnityEngine;

/// <summary>
/// Defines the core statistics for an enemy.
/// </summary>
/// <remarks>
/// Used as a ScriptableObject to store and manage enemy attributes such as movement speed, health, and tier.
/// </remarks>
[CreateAssetMenu(fileName = "New Enemy Stats", menuName = "Enemy/Enemy Stats")]
[Serializable]
public class EnemyStats : ScriptableObject
{
    //  ------------------ Public ------------------

    [Header("Enemy Attributes")]
    
    [Tooltip("The movement speed of the enemy.")]
    public float movementSpeed;

    [Tooltip("The health of the enemy.")]
    public int health;

    [Tooltip("Prefab that spawns when the enemy dies.")]
    public GameObject spawnable;

    [Tooltip("The number of bits awarded upon killing this enemy.")]
    public int bitsOnKill = 25;

    [Tooltip("Tier of the enemy (Higher tiers spawn later).")]
    public int tier;
}
