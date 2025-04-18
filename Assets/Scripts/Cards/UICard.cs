using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI; // New Input System

/// <summary>
/// Controls the flipping animation of a UI card.
/// Uses an Animator for smooth 360-degree flips.
/// Supports mouse selection with the New Input System.
/// </summary>
public class UICard : MonoBehaviour
{
    //  ------------------ Public ------------------

    [Header("References")]
    [Tooltip("Reference to the button component.")]
    public Button button;

    [Tooltip("Reference to the image component.")]
    public Image image;

    [Header("Card Images")]
    [Tooltip("The front image of the card.")]
    public GameObject frontImage;

    [Tooltip("The back image of the card.")]
    public GameObject backImage;

    [Tooltip("Sprite for when the card is non-interactable.")]
    public Sprite nonInteractable;

    [Tooltip("Sprite for when the card is interactable.")]
    public Sprite interactable;

    [Header("Animation")]
    [Tooltip("Animator controlling the flip animation.")]
    public Animator animator;
    public AudioSource AudioSrc;

    //  ------------------ Private ------------------

    private bool _isFlipped;

    /// <summary>
    /// Triggers the flip animation.
    /// </summary>
    public void FlipCard()
    {
        button.interactable = false;
        animator.Play("FlipCard");
        AudioSrc.Play();
    }

    /// <summary>
    /// Triggers the unflip animation.
    /// </summary>
    public void UnFlipCard()
    {
        if (!_isFlipped) return;
        animator.Play("UnFlipCard");
        AudioSrc.Play();
    }

    /// <summary>
    /// Toggles visibility of the card images at 90 degrees.
    /// Called via an animation event.
    /// </summary>
    public void ToggleVisibility()
    {
        if (_isFlipped)
        {
            Card card = GetComponent<Card>();
            if (card.cardUsed)
            {
                button.interactable = false;
                image.sprite = nonInteractable;
            }
            else
            {
                button.interactable = true;
                image.sprite = interactable;
            }
            transform.rotation = Quaternion.identity;
        }

        _isFlipped = !_isFlipped;
        frontImage.SetActive(_isFlipped);
        backImage.SetActive(!_isFlipped);
    }
}
