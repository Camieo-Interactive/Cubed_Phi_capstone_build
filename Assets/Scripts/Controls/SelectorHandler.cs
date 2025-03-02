using UnityEngine;
using UnityEngine.InputSystem;
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

    [Tooltip("The selection sprite that appears when hovering over a tile.")]
    public GameObject selectionSprite;

    [Tooltip("The object that collects particle effects at the mouse position.")]
    public GameObject particleCollector;

    //  ------------------ Private ------------------
    private Camera _mainCamera;

    /// <summary>
    /// Updates the position and visibility of the selection sprite based on mouse position.
    /// </summary>
    /// <param name="mouseScreenPosition">The screen position of the mouse.</param>
    public void UpdateSelectionSprite(Vector2 mouseScreenPosition)
    {
        Vector3 mouseWorldPosition = _mainCamera.ScreenToWorldPoint(new Vector3(
            mouseScreenPosition.x,
            mouseScreenPosition.y,
            _mainCamera.nearClipPlane));

        mouseWorldPosition.z = 0f;
        particleCollector.transform.position = mouseWorldPosition;
        Vector3Int cellPosition = grid.WorldToCell(mouseWorldPosition);

        if (IsMouseOverTile(cellPosition))
        {
            selectionSprite.SetActive(true);
            transform.position = grid.GetCellCenterWorld(cellPosition);
        }
        else
        {
            selectionSprite.SetActive(false);
        }
    }

    /// <summary>
    /// Initializes the main camera reference.
    /// </summary>
    private void Awake()
    {
        _mainCamera = Camera.main;
        selectionSprite.SetActive(false); // Ensure it's initially hidden
    }

    /// <summary>
    /// Checks if the mouse is currently hovering over a tile.
    /// </summary>
    /// <param name="cellPosition">The grid cell position.</param>
    /// <returns>True if the tile exists, otherwise false.</returns>
    private bool IsMouseOverTile(Vector3Int cellPosition) => tilemap.HasTile(cellPosition);
}
