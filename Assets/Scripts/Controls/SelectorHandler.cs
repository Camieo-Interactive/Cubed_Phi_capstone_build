using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

/// <summary>
/// Handles mouse interactions with the grid, displaying a selection sprite when hovering over valid tiles.
/// </summary>
public class SelectorHandler : MonoBehaviour
{
    //  ------------------ Public ------------------

    [Header("Grid Settings")]
    [Tooltip("Reference to the Grid used for placing the sprite. It should be part of a Grid layout.")]
    public Grid grid;

    [Tooltip("Reference to the Tilemap to check for valid tiles.")]
    public Tilemap tilemap;

    [Header("Selection Elements")]
    [Tooltip("The selection sprite that appears when hovering over a tile.")]
    public GameObject selectionSprite;

    [Tooltip("The object that collects particle effects at the mouse position.")]
    public GameObject particleCollector;

    [Tooltip("Reference to the sprite renderer for color changes.")]
    public SpriteRenderer spriteRenderer;

    [Tooltip("Bits Gameobject")]
    public GameObject bitsParticleSystem;

    [Tooltip("Sell Gameobject")]
    public GameObject sellMenu;
    public GraphicRaycaster sellMenuRaycaster;

    /// <summary>
    /// Updates the position and visibility of the selection sprite based on mouse position.
    /// </summary>
    /// <param name="mouseScreenPosition">The screen position of the mouse.</param>
    public void UpdateSelectionSprite(Vector2 mouseScreenPosition)
    {
        Vector3 mouseWorldPosition = SelectionWorldPosition(mouseScreenPosition);
        mouseWorldPosition.z = 0; // Ensure Z axis is zero for 2D positioning
        particleCollector.transform.position = mouseWorldPosition;

        Vector3Int cellPosition = grid.WorldToCell(mouseWorldPosition);
        if (IsMouseOverTile(cellPosition))
            OnSelectionOfTile(cellPosition);
        else
            OnDeselectionOfTile();
    }
    /// <summary>
    /// Returns the world position of a valid tile under the given screen position, or null if invalid.
    /// </summary>
    /// <param name="screenPosition">Mouse screen position.</param>
    /// <returns>Nullable Vector3 world position of a valid tile, or null.</returns>
    public Vector3? GetValidTile(Vector2 mouseScreenPosition)
    {
        Vector3Int cellPosition = grid.WorldToCell(SelectionWorldPosition(mouseScreenPosition));

        if (!tilemap.HasTile(cellPosition)) return null;
        return grid.GetCellCenterWorld(cellPosition);
    }

    /// <summary>
    /// Attempts to place a tile at the given position.
    /// </summary>
    /// <param name="position">The screen position where the tile should be placed.</param>
    public void PlaceTile(Vector2 position)
    {
        Vector3Int cellPosition = grid.WorldToCell(SelectionWorldPosition(position));
        GameControls.MoveReadValue = position;
        if (CubedPhiUtils.IsPointerOverUI(position, sellMenuRaycaster, EventSystem.current))
            return;
        if (GameManager.Instance.buildingLocations.TryGetValue(cellPosition, out var isOccupied) && isOccupied.Item1)
        {
            if (sellMenu.activeSelf) return;
            Vector3 aboveCellOffset = new Vector3(0, grid.cellSize.y + 1.5f, 0); // Adjusted offset to align 1.5 units above the tower
            Vector3 pos = grid.GetCellCenterWorld(cellPosition) + aboveCellOffset;
            pos.z = 0;
            sellMenu.transform.position = pos;
            sellMenu.SetActive(true);
            _sellCellPosition = cellPosition;
            Debug.Log($"[SelectorHandler] Attempting to sell tower at {cellPosition}");
            return;
        }
        sellMenu.SetActive(false);

        if (GameManager.Instance.selectedCard == null) return;


        if (!tilemap.HasTile(cellPosition)) return; // Ensure the position is within the tilemap grid

        if (GameManager.Instance.BitsCollected < GameManager.Instance.selectedCard.stats.cardCost) return;

        GameManager.RaiseBitChange(-GameManager.Instance.selectedCard.stats.cardCost);
        Vector3 spawnPosition = grid.GetCellCenterWorld(cellPosition);


        // Build
        if (GameManager.Instance.selectedCard.stats.cardObject != null)
        {
            Debug.Log($"[SelectorHandler] Placing {GameManager.Instance.selectedCard.stats.cardObject.name} at {spawnPosition}");
            GameObject baseObject = PoolManager.Instance
            .GetObject(GameManager.Instance.selectedCard.stats.cardObject, spawnPosition, Quaternion.identity);

            Debug.Log($"[SelectorHandler] Placing {GameManager.Instance.selectedCard.stats.cardObject.name} at {spawnPosition}");
            GameManager.levelStats.NumberOfTowersCreated++;
            GameManager.Instance.buildingLocations.Add(grid.WorldToCell(spawnPosition), new Tuple<bool, GameObject>(true, baseObject));

            if (TutorialMananger.Instance != null) TutorialMananger.TriggerTowerPlaced();
        }

        GameManager.Instance.CardDeck.RemoveCardFromDeck(GameManager.Instance.selectedCard.gameObject);
    }

    public void sellTower()
    {
        if (!GameControls.MoveReadValue.HasValue)
        {
            Debug.LogWarning("[SellTower] MoveReadValue is null.");
            return;
        }
        Vector2 mouseScreenPos = GameControls.MoveReadValue.Value;
        Vector3 mouseWorldPos = _mainCamera.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, -_mainCamera.transform.position.z));
        mouseWorldPos.z = 0f;

        Vector3Int cell = _sellCellPosition;

        if (!GameManager.Instance.buildingLocations.TryGetValue(cell, out var data) || !data.Item1 || data.Item2 == null)
        {
            Debug.LogWarning($"[SellTower] No valid tower found at cell {cell}");
            return;
        }

        BuildableUnit unit = data.Item2.GetComponentInChildren<BuildableUnit>();
        if (unit == null)
        {
            Debug.LogError($"[SellTower] No BuildableUnit component found in object {data.Item2.name}.");
            return;
        }

        int sellAmount = unit.Sell();
        if (sellAmount <= 0)
        {
            Debug.LogWarning($"[SellTower] Sell value is zero or less for unit {unit.name}");
            return;
        }

        GameObject particle = PoolManager.Instance.GetObject(bitsParticleSystem, grid.GetCellCenterWorld(cell), Quaternion.identity);
        particle.GetComponent<BitsController>()?.StartBits(sellAmount);

        sellMenu.SetActive(false);
    }


    //  ------------------ Private ------------------
    private Camera _mainCamera;
    private Vector3Int _sellCellPosition;
    /// <summary>
    /// Initializes the main camera reference and hides the selection sprite initially.
    /// </summary>
    private void Awake()
    {
        _mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        selectionSprite.SetActive(false);
    }

    /// <summary>
    /// Checks if the mouse is currently hovering over a tile.
    /// </summary>
    /// <param name="cellPosition">The grid cell position.</param>
    /// <returns>True if the tile exists, otherwise false.</returns>
    private bool IsMouseOverTile(Vector3Int cellPosition) => tilemap.HasTile(cellPosition);

    /// <summary>
    /// Handles selection of a tile by enabling the selection sprite and updating its position.
    /// </summary>
    /// <param name="cellPosition">The grid cell position where the selection occurred.</param>
    private void OnSelectionOfTile(Vector3Int cellPosition)
    {
        if (!tilemap.HasTile(cellPosition)) return; // Ensure selection only occurs on the tilemap

        selectionSprite.SetActive(true);
        transform.position = grid.GetCellCenterWorld(cellPosition);
        spriteRenderer.color = GameManager.Instance.buildingLocations.TryGetValue(cellPosition, out var isOccupied) && isOccupied.Item1 ? Color.red : Color.green;
    }

    /// <summary>
    /// Handles deselection of a tile by hiding the selection sprite and resetting its color.
    /// </summary>
    private void OnDeselectionOfTile()
    {
        selectionSprite.SetActive(false);
        spriteRenderer.color = Color.white;
    }

    /// <summary>
    /// Converts screen coordinates to world position.
    /// </summary>
    /// <param name="selectionScreenPos">The screen position to convert.</param>
    /// <returns>The world position corresponding to the screen coordinates.</returns>
    private Vector3 SelectionWorldPosition(Vector2 selectionScreenPos) =>
        _mainCamera.ScreenToWorldPoint(new Vector3(selectionScreenPos.x, selectionScreenPos.y, 0));
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) => _mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;
}
