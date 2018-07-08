using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.SceneManagement;

public class GameSetup : MonoBehaviour {

    //Board size selection variables and dropdown
    public Transform boardSizeSelect = null;
    readonly Dictionary<int, string> boardSizeSelectOptions = new Dictionary<int, string>() {
        { 0, "Small" },
        { 1, "Medium" },
        { 2, "Large" }
    };
    readonly Dictionary<string, int> actualBoardSizeOptions = new Dictionary<string, int>() {
        {"Small", 10 },
        {"Medium", 15 },
        {"Large", 20 }
    };

    //Player number selection variables and dropdown
    public Transform playerNumberSelect = null;
    readonly Dictionary<int, int> playerNumberSelectOptions = new Dictionary<int, int>() {
        {0, 2 },
        {1, 3 },
        {2, 4 }
    };

    //Player race selection variables and dropdown
    public Transform raceSelect = null;
    readonly Dictionary<int, Race> raceSelectOptions = new Dictionary<int, Race>() {
        {0, Race.Magic },
        {1, Race.Undead },
        {2, Race.Forest },
        {3, Race.Human }
    };

    //Player hero selection variables and dropdown
    public Transform heroSelect = null;

    //Player deck selection dropdown
    public Transform deckSelect = null;

    //Player race information and image
    Transform raceImage = null;
    Transform raceDescription = null;

    //Player hero information and image
    Transform heroImage = null;
    Transform heroDescription = null;

    //Array for sprites for race and hero information
    Object[] sprites = null;

    //Game setup data to be passed into setup data object
    int boardSize;
    int numPlayers;
    Race[] playerRaces = new Race[4];
    Hero[] playerHeroes = new Hero[4];
    List<Deck> playerDecks;

    //Returns to the main menu
    public void LoadMainMenu() {
        SceneManager.LoadScene(0);
    }

    //Opens the deck building scene
    public void LoadDeckBuilder() {
        SceneManager.LoadScene(2);
    }

    //Starts the game and passes the game setup data into the data object
    public void LoadGame() {
        GameSetupData.boardSize = boardSize;
        GameSetupData.numPlayers = numPlayers;
        GameSetupData.playerRaces = playerRaces;
        GameSetupData.playerHeroes = playerHeroes;
        GameSetupData.playerDecks = playerDecks;

        SceneManager.LoadScene(3);
    }
    
    //Updates the board size information
    public void UpdateBoardSize() {
        boardSize = actualBoardSizeOptions[boardSizeSelectOptions[boardSizeSelect.GetComponent<Dropdown>().value]];
    }

    //Updates the number of players information
    public void UpdateNumberPlayers() {
        numPlayers = playerNumberSelectOptions[playerNumberSelect.GetComponent<Dropdown>().value];
    }

    //Updates the player race information and image
    public void UpdateRaceInfo() {
        playerRaces[0] = raceSelectOptions[raceSelect.GetComponent<Dropdown>().value];

        switch (playerRaces[0]) {
            case Race.Magic:
                raceImage.GetComponent<Image>().sprite = (Sprite)sprites[1];
                return;
            case Race.Undead:
                raceImage.GetComponent<Image>().sprite = (Sprite)sprites[2];
                return;
            case Race.Forest:
                raceImage.GetComponent<Image>().sprite = (Sprite)sprites[3];
                return;
            case Race.Human:
                raceImage.GetComponent<Image>().sprite = (Sprite)sprites[4];
                return;
            default:
                return;
        }
    }

    //Updates the player hero information and image
    public void UpdateHeroInfo() {
        playerHeroes[0] = Hero.Demon;

        switch (playerHeroes[0]) {
            case Hero.Demon:
                heroImage.GetComponent<Image>().sprite = (Sprite)sprites[3];
                return;
            default:
                heroImage.GetComponent<Image>().sprite = (Sprite)sprites[4];
                return;
        }
    }

    public void Start () {
        //Sets the race and hero images and information
        raceImage = this.transform.Find("Information Panel/Race Image");
        raceDescription = this.transform.Find("Information Panel/Race Description");
        heroImage = this.transform.Find("Information Panel/Hero Image");
        heroDescription = this.transform.Find("Information Panel/Hero Description");

        //Gets the images for the hero and race information
        sprites = AssetDatabase.LoadAllAssetsAtPath("Assets/Artwork/Tiles/Images/Practice Grayscale.png");

        //Gets the initial values of all of the selections
        UpdateBoardSize();
        UpdateNumberPlayers();
        UpdateRaceInfo();
        UpdateHeroInfo();
        playerDecks = new List<Deck>();
    }
}
