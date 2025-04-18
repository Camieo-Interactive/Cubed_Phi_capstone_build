using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Manages game controls and input actions for selection handling.
/// </summary>
public class GameControls : MonoBehaviour
{
    //  ------------------ Public ------------------

    [Header("Selection Handler")]
    [Tooltip("Reference to the SelectorHandler responsible for updating the selection sprite.")]
    public SelectorHandler selectorHandler;
    public static Vector3? CurrentTileSelected = null;
    //  ------------------ Private ------------------

    private InputAction _move;
    private InputAction _select;
    private GameInputActions _controls;

    /// <summary>
    /// Initializes input actions.
    /// </summary>
    private void Awake()
    {
        _controls = new GameInputActions();
        _move = _controls.Game.Move;
        _select = _controls.Game.Select;
    }

    /// <summary>
    /// Enables input actions when the object is enabled.
    /// </summary>
    private void OnEnable()
    {
        _move.Enable();
        _select.Enable();
        _select.performed += _ => selectorHandler.PlaceTile(_move.ReadValue<Vector2>());
    }

    /// <summary>
    /// Disables input actions when the object is disabled.
    /// </summary>
    private void OnDisable()
    {
        _select.performed -= _ => selectorHandler.PlaceTile(_move.ReadValue<Vector2>());
        _move.Disable();
        _select.Disable();
    }

    /// <summary>
    /// Updates the selection sprite based on movement input.
    /// </summary>
    private void Update()
    {
        selectorHandler.UpdateSelectionSprite(_move.ReadValue<Vector2>());
        Vector3? vector = selectorHandler.GetValidTile(_move.ReadValue<Vector2>());
        CurrentTileSelected = (vector != null) ? vector : CurrentTileSelected;
    }
}
