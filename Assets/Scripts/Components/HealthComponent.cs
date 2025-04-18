using System.Collections;
using UnityEngine;

/// <summary>
/// Manages the health of an entity, including damage handling and status effects.
/// </summary>
public class HealthComponent : MonoBehaviour
{
    //  ------------------ Public ------------------
    [Header("Health Settings")]
    [Tooltip("Current damage status of the entity.")]
    public DamageStatus currentStatus = DamageStatus.NONE;
    [Tooltip("Blood particle effect to spawn when taking damage.")]
    public GameObject bloodFX;
    [Tooltip("The Flash component of the entity.")]
    public FlashComponent flashComponent;
    [Tooltip("The Health bar component of the entity.")]
    // The component to subscribe to
    public HealthBarComponent healthBarComponent;


    /// <summary>
    /// Delegate triggered when health initializes.
    /// </summary>
    /// <param name="defaultHealth">The starting health of the entity.</param>
    public delegate void InitHealth(int defaultHealth);

    /// <summary>
    /// Delegate triggered when health changes.
    /// </summary>
    /// <param name="currentHealth">The updated health value.</param>
    public delegate void HealthChanged(int currentHealth);

    /// <summary>
    /// Delegate triggered when health reaches zero.
    /// </summary>
    public delegate void Death();

    /// <summary>
    /// Event invoked when health reaches zero.
    /// </summary>
    public event Death OnDeath;
    public event HealthChanged OnHealthChanged;

    /// <summary>
    /// Initializes the default health of the entity.
    /// </summary>
    /// <param name="defaultHealth">The starting health of the entity.</param>
    public void InitializeHealth(int defaultHealth)
    {
        _maxHealth = defaultHealth;
        _currentHealth = defaultHealth;
        OnHealthChanged?.Invoke(_currentHealth);
        if (flashComponent != null) flashComponent.IntitalizeFlash();
        if (healthBarComponent != null) healthBarComponent.setInitalHealth(defaultHealth);
    }


    /// <summary>
    /// Modifies the entity's health based on the damage value received.
    /// </summary>
    /// <param name="value">The damage value, which includes the amount and status effect.</param>
    public void ChangeHealth(DamageValue value)
    {
        OnHealthChanged?.Invoke(_currentHealth);
        currentStatus = value.damageStatus;
        if (flashComponent != null) flashComponent.Flash(Color.white, 0.25f, 4);
        if (value.damageStatus != DamageStatus.NONE)
        {
            if (value.damageStatus == DamageStatus.POISON || value.damageStatus == DamageStatus.FIRE)
                StartCoroutine(DecayHealth(value, value.statusDuration));
            if (currentStatus != DamageStatus.NONE)
                StartCoroutine(WaitForStatus(value.statusDuration));
        }
        if (healthBarComponent != null) healthBarComponent.setHealth(value.damage, currentStatus);
        _currentHealth += value.damage;
        if (_currentHealth <= 0)
            OnDeath?.Invoke();
    }

    /// <summary>
    /// Modifies the entity's health and spawns a blood effect at the specified position.
    /// </summary>
    /// <param name="value">The damage value, which includes the amount and status effect.</param>
    /// <param name="pos">The position where the blood effect should appear.</param>
    /// <param name="quaternion">The rotation of the blood effect.</param>
    public void ChangeHealth(DamageValue value, Vector3 pos, Quaternion quaternion)
    {
        if (bloodFX != null) PoolManager.Instance.GetObject(bloodFX, pos, quaternion);
        if (flashComponent != null) flashComponent.Flash(Color.white, default, default);

        ChangeHealth(value);
    }

    /// <summary>
    /// Returns the current health as a normalized percent (0 to 1).
    /// </summary>
    public float CurrentPercent
    {
        get
        {
            if (_maxHealth <= 0)
            {
                Debug.LogWarning("CurrentPercent accessed before health was initialized!");
                return 0f;
            }
            return Mathf.Min((float)_currentHealth / _maxHealth , 1);
        }
    }

    //  ------------------ Private ------------------
    private int _currentHealth = 0;
    private int _maxHealth = 100;

    /// <summary>
    /// Waits for the duration of a status effect before resetting the status.
    /// </summary>
    /// <param name="duration">The duration of the status effect.</param>
    private IEnumerator WaitForStatus(float duration)
    {
        yield return new WaitForSeconds(duration);
        currentStatus = DamageStatus.NONE;
    }

    private IEnumerator DecayHealth(DamageValue damageValue, float duration)
    {
        float timeElapsed = 0f;
        damageValue.damageStatus = DamageStatus.NONE;
        while (timeElapsed < duration)
        {
            ChangeHealth(damageValue);
            yield return new WaitForSeconds(1f);
            timeElapsed += 1f;
        }
    }
}
