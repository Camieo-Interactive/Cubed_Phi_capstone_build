using UnityEngine;

/// <summary>
/// Updates a SpriteMask based on health percentage thresholds.
/// </summary>
/// <remarks>
/// Uses event-driven updates via HealthComponent.OnHealthChanged
/// </remarks>
public class SpriteDamageComponent : MonoBehaviour
{
    //  ------------------ Public ------------------

    [Tooltip("The SpriteMask used to display damage visuals.")]
    public SpriteMask targetMask;

    [Tooltip("The HealthComponent to listen to.")]
    public HealthComponent healthComponent;

    [Tooltip("Sprites that represent different levels of damage (from least to most severe).")]
    public Sprite[] damageStages;

    //  ------------------ Private ------------------

    private int _lastIndex = -1;

    private void OnEnable()
    {
        if (healthComponent != null)
            healthComponent.OnHealthChanged += HandleHealthChanged;
        targetMask.sprite = null;
    }

    private void OnDisable()
    {
        if (healthComponent != null)
            healthComponent.OnHealthChanged -= HandleHealthChanged;
        targetMask.sprite = null;
    }

    /// <summary>
    /// Responds to health changes and updates the mask.
    /// </summary>
    private void HandleHealthChanged(int currentHealth)
    {
        float percent = healthComponent.CurrentPercent;
        int spriteIndex = GetSpriteIndex(percent);

        if (spriteIndex != _lastIndex)
        {
            targetMask.sprite = spriteIndex >= 0 ? damageStages[spriteIndex] : null;
            _lastIndex = spriteIndex;
        }
    }

    /// <summary>
    /// Converts health percent into the appropriate sprite index.
    /// Sprites appear only below 90% health.
    /// </summary>
    private int GetSpriteIndex(float percent)
    {
        if (damageStages.Length == 0) 
            return -1;

        // Only show sprites starting at ≤ 90% health
        if (percent > 0.9f) {
            targetMask.sprite = null;
            return -1;
        }
        float range = 0.9f; // the 0.0–0.9 range is mapped to sprite stages
        float normalized = Mathf.Clamp01((0.9f - percent) / range);
        int index = Mathf.FloorToInt(normalized * damageStages.Length);
        return Mathf.Clamp(index, 0, damageStages.Length - 1);
    }
}
