using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    [Tooltip("Total Stages to finish the level.")]
    public int totalStages = 15;

    [Tooltip("Time between enemy spawns.")]
    public float spawnInterval = 3f;

    [Tooltip("How long it takes to start the game.")]
    public float startDelay = 10f;

    [Tooltip("Maximum number of active enemies at once.")]
    public int maxEnemies = 10;


    [Header("UI Elements")]
    [Tooltip("Progress bar showing enemy kills until the next stage.")]
    public Image progressBar;
    public GameObject GameEndCanvas;
    public GameObject GameOverScreen;
    public GameObject GameFinishedScreen;

    private int _enemiesDefeated = 0;
    private int _currentEnemyCap = 1;
    private bool _isSpawning = false;
    private int _currentTier = 1;
    private List<GameObject> _enemyList = new(); // Tracks active enemies

    public override void PostAwake() { }

    public void GameOver()
    {
        GameEndCanvas.SetActive(true);
        GameOverScreen.SetActive(true);
        Time.timeScale = 0;
    }

    public void EndGame()
    {
        Time.timeScale = 1;
        GameOverScreen.SetActive(false);
        GameFinishedScreen.SetActive(false);
        GameEndCanvas.SetActive(false);
        SceneManager.LoadScene("MenuScene");
    }

    public void SpawnEnemyInstance(GameObject enemyPrefab, Vector3 pos)
    {
        GameObject enemy = PoolManager.Instance.GetObject(enemyPrefab, pos, Quaternion.identity);
        EnemyBase enemyBase = enemy.GetComponent<EnemyBase>();
        enemyBase.OnDeathCallback = OnEnemyDefeated;
        enemyBase.Init();

        _enemyList.Add(enemy); // Track enemy
    }

    /// <summary>
    /// Manages the dynamic spawning of enemies.
    /// </summary>
    private IEnumerator EnemySpawnController()
    {
        yield return new WaitForSeconds(startDelay);
        while (_currentTier < totalStages)
        {
            if (_enemyList.Count < _currentEnemyCap && !_isSpawning) StartCoroutine(SpawnEnemy());
            yield return new WaitForSeconds(spawnInterval);
        }


        // Wait for all enemies killed.. 
        while (_enemyList.Count > 0)
        {
            yield return new WaitForSeconds(spawnInterval);
        }
        GameEndCanvas.SetActive(true);
        GameFinishedScreen.SetActive(true);
        Time.timeScale = 0;
        Debug.Log("End of the stage show a screen here!");
        yield return null;
    }

    /// <summary>
    /// Spawns a single enemy at a random location, selecting from available tiers.
    /// </summary>
    private IEnumerator SpawnEnemy()
    {
        _isSpawning = true;

        if (enemyPrefabs.Count == 0 || spawnPoints.Length == 0) yield break;

        // Filter available enemies based on the current tier
        List<GameObject> availableEnemies = enemyPrefabs.Where(e => e.GetComponent<EnemyBase>().stats.tier <= _currentTier).ToList();

        if (availableEnemies.Count == 0)
        {
            _isSpawning = false;
            yield break;
        }

        GameObject enemyPrefab = availableEnemies[Random.Range(0, availableEnemies.Count)];
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        SpawnEnemyInstance(enemyPrefab, spawnPoint.position);


        yield return new WaitForSeconds(spawnInterval);
        _isSpawning = false;
    }

    /// <summary>
    /// Called when an enemy is defeated.
    /// </summary>
    private void OnEnemyDefeated(GameObject enemy)
    {
        _enemiesDefeated++;
        _enemyList.Remove(enemy);
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
            progressBar.fillAmount = (float)(_currentTier - 1) / totalStages;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject gameUI = GameObject.FindGameObjectWithTag("GameUi");
        GameEndCanvas = gameUI.transform.Find("GameOverCanvas")?.gameObject;
        GameOverScreen = GameEndCanvas.transform.Find("GameOver")?.gameObject;
        GameFinishedScreen = GameEndCanvas.transform.Find("GameEnd")?.gameObject;
        StartCoroutine(EnemySpawnController());
        UpdateProgressBar(); // Initialize progress bar
    }
    private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;
}
