using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialMananger : SingletonBase<TutorialMananger>
{
    public EnemyManager enemyManager;
    public List<GameObject> tutorialSteps;
    public static event Action OnTutorialButtonPressed;
    public static event Action OnTowerPlaced;
    public override void PostAwake() { }
    public void RestartEnemyManager() => enemyManager.enabled = true;
    public static void TriggerTutorialButton() => OnTutorialButtonPressed?.Invoke();
    public static void TriggerTowerPlaced() => OnTowerPlaced?.Invoke();
    public static void TriggerButtonAfterTowerPlaced() => Instance.AdvanceIfTowerPlaced();
    public void AdvanceIfTowerPlaced()
    {
        if (_towerWasPlaced) _AdvanceTutorialStep();
    }

    private bool _towerWasPlaced = false;
    private int _actionCounter = 0;

    private void _OnTutorialButtonPressed() => _AdvanceTutorialStep();

private void _AdvanceTutorialStep()
{
    // If we've reached the end, enable enemyManager and return
    if (_actionCounter >= tutorialSteps.Count)
    {
        enemyManager.enabled = true;

        // Hide the last tutorial step if it's still active
        if (tutorialSteps.Count > 0)
            tutorialSteps[tutorialSteps.Count - 1].SetActive(false);

        return;
    }

    // Hide the previous step (if any)
    if (_actionCounter > 0) tutorialSteps[_actionCounter - 1].SetActive(false);

    // Show the current step
    if (tutorialSteps[_actionCounter] != null) tutorialSteps[_actionCounter].SetActive(true);

    _actionCounter++;
}


    // Toggle the boolean.. 
    private void _OnTowerPlaced()
    {
        _towerWasPlaced = true;
        _AdvanceTutorialStep();
    }

    private void _OnSceneLoad(Scene sceneArgs, LoadSceneMode loadSceneMode)
    {
        // Make sure its off.. 
        enemyManager.enabled = false;

        // Reset state when scene loads (optional)
        _actionCounter = 0;
        _towerWasPlaced = false;

        // Disable all tutorial steps on scene load
        foreach (var step in tutorialSteps)
        {
            if (step != null)
                step.SetActive(false);
        }
        tutorialSteps[0].SetActive(true);
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += _OnSceneLoad;
        OnTutorialButtonPressed += _OnTutorialButtonPressed;
        OnTowerPlaced += _OnTowerPlaced;
        
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= _OnSceneLoad;
        OnTutorialButtonPressed -= _OnTutorialButtonPressed;
        OnTowerPlaced -= _OnTowerPlaced;
    }



}
