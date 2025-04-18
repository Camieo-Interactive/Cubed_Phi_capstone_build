using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New Enemy Stats", menuName = "Enemy/Enemy Wave")]
[Serializable]
public class EnemyWave : ScriptableObject {
    [Header("Enemy Wave")]
    [Tooltip("List of enemy prefabs to spawn.")]
    public List<GameObject> enemyPrefabs;
    [Tooltip("Which wave interval should this wave spawn?")]
    public int interval; 
}