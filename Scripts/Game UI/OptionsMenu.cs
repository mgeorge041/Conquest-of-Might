using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsMenu : MonoBehaviour {

    public Transform settingsButton;
    public Transform mainMenuButton;
    public Transform returnToGameButton;

    public void LoadSettings() {

    }

    public void LoadMainMenu() {
        SceneManager.LoadScene(0);
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
