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
    [Header("UI Elements")]
    [Tooltip("Text UI element that displays the bit count.")]
    public TextMeshProUGUI bitCount;

    /// <summary>
    /// The total bits collected.
    /// </summary>
    public long BitsCollected = 0;

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
    public override void PostAwake() { }

    /// <summary>
    /// Raises the OnBitChange event with the specified delta.
    /// </summary>
    /// <param name="bitDelta">The change in bits (positive or negative).</param>
    public static void RaiseBitChange(long bitDelta) => OnBitChange?.Invoke(bitDelta);

    //  ------------------ Private ------------------
    /// <summary>
    /// Handles bit change events by updating the total bits and the UI text.
    /// </summary>
    /// <param name="bitDelta">The change in bits (positive or negative).</param>
    private void HandleBitChange(long bitDelta) =>
        bitCount.text = $"Bit count: {BitsCollected += bitDelta}";

    /// <summary>
    /// Subscribes to the bit change event.
    /// </summary>
    private void OnEnable() => OnBitChange += HandleBitChange;

    /// <summary>
    /// Unsubscribes from the bit change event.
    /// </summary>
    private void OnDisable() => OnBitChange -= HandleBitChange;
}
