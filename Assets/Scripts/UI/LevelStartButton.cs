using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelStartButton : MonoBehaviour {
    public string sceneName; 
    public void startLevel() => StartCoroutine(_loadGameSceneAsync());

    private IEnumerator _loadGameSceneAsync()
    {
        // Check if the scene exists in Build Settings before loading
        if (!Application.CanStreamedLevelBeLoaded(sceneName))
        {
            Debug.LogError($"Scene {sceneName} not found! Ensure it is added to Build Settings.");
            yield break;
        }

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        // Prevents the scene from activating immediately
        operation.allowSceneActivation = false;

        // Wait until the scene is mostly loaded
        while (operation.progress < 0.9f) yield return null;
    
        // Wait for player input or auto-activate after a delay
        yield return new WaitForSeconds(0.1f); // Optional delay
        operation.allowSceneActivation = true;
    }
}