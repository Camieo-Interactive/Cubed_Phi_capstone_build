using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages enemy spawning, increasing difficulty dynamically based on enemy deaths.
/// </summary>
public class EnemyManager : SingletonBase<EnemyManager>
{
    [Header("Spawn Settings")]
    [Tooltip("List of enemy prefabs to spawn.")]
    public List<GameObject> enemyPrefabs;

    [Tooltip("Spawn points for enemies.")]
    public Transform[] spawnPoints;

    [Tooltip("Initial enemies required to progress.")]
    public int enemiesToNextStage = 5;

    [Tooltip("Time between enemy spawns.")]
    public float spawnInterval = 3f;

    [Tooltip("Maximum number of active enemies at once.")]
    public int maxEnemies = 10;

    [Header("UI Elements")]
    [Tooltip("Progress bar showing enemy kills until the next stage.")]
    public Image progressBar;

    private int _enemiesDefeated = 0;
    private int _currentEnemyCap = 1;
    private int _activeEnemies = 0;
    private bool _isSpawning = false;
    private int _currentTier = 1;

    public override void PostAwake() { }

    private void Start()
    {
        StartCoroutine(EnemySpawnController());
        UpdateProgressBar(); // Initialize progress bar
    }

    /// <summary>
    /// Manages the dynamic spawning of enemies.
    /// </summary>
    private IEnumerator EnemySpawnController()
    {
        while (true)
        {
            if (_activeEnemies < _currentEnemyCap && !_isSpawning)
            {
                StartCoroutine(SpawnEnemy());
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    /// <summary>
    /// Spawns a single enemy at a random location, selecting from available tiers.
    /// </summary>
    private IEnumerator SpawnEnemy()
    {
        _isSpawning = true;

        if (enemyPrefabs.Count == 0 || spawnPoints.Length == 0) yield break;

        // Filter available enemies based on the current tier
        List<GameObject> availableEnemies = enemyPrefabs
            .Where(e => e.GetComponent<EnemyBase>().stats.tier <= _currentTier)
            .ToList();

        if (availableEnemies.Count == 0)
        {
            _isSpawning = false;
            yield break;
        }

        GameObject enemyPrefab = availableEnemies[Random.Range(0, availableEnemies.Count)];
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        GameObject enemy = PoolManager.Instance.GetObject(enemyPrefab, spawnPoint.position, Quaternion.identity);
        EnemyBase enemyBase = enemy.GetComponent<EnemyBase>();
        enemyBase.OnDeathCallback = OnEnemyDefeated;
        enemyBase.Init();

        _activeEnemies++;

        yield return new WaitForSeconds(spawnInterval);
        _isSpawning = false;
    }

    /// <summary>
    /// Called when an enemy is defeated.
    /// </summary>
    private void OnEnemyDefeated()
    {
        _activeEnemies--;
        _enemiesDefeated++;

        UpdateProgressBar();

        // Progression logic
        if (_enemiesDefeated >= enemiesToNextStage)
        {
            _enemiesDefeated = 0;
            _currentEnemyCap = Mathf.Min(_currentEnemyCap + 1, maxEnemies);

            // Increase enemy tier after every stage progression
            _currentTier++;
            Debug.Log($"Stage Progressed! New Enemy Cap: {_currentEnemyCap}, Unlocked Tier: {_currentTier}");

            UpdateProgressBar(); // Reset progress bar on new stage
        }
    }

    /// <summary>
    /// Updates the UI progress bar based on enemy defeats.
    /// </summary>
    private void UpdateProgressBar()
    {
        if (progressBar != null)
        {
            progressBar.fillAmount = (float)_enemiesDefeated / enemiesToNextStage;
        }
    }
}
