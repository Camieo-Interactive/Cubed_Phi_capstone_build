using UnityEngine;
using TMPro;
public class GetFlavorText : MonoBehaviour
{
    public TextMeshProUGUI textBox;
    void OnEnable()
    {
        string splash = SplashText.GetCurrentSplash();
        textBox.text = $"//: {splash}";
    }
}
