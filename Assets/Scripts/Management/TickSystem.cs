using System.Collections;
using UnityEngine;

/// <summary>
/// Singleton that manages tick speed for optimizations.
/// </summary>
/// <remarks>
/// Back ported from Cubed Arcade.
/// </remarks>
public class TickSystem : SingletonBase<TickSystem>
{
    //  ------------------ Public ------------------
    /// <summary>
    /// Delegate for tick events.
    /// </summary>
    public delegate void OnTick();

    /// <summary>
    /// Event triggered every tick.
    /// </summary>
    public static event OnTick OnTickAction;

    //  ------------------ Private ------------------
    /// <summary>
    /// Maximum tick duration, allowing for 1/10 of a second per tick.
    /// </summary>
    private const float _MAX_TICK = 0.1f;

    /// <summary>
    /// Coroutine that handles the tick timer.
    /// </summary>
    /// <returns>Coroutine enumerator.</returns>
    private IEnumerator TickTimer()
    {
        // Keeps the coroutine running indefinitely.
        while (true)
        {
            yield return new WaitForSeconds(_MAX_TICK);
            OnTickAction?.Invoke();
        }
    }

    /// <summary>
    /// Called after the singleton instance is initialized.
    /// </summary>
    public override void PostAwake() => StartCoroutine(TickTimer());
}
