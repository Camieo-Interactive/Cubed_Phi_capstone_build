using System.Collections;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A monoBehaviour component that manages the health bar
/// </summary>
/// <remarks>
/// A part of the health component structure
/// </remarks>

public class HealthBarComponent : MonoBehaviour
{

    //  ------------------ Public ------------------

    [Tooltip("Image of the health bar")]
    // Image of the health bar
    public Image healthBar;

    [Tooltip("The health bar text to update")]
    // The GUI to subscribe to
    public TextMeshProUGUI healthText;


    /// <summary>
    /// Function that sets the intial private values of health
    /// </summary>
    /// <param name="initalHealth">
    /// The outside value of the inital health
    /// </param>
    public void setInitalHealth(int initalHealth)
    {
        _initalhealth = initalHealth;
        setHealth(_initalhealth);
    }

    /// <summary>
    /// Function that sets the health of the UI bar
    /// </summary>
    /// <param name="currentHealth">
    /// current health of the health bar component
    /// </param>
    public void setHealth(int healthDelta, DamageStatus currentStatus = DamageStatus.NONE)
    {
        _currentHealth += healthDelta;
        _currentHealth = math.max(0, _currentHealth);
        if (healthText != null)
        {
            healthText.text = $"Health: {_currentHealth}/{_initalhealth}" + currentStatus switch
            {
                DamageStatus.STUN => "<sprite name=\"STUNSTATUS\">",
                DamageStatus.POISON => "<sprite name=\"POISIONSTATUS\">",
                DamageStatus.FIRE => "<sprite name=\"FIRESTATUS\">",
                DamageStatus.SLOW => "<sprite name=\"SLOWSTATUS\">",
                _ => ""
            };
        }
        if (healthBar != null) healthBar.fillAmount = _currentHealth / (float)_initalhealth;
    }

    //  ----------------- Private ------------------

    // Private counter for the health
    private int _initalhealth = 0;
    private int _currentHealth = 0;

}