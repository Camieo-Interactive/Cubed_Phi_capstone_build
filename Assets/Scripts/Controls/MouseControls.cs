using UnityEngine;
using UnityEngine.InputSystem;

public class MouseControls : MonoBehaviour
{
    [Tooltip("Reference to the Grid used for placing the sprite. It should be part of a Grid layout.")]
    public Grid grid;
    void Update()
    {
        Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, Camera.main.nearClipPlane));
        mouseWorldPosition.z = 0f;
        Vector3Int cellPosition = grid.WorldToCell(mouseWorldPosition);
        transform.position = grid.GetCellCenterWorld(cellPosition);
    }
}
