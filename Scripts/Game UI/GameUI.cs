using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameUI : MonoBehaviour {

    //Menu buttons
    public List<Transform> buttons;

    public Transform objectivesPanel;

    public Transform optionsMenu;
    public Transform encyclopediaPanel;
    Transform activeMenu = null;
    public Map map;
    public Image pauseImage;

    bool gamePaused = false;

    Dictionary<string, KeyCode> hotkeys = new Dictionary<string, KeyCode>(){
        {"Escape", KeyCode.Escape },
        {"b", KeyCode.B }
    };

    // Use this for initialization
	void Start () {

    }
	
	// Key inputs
	void Update () {
        if (Input.anyKeyDown) {
            if (activeMenu == encyclopediaPanel) {
                if (Input.GetKeyDown(KeyCode.Escape)) {
                    ToggleEncyclopedia();
                }
                else if (Input.GetKeyDown(KeyCode.Tab)) {
                    encyclopediaPanel.Find("Unit Selection Panel/Search Panel/Search Bar").GetComponent<Selectable>().Select();
                }
            }
            else if (activeMenu != encyclopediaPanel) { 
                if (Input.GetKeyDown(KeyCode.Escape)) {
                    ToggleOptionsMenu();
                }
                else if (Input.GetKeyDown(KeyCode.Z)) {
                    ToggleObjectives();
                }
                else if (Input.GetKeyDown(KeyCode.Q)) {
                    ToggleEncyclopedia();
                }
            }
        }
	}

    //Opens and closes the objectives
    public void ToggleObjectives() {
        if (objectivesPanel.gameObject.activeSelf) 
            objectivesPanel.gameObject.SetActive(false);
        else 
            objectivesPanel.gameObject.SetActive(true);
    }

    //Opens and closes the options menu
    public void ToggleOptionsMenu() {
        if (optionsMenu.gameObject.activeSelf) {
            optionsMenu.gameObject.SetActive(false);
            UnpauseGame();
            SetActiveMenu(null);
        }
        else {
            optionsMenu.gameObject.SetActive(true);
            PauseGame();
            SetActiveMenu(optionsMenu);
        }
    }

    //Opens and closes the encyclopedia
    public void ToggleEncyclopedia() {
        if (encyclopediaPanel.gameObject.activeSelf) {
            encyclopediaPanel.gameObject.SetActive(false);
            UnpauseGame();
            SetActiveMenu(null);
            EventSystem.current.SetSelectedGameObject(null);
        }
        else {
            encyclopediaPanel.gameObject.SetActive(true);
            PauseGame();
            SetActiveMenu(encyclopediaPanel);
        }
    }

    //Sets the current open menu
    public void SetActiveMenu(Transform activeMenu) {
        this.activeMenu = activeMenu;
    }

    //Pauses the game
    void PauseGame() {
        gamePaused = true;
        map.SetGamePaused(true);
        pauseImage.gameObject.SetActive(true);
        foreach (Transform button in buttons)
            button.GetComponent<Button>().interactable = false;
    }

    //Unpauses the game
    void UnpauseGame() {
        gamePaused = false;
        map.SetGamePaused(false);
        pauseImage.gameObject.SetActive(false);
        foreach (Transform button in buttons)
            button.GetComponent<Button>().interactable = true;
    }

    void UpdateResourcePanel() {

    }
}
