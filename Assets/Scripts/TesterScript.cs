using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// Spawns a particle effect prefab at random positions within the camera view.
/// </summary>
/// <remarks>
/// This script calculates random positions using the camera's viewport coordinates,
/// converts them to world space, and instantiates the assigned prefab at those positions.
/// Adjust the spawn interval as needed. Attach this script to a GameObject in your scene,
/// and assign the prefab and, optionally, the main camera in the Inspector.
/// </remarks>
public class TesterScript : MonoBehaviour
{
    //  ------------------ Public ------------------
    [Header("Spawner Settings")]
    [Tooltip("The particle effect prefab to spawn.")]
    public GameObject prefab;

    [Tooltip("The Mouse object that is the trigger for particleSys")]
    public GameObject mouseTrigger;

    [Tooltip("Time interval (in seconds) between spawns.")]
    public float spawnInterval = 1.0f;

    [Tooltip("Reference to the main camera used for calculating spawn positions.")]
    public Camera mainCamera;

    //  ------------------ Private ------------------
    private float timer = 0f;

    /// <summary>
    /// Initializes the main camera reference.
    /// </summary>
    private void Start() => mainCamera = Camera.main;

    /// <summary>
    /// Updates the timer and spawns particle effects when the interval elapses.
    /// </summary>
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnParticleEffect();
            timer = 0f;
        }
    }

    /// <summary>
    /// Spawns the particle effect prefab at a random position within the camera's viewport.
    /// </summary>
    private void SpawnParticleEffect()
    {
        // Generate random viewport coordinates (values between 0 and 1)
        float randomX = UnityEngine.Random.Range(0f, 1f);
        float randomY = UnityEngine.Random.Range(0f, 1f);
        int random = UnityEngine.Random.Range(10, 100);

        // Create a viewport point with a z value ensuring the prefab appears in front of the camera.
        // Adjust the z value as needed based on your scene's setup.
        Vector3 viewportPosition = new Vector3(randomX, randomY, mainCamera.nearClipPlane + 1f);

        // Convert the viewport coordinates to world space.
        Vector3 worldPosition = mainCamera.ViewportToWorldPoint(viewportPosition);
        worldPosition.z = 0.0f;

        // Instantiate the prefab at the calculated world position.
        GameObject instance = PoolManager.Instance.GetObject(prefab, worldPosition, quaternion.identity);
        instance.GetComponent<BitsController>().StartBits(random, mouseTrigger);
    }
}
