using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject instructionsCanvas;
    public void OnStartSelected() => StartCoroutine(LoadGameSceneAsync());
    public void OnInstructionsSelected() => instructionsCanvas.SetActive(!instructionsCanvas.activeSelf);

    private IEnumerator LoadGameSceneAsync()
    {
        // Check if the scene exists in Build Settings before loading
        if (!Application.CanStreamedLevelBeLoaded("GameScene"))
        {
            Debug.LogError("Scene 'GameScene' not found! Ensure it is added to Build Settings.");
            yield break;
        }

        AsyncOperation operation = SceneManager.LoadSceneAsync("GameScene");

        // Prevents the scene from activating immediately
        operation.allowSceneActivation = false;

        // Wait until the scene is mostly loaded
        while (operation.progress < 0.9f)
        {
            yield return null;
        }

        // Wait for player input or auto-activate after a delay
        yield return new WaitForSeconds(0.1f); // Optional delay
        operation.allowSceneActivation = true;
    }
}
