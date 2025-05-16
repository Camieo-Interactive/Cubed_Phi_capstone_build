using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FastFoward : MonoBehaviour
{
    public bool isFastFoward = false;
    public TextMeshProUGUI speedText;
    public void ToggleFastFoward()
    {
        isFastFoward = !isFastFoward;
        if (isFastFoward)
        {
            Time.timeScale = 2;
            speedText.text = "2x";
            image.color = Color.red;
        }
        else
        {
            Time.timeScale = 1;
            speedText.text = "1x";
            image.color = colorDefault;
        }
    }

    private Image image;
    private Color colorDefault;
    private void Start()
    {
        image = gameObject.GetComponent<Image>();
        colorDefault = image.color;
    }
}
