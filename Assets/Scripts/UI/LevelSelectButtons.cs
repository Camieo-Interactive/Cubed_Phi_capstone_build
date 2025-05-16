using TMPro;
using UnityEngine;

/// <summary>
/// Represents a UI button in the level selection screen. This class handles
/// initialization with a level handler and level description, and processes
/// click events to display level details.
/// </summary>
public class LevelSelectButtons : MonoBehaviour
{
    /// <summary>
    /// A reference to the TextMeshProUGUI component used to display the level text
    /// on the level selection buttons in the UI.
    /// </summary>
    public TextMeshProUGUI levelText;
    /// <summary>
    /// Initializes the button with the specified level handler and level description.
    /// </summary>
    /// <param name="newHandler">The handler responsible for managing level selection.</param>
    /// <param name="desc">The description of the level associated with this button.</param>
    public virtual void Init(LevelSelectHandler newHandler, LevelDescription desc)
    {
        _handler = newHandler;
        _desc = desc;
        levelText.text = _desc.levelName;
    }

    /// <summary>
    /// Handles the click event for the button. Displays the details of the associated level
    /// using the level handler. Logs a warning if the handler is not set.
    /// </summary>
    public void OnClick()
    {
        if (_handler == null)
        {
            Debug.LogWarning("GPT GENERATE THIS");
            return;
        }

        _handler.DisplayDetails(_desc);
    }

    private LevelSelectHandler _handler;
    private LevelDescription _desc;
}