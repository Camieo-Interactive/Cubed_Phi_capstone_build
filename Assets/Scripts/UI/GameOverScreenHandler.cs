using TMPro;
using UnityEngine;

public class GameOverScreenHandler : MonoBehaviour
{
    public TextMeshProUGUI TipText;
    private void OnEnable() => TipText.text = TipsText.GetTip();
}
