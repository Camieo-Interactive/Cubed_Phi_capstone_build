using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handles mouse controls for placing a sprite on a grid.
/// </summary>
/// <remarks>
/// Assumes all references are set in the Inspector.
/// </remarks>
public class MouseControls : MonoBehaviour
{
    //  ------------------ Public ------------------
    [Tooltip("Reference to the Grid used for placing the sprite. It should be part of a Grid layout.")]
    public Grid grid;
    
    //  ------------------ Private ------------------
    /// <summary>
    /// Updates the mouse position and moves the sprite to the corresponding grid cell.
    /// </summary>
    private void Update()
    {
        Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(
            mouseScreenPosition.x, 
            mouseScreenPosition.y, 
            Camera.main.nearClipPlane));
        mouseWorldPosition.z = 0f;
        Vector3Int cellPosition = grid.WorldToCell(mouseWorldPosition);
        transform.position = grid.GetCellCenterWorld(cellPosition);
    }
}
