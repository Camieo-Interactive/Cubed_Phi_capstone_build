using UnityEngine;
using TMPro; 
public class LevelSelectHandler : MonoBehaviour {
    public TextMeshProUGUI CycleName; 
    public TextMeshProUGUI LevelDetails; 
    public GameObject LevelGrid; 
    public GameObject StartLevelButton;
    public LevelStartButton levelStartButton;
    public GameObject NoSelectionCanvas; 
    public GameObject LevelButtonPrefab;
    public GameObject LevelSelectionCanvas;
    public void HandleCardSelect(CycleLevelStats levelStats) {
        NoSelectionCanvas.SetActive(false);
        StartLevelButton.SetActive(false);
        LevelSelectionCanvas.SetActive(true);

        if(levelStats == null) {
            Debug.Log("No levelStats Passed in");
            return;
        }
        CycleName.text = levelStats.cycleName;
        LevelDetails.text = "";
        _createLevelButtons(levelStats);

            
    }

    public void DisplayDetails(CycleLevelStats desc) {
        // TODO: 
        Debug.Log("Card Selected");
        HandleCardSelect(desc);
    }

    public void DisplayDetails(LevelDescription desc) {
        // TODO: 
        Debug.Log("Level selected");
        CycleName.text = desc.levelName;
        LevelDetails.text = desc.Description;
        StartLevelButton.SetActive(true);
        levelStartButton.sceneName = desc.SceneName;

    }

    private void _createLevelButtons(CycleLevelStats levelStats) {
        foreach (Transform child in LevelGrid.transform) Destroy(child.gameObject);

        // Creates buttons..
        foreach(var lvl in levelStats.levels) {
            var obj = Instantiate(LevelButtonPrefab, LevelGrid.transform);
            obj.GetComponent<LevelSelectButtons>().Init(this, lvl);
        }   
    }
}