using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject instructionsCanvas;
    public SettingsManager settingsManager;

    [Header("Platform-Specific UI")]
    [Tooltip("Quit button, only shown on PC builds.")]
    public GameObject QuitButton;
    public void OnStartSelected() => StartCoroutine(LoadGameSceneAsync());
    public void OnInstructionsSelected()
    {
        if (settingsManager.isSettingsVisible) settingsManager.SettingsToggle();
        instructionsCanvas.SetActive(!instructionsCanvas.activeSelf);
    }

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

    /// <summary>
    /// Shows the quit button only if running on a standalone (PC) build.
    /// </summary>
    public void ShowQuitButtonIfPC()
    {
#if UNITY_STANDALONE
        QuitButton?.gameObject.SetActive(true);
#else
    QuitButton?.gameObject.SetActive(false);
#endif
    }


    private void Start() => ShowQuitButtonIfPC();

    /// <summary>
    /// Quits the application. Has no effect in the editor.
    /// </summary>
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }


}
