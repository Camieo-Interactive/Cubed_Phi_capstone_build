using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.Collections;

/// <summary>
/// Manages enemy spawning, increasing difficulty dynamically based on enemy deaths.
/// </summary>
public class EnemyManager : SingletonBase<EnemyManager>
{
    //  ------------------ Public ------------------

    [Header("Spawn Settings")]
    [Space(5)]
    [Tooltip("List of enemy prefabs to spawn.")]
    public List<GameObject> enemyPrefabs;

    [Tooltip("Spawn points for enemies.")]
    public Transform[] spawnPoints;

    [Tooltip("Initial number of enemies required to progress to the next stage.")]
    public int baseEnemiesToNextStage = 5;

    [Tooltip("Total number of stages to finish the level.")]
    public int totalStages = 15;

    [Tooltip("Time between enemy spawns.")]
    public float spawnInterval = 3f;

    [Tooltip("How long it takes to start the game.")]
    public float startDelay = 10f;

    [Tooltip("Maximum number of active enemies at once.")]
    public int maxEnemies = 10;

    [Header("Tier Settings")]
    [Space(5)]
    [Tooltip("Current tier of the game.")]
    [ReadOnly]
    public int CurrentTier = 1;

    [Tooltip("Lowest lane available for enemy spawn.")]
    [ReadOnly]
    public int LowestLane = -7;

    [Tooltip("Highest lane available for enemy spawn.")]
    [ReadOnly]
    public int highestLane = 0;
    
    [Tooltip("Stride value for lane spacing.")]
    [ReadOnly]
    public int strideLane = 1;

    [Header("Mine Settings")]
    [Space(5)]
    [Tooltip("Prefab for the mine to be placed on each lane.")]
    public GameObject minePrefab;

    [Header("Mine Placement Settings")]
    [Space(5)]
    [Tooltip("X coordinate where mines will be placed.")]
    public float mineXCoordinate;

    [Header("Wave Settings")]
    [Space(5)]
    [Tooltip("Enemy waves to spawn at specific stage intervals.")]
    public List<EnemyWave> enemyWaves;

    [Header("UI Elements")]
    [Space(5)]
    [Tooltip("Progress bar showing the number of enemy kills until the next stage.")]
    public Image progressBar;

    [Header("Game End Elements")]
    [Space(5)]
    [Tooltip("Canvas used for displaying the game end UI.")]
    public GameObject GameEndCanvas;

    [Tooltip("Game over screen displayed when the player loses.")]
    public GameObject GameOverScreen;

    [Tooltip("Game finished screen displayed when the player wins.")]
    public GameObject GameFinishedScreen;

    [Header("Wave Notification")]
    [Space(5)]
    [Tooltip("Wave incoming handler for displaying wave notifications.")]
    public WaveIncomingHandler waveIncomingHandler;
    public override void PostAwake() { }

    /// <summary>
    /// Displays the Game Over screen.
    /// </summary>
    public void GameOver()
    {
        // GameWinScreen.SetActive(true);
        GameOverScreen.SetActive(true);
        GameEndCanvas.SetActive(true);
        Time.timeScale = 0;
    }

    /// <summary>
    /// Ends the game and returns to the main menu.
    /// </summary>
    public void EndGame()
    {
        Time.timeScale = 1;
        GameOverScreen.SetActive(false);
        GameFinishedScreen.SetActive(false);
        GameEndCanvas.SetActive(false);
        SceneManager.LoadScene("MenuScene");
    }

    /// <summary>
    /// Spawns an enemy instance at the given position.
    /// </summary>
    public void SpawnEnemyInstance(GameObject enemyPrefab, Vector3 pos)
    {
        GameObject enemy = PoolManager.Instance.GetObject(enemyPrefab, pos, Quaternion.identity);
        EnemyBase enemyBase = enemy.GetComponent<EnemyBase>();
        enemyBase.OnDeathCallback = OnEnemyDefeated;
        _enemyList.Add(enemy); // Track enemy
    }


    //  ------------------ Private ------------------

    private int _enemiesDefeated = 0;
    private int _currentEnemyCap = 1;
    private bool _isSpawning = false;
    private bool _isWaveInProgress = false;
    private HashSet<int> _wavesPassed = new(); // Tracks passed waves
    private List<GameObject> _enemyList = new(); // Tracks active enemies

