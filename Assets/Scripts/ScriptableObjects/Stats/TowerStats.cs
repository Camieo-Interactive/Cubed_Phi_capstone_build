using System;
using UnityEngine;

/// <summary>
/// Defines the core statistics for a tower.
/// </summary>
/// <remarks>
/// Used as a ScriptableObject to store and manage tower attributes such as damage, range, health, cost, and attack speed.
/// </remarks>
[CreateAssetMenu(fileName = "New Tower Stats", menuName = "Tower/Tower Stats")]
[Serializable]
public class TowerStats : ScriptableObject
{
    //  ------------------ Public ------------------

    [Header("Tower Attributes")]

    [Tooltip("The damage dealt by the tower.")]
    public int damage;
    [Tooltip("The status dealt by the tower.")]
    public DamageStatus status;

    [Tooltip("The attack range of the tower.")]
    public float range;

    [Tooltip("The health of the tower.")]
    public int health;

    [Tooltip("The rate at which the tower fires projectiles (shots per second).")]
    public float fireRate;

    [Tooltip("The recharge time needed before the tower can fire again.")]
    public float rechargeTime;

    [Tooltip("The projectile used by the tower when firing.")]
    public GameObject projectile;

    [Tooltip("The layer mask used to detect enemies within range.")]
    public LayerMask detectionMask;

    [Tooltip("Sell value")]
    public int sellValue = 25;

    [Tooltip("Shoot FX")]
    public GameObject shootParticleSystem; 

    [Tooltip("Death FX")]
    public GameObject deathParticleSystem; 
}
