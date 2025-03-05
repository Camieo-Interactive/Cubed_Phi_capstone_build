using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

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
    /// Attempts to place a tile at the given position.
    /// </summary>
    /// <param name="position">The screen position where the tile should be placed.</param>
    public void PlaceTile(Vector2 position)
    {

        if (GameManager.Instance.selectedCard == null) return;

        Vector3Int cellPosition = grid.WorldToCell(SelectionWorldPosition(position));
        if (!tilemap.HasTile(cellPosition)) return; // Ensure the position is within the tilemap grid
        if (GameManager.Instance.BitsCollected < GameManager.Instance.selectedCard.stats.cardCost) return;
        if (GameManager.Instance.buildingLocations.TryGetValue(cellPosition, out bool isOccupied) && isOccupied) return;
        GameManager.RaiseBitChange(-GameManager.Instance.selectedCard.stats.cardCost);
        Vector3 spawnPosition = grid.GetCellCenterWorld(cellPosition);
        PoolManager.Instance.GetObject(GameManager.Instance.selectedCard.stats.cardObject, spawnPosition, Quaternion.identity);

        GameManager.Instance.CardDeck.RemoveCardFromDeck(GameManager.Instance.selectedCard.gameObject);
    }

    //  ------------------ Private ------------------
    private Camera _mainCamera;
    private CellStatus _selectionStatus;

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
        spriteRenderer.color = GameManager.Instance.buildingLocations.TryGetValue(cellPosition, out bool isOccupied) && isOccupied ? Color.red : Color.green;
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