    /// <summary>
    /// Manages the dynamic spawning of enemies.
    /// </summary>
    private IEnumerator EnemySpawnController()
    {
        yield return new WaitForSeconds(startDelay);

        while (CurrentTier <= totalStages)
        {
            // Check if we should spawn a wave based on the current tier
            EnemyWave currentWave = enemyWaves.FirstOrDefault(
                w => w.interval == CurrentTier
                &&
                !_wavesPassed.Contains(w.interval)
            );
            Debug.Log($"Current Tier: {CurrentTier}, Current Wave: {currentWave?.name}");
            if (currentWave != null && !_isWaveInProgress)
            {
                _wavesPassed.Add(CurrentTier); // Track passed waves
                StartCoroutine(SpawnEnemyWave(currentWave));
                yield return new WaitUntil(() => !_isWaveInProgress);
            }
            else if (_enemyList.Count < _currentEnemyCap && !_isSpawning)
            {
                StartCoroutine(SpawnEnemy());
            }

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
        Debug.Log("End of the stage, show a screen here!");
        yield return null;
    }

    /// <summary>
    /// Places mines along all lanes at the specified X coordinate.
    /// </summary>
    private void PlaceMines()
    {
        for (int lane = LowestLane; lane <= highestLane; lane += strideLane)
        {
            Vector3 minePosition = new Vector3(mineXCoordinate, lane, 0);
            PoolManager.Instance.GetObject(minePrefab, minePosition, Quaternion.identity);
        }
    }

    /// <summary>
    /// Spawns a wave of enemies based on the EnemyWave definition
    /// </summary>
    private IEnumerator SpawnEnemyWave(EnemyWave wave)
    {
        _isWaveInProgress = true;
        waveIncomingHandler.showWaveIncoming(wave.isFinalWave);
        Debug.Log($"Starting Wave at Tier {CurrentTier}!");

        // Spawn all enemies in the wave
        foreach (GameObject enemyPrefab in wave.enemyPrefabs)
        {
            // Wait until we're under the enemy cap
            yield return new WaitUntil(() => _enemyList.Count < maxEnemies);

            // Choose a random spawn point
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            SpawnEnemyInstance(enemyPrefab, spawnPoint.position);

            // Small delay between spawns in the same wave
            yield return new WaitForSeconds(0.5f);
        }

        _isWaveInProgress = false;
        Debug.Log("Wave completed!");
    }

    /// <summary>
    /// Spawns a single enemy at a random location, selecting from available tiers.
    /// </summary>
    private IEnumerator SpawnEnemy()
    {
        _isSpawning = true;

        if (enemyPrefabs.Count == 0 || spawnPoints.Length == 0) yield break;

        // Filter available enemies based on the current tier
        List<GameObject> availableEnemies = enemyPrefabs.Where(e => e.GetComponent<EnemyBase>().stats.tier <= CurrentTier).ToList();

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
        int requiredEnemiesThisStage = baseEnemiesToNextStage * CurrentTier;
        if (_enemiesDefeated >= requiredEnemiesThisStage)
        {
            _enemiesDefeated = 0;
            _currentEnemyCap = Mathf.Min(_currentEnemyCap + 1, maxEnemies);

            // Increase enemy tier after every stage progression
            CurrentTier++;
            Debug.Log($"Stage Progressed! New Enemy Cap: {_currentEnemyCap}, Unlocked Tier: {CurrentTier}");

            UpdateProgressBar(); // Reset progress bar on new stage
        }
    }

    /// <summary>
    /// Updates the UI progress bar based on enemy defeats.
    /// </summary>
    private void UpdateProgressBar()
    {
        if (progressBar != null) progressBar.fillAmount = (float)CurrentTier / totalStages;
    }

    private void OnEnable()
    {
        // Find UI elements
        GameObject gameUI = GameObject.FindGameObjectWithTag("GameUi");
        GameEndCanvas = gameUI.transform.Find("GameOverCanvas")?.gameObject;
        GameOverScreen = GameEndCanvas.transform.Find("GameOver")?.gameObject;
        GameFinishedScreen = GameEndCanvas.transform.Find("GameEnd")?.gameObject;
        if (spawnPoints != null && spawnPoints.Length > 0)
        {
            LowestLane = Mathf.FloorToInt(spawnPoints.Min(t => t.position.y));
            highestLane = Mathf.CeilToInt(spawnPoints.Max(t => t.position.y));
            strideLane = Mathf.Max(1, Mathf.RoundToInt((highestLane - LowestLane) / (spawnPoints.Length - 1)));
        }
        PlaceMines();
        StartCoroutine(EnemySpawnController());
        UpdateProgressBar(); // Initialize progress bar
    }
    private void OnDisable()
    {
        
    }
}