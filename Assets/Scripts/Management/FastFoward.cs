using UnityEngine;
using UnityEngine.UI;

public class FastFoward : MonoBehaviour
{
    public bool isFastFoward = false;
    public void ToggleFastFoward()
    {
        isFastFoward = !isFastFoward;
        if (isFastFoward)
        {
            Time.timeScale = 2;
            image.color = Color.red;
        }
        else
        {
            Time.timeScale = 1;
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
