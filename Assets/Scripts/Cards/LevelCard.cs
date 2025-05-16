using UnityEngine;
using UnityEngine.UI;

public class LevelCard : Card
{
    public CycleLevelStats cycleLevels; 
    public LevelSelectHandler handler; 
    public override void Init() {}
    
    /// <summary>
    /// Selects this card.
    /// </summary>
    public override void CardPressed()
    {
        if (!cardUsed)
        {
            ClickAudioSource.Play();
            // GameManager.Instance.selectedCard = this;
            Debug.Log("Card Selected");
            handler.DisplayDetails(cycleLevels);
        }
    }

    /// <summary>
    /// Deselects this card.
    /// </summary>
    public override void CardDeselected() => Debug.Log("Card Deselected"); //GameManager.Instance.selectedCard = null;
}
