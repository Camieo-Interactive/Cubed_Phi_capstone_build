using TMPro;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// Handles the display of damage numbers with appropriate animations and color coding based on damage and critical hit tier.
/// </summary>
public class DamageNumberHandler : MonoBehaviour
{
    //  ------------------ Public Variables ------------------

    /// <summary>
    /// The TextMeshProUGUI component that will display the damage number.
    /// </summary>
    [Tooltip("The TextMeshProUGUI component that will display the damage number.")]
    public TextMeshProUGUI textMesh;

    /// <summary>
    /// The animator used to play damage number animations.
    /// </summary>
    [Tooltip("The animator used to play damage number animations.")]
    public Animator animator;

    //  ------------------ Public Methods ------------------

    /// <summary>
    /// Initializes the damage number display based on the damage value and critical hit tier.
    /// </summary>
    /// <param name="damageValue">The damage value to be displayed, including the damage amount and critical hit tier.</param>
    public void Init(DamageValue damageValue)
    {
        // Display the absolute value of damage (so no negative numbers appear).
        string damage = math.abs(damageValue.damage).ToString();
        string status = damageValue.damageStatus switch
        {
            DamageStatus.STUN => "<sprite name=\"STUNSTATUS\">",
            DamageStatus.POISON => "<sprite name=\"POISIONSTATUS\">",
            DamageStatus.FIRE => "<sprite name=\"FIRESTATUS\">",
            DamageStatus.SLOW => "<sprite name=\"SLOWSTATUS\">",
            _ => ""
        };
        textMesh.text = $"{damage}{status}";
        // Set the text color based on the critical hit tier.
        textMesh.color = Color.white;
        animator.Play("DamagePopupAnime");
    }
}