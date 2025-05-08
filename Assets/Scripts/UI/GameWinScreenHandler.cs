using TMPro;
using UnityEngine;

public class GameWinScreenHandler : MonoBehaviour
{
    public TextMeshProUGUI gameWinText;
    private void OnEnable() => gameWinText.text = 
        $"Total Enemies Defeated: {GameManager.levelStats.numberOfEnemiesKilled}\n" +
        $"Total Bits Collected: {GameManager.levelStats.NumberOfBitsCollected}\n" +
        $"Total Towers Built: {GameManager.levelStats.NumberOfTowersCreated}";
}