using System.Collections;
using UnityEngine;

/// <summary>
/// Handles flashing a SpriteRenderer with a temporary material and color.
/// </summary>
public class FlashComponent : MonoBehaviour
{
    //  ------------------ Public ------------------

    [Tooltip("Material to switch to during the flash.")]
    public Material flashMaterial;

    [Tooltip("SpriteRenderer that will flash.")]
    public SpriteRenderer spriteRenderer;

    /// <summary>
    /// Starts a flash effect using the given color and original color.
    /// </summary>
    /// <param name="color">Color to flash to. Defaults to white.</param>
    /// <param name="originalColor">Color to revert to after each flash.</param>
    /// <param name="duration">Total duration of the flash sequence.</param>
    /// <param name="flashes">Number of times to flash.</param>
    public void Flash(Color color = default, Color originalColor = default, float duration = 0.25f, int flashes = 4)
    {
        _originalColor = originalColor == default ? spriteRenderer.color : originalColor;
        Flash(color == default ? Color.white : color, duration, flashes);
    }

    /// <summary>
    /// Starts a flash effect using the given color and internally stored original color.
    /// </summary>
    /// <param name="color">Color to flash to. Defaults to white.</param>
    /// <param name="duration">Total duration of the flash sequence.</param>
    /// <param name="flashes">Number of times to flash.</param>
    public void Flash(Color color = default, float duration = 0.25f, int flashes = 4)
    {
        if (_flashRoutine != null)
        {
            StopCoroutine(_flashRoutine);
        }

        _flashRoutine = StartCoroutine(FlashRoutine(color == default ? Color.white : color, duration, flashes));
    }

    //  ------------------ Private ------------------

    private Material _originalMaterial;
    private Coroutine _flashRoutine;
    private Color _originalColor;

    private void Start()
    {
        spriteRenderer ??= GetComponent<SpriteRenderer>();
        _originalMaterial = spriteRenderer.material;
        flashMaterial = new Material(flashMaterial);
        _originalColor = spriteRenderer.color;
    }

    private IEnumerator FlashRoutine(Color color, float duration, int flashes)
    {
        float timePerFlash = duration / flashes;

        for (int i = 0; i < flashes; i++)
        {
            // Flash on
            spriteRenderer.material = flashMaterial;
            spriteRenderer.color = color;
            yield return new WaitForSeconds(timePerFlash);

            // Flash off
            spriteRenderer.material = _originalMaterial;
            spriteRenderer.color = _originalColor;
            yield return new WaitForSeconds(timePerFlash);
        }

        // Reset state
        spriteRenderer.material = _originalMaterial;
        spriteRenderer.color = _originalColor;
        _flashRoutine = null;
    }
}
