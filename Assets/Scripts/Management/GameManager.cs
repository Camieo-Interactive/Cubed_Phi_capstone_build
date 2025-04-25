using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;
using TMPro;
using UnityEngine;

/// <summary>
/// Manages game state, including tracking and displaying the collected bits.
/// </summary>
/// <remarks>
/// This singleton class subscribes to the OnBitChange event and updates the total bits collected.
/// When the event is triggered, the UI is updated to reflect the current total bits collected.
/// </remarks>
public class GameManager : SingletonBase<GameManager>
{
    //  ------------------ Public ------------------

    [Header("Card List")]
    [Tooltip("Card List Scriptable Object.")]
    public CardList currentCardList;

    [Tooltip("Card currently selected.")]
    public Card selectedCard = null;

    [Header("UI Elements")]
    [Tooltip("Text UI element that displays the bit count.")]
    public TextMeshProUGUI bitCount;

    [Tooltip("The Mouse object that is the trigger for particle effects.")]
    public GameObject mouseTrigger;

    [Tooltip("Deck currently in use.")]
    public Deck CardDeck;
    public int reRollDelta = 100;
    public int reRollMax = 1000;

    [Tooltip("Grid currently in use.")]
    public Grid grid;

    public Camera mainCamera;

    [Tooltip("Dictionary tracking the locations of buildings.")]
    public Dictionary<Vector3Int, Tuple<bool, GameObject>> buildingLocations = new();



    /// <summary>
    /// The total bits collected.
    /// </summary>
    public long BitsCollected = 50;

    /// <summary>
    /// Delegate for bit change events.
    /// </summary>
    /// <param name="bitDelta">The change in bits (positive or negative).</param>
    public delegate void ChangeBits(long bitDelta);

    /// <summary>
    /// Event triggered when the bit count changes.
    /// </summary>
    public static event ChangeBits OnBitChange;

    /// <summary>
    /// Called after the singleton instance is initialized.
    /// </summary>
    public override void PostAwake()
    {
        bitCount.text = $"Bits: {BitsCollected}";
    }

    /// <summary>
    /// Raises the OnBitChange event with the specified delta.
    /// </summary>
    /// <param name="bitDelta">The change in bits (positive or negative).</param>
    public static void RaiseBitChange(long bitDelta) => OnBitChange?.Invoke(bitDelta);

    public Vector2 GetRandomOffScreenPoint(float distanceFromEdge)
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        // Get camera boundaries in world space
        float camHeight = 2f * mainCamera.orthographicSize;
        float camWidth = camHeight * mainCamera.aspect;

        float minX = mainCamera.transform.position.x - camWidth / 2f;
        float maxX = mainCamera.transform.position.x + camWidth / 2f;
        float minY = mainCamera.transform.position.y - camHeight / 2f;
        float maxY = mainCamera.transform.position.y + camHeight / 2f;

        Vector2 spawnPos = Vector2.zero;

        // Choose a random side: 0 = Left, 1 = Right, 2 = Top, 3 = Bottom
        int side = UnityEngine.Random.Range(0, 4);

        switch (side)
        {
            case 0: // Left
                spawnPos.x = minX - distanceFromEdge;
                spawnPos.y = UnityEngine.Random.Range(minY, maxY);
                break;
            case 1: // Right
                spawnPos.x = maxX + distanceFromEdge;
                spawnPos.y = UnityEngine.Random.Range(minY, maxY);
                break;
            case 2: // Top
                spawnPos.y = maxY + distanceFromEdge;
                spawnPos.x = UnityEngine.Random.Range(minX, maxX);
                break;
            case 3: // Bottom
                spawnPos.y = minY - distanceFromEdge;
                spawnPos.x = UnityEngine.Random.Range(minX, maxX);
                break;
        }

        return spawnPos;
    }

    //  ------------------ Private ------------------

    /// <summary>
    /// Handles bit change events by updating the total bits and the UI text.
    /// </summary>
    /// <param name="bitDelta">The change in bits (positive or negative).</param>
    private void HandleBitChange(long bitDelta) => bitCount.text = $"Bits: {BitsCollected += bitDelta}";


    /// <summary>
    /// Subscribes to the bit change event.
    /// </summary>
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        OnBitChange += HandleBitChange;
    }

    /// <summary>
    /// Unsubscribes from the bit change event.
    /// </summary>
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        OnBitChange -= HandleBitChange;
    }

    /// <summary>
    /// Handles scene load logic.
    /// </summary>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reset the list..
        BitsController.triggers = new List<GameObject>();
        buildingLocations = new();
        BitsController.AddTrigger(mouseTrigger);
    }

}
