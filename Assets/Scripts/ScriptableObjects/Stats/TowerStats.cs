using System;
using UnityEngine;

/// <summary>
/// Defines the core statistics for a tower.
/// </summary>
/// <remarks>
/// Used as a ScriptableObject to store and manage tower attributes such as range, health, cost, and attack speed.
/// </remarks>
[CreateAssetMenu(fileName = "New Tower Stats", menuName = "Tower/Tower Stats")]
[Serializable]
public class TowerStats : ScriptableObject
{
    //  ------------------ Public ------------------
    
    [Header("Tower Attributes")]
    
    [Tooltip("The attack range of the tower.")]
    public float range;

    [Tooltip("The health of the tower.")]
    public int health;

    [Tooltip("The resource cost to build the tower.")]
    public int cost;

    [Tooltip("The rate at which the tower fires projectiles (shots per second).")]
    public float fireRate;

    [Tooltip("The recharge time needed before the tower can fire again.")]
    public float rechargeTime;
}
