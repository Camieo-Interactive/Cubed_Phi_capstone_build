using UnityEngine;
using TMPro;
using EasyTextEffects;
public class GetFlavorText : MonoBehaviour
{
    public TextMeshProUGUI textBox;
    void OnEnable()
    {
        string splash = SplashText.GetCurrentSplash();
        textBox.text = $"//: {splash}";
    }
}
