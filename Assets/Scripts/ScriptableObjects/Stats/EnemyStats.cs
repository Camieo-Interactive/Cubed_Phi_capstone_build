using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Stats", menuName = "Enemy/Enemy Stats")]
[Serializable]
public class EnemyStats : ScriptableObject
{
    public float movementSpeed;
    public int health;
    public GameObject spawnable;
    public int bitsOnKill = 25;
    
    [Tooltip("Tier of the enemy (Higher tiers spawn later).")]
    public int tier;
}
