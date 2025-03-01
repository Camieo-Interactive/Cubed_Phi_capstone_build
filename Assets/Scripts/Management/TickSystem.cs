using System.Collections;
using UnityEngine;

/// <summary>
/// Singleton that manages tick speed for optimizations
/// </summary>
/// <remarks>
/// Back ported from cubed arcade.. 
/// </remarks>
public class TickSystem : SingletonBase<TickSystem>
{
    public delegate void onTick();
    // Event triggered every half second.
    public static event onTick OnTickAction;
    // Maximum tick duration, allowing for 1/10 of a second per tick.
    private const float _MAX_TICK = 0.1f;
    public IEnumerator tickTimer()
    {
        // Keeps the coroutine running indefinitely
        // Inital timer causes a stack overflow.. 
        while (true)
        {
            yield return new WaitForSeconds(_MAX_TICK);
            OnTickAction?.Invoke();
        }
    }

    public override void PostAwake() => StartCoroutine(tickTimer());
}