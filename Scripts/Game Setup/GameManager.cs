using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public Map map;
    public Player playerObject;

    Player[] players;
    string cityPath;

    readonly Vector3[] startLocations = {
        new Vector3(0, GameSetupData.boardSize - 2, -(GameSetupData.boardSize - 2)),
        new Vector3(0, -(GameSetupData.boardSize - 2), GameSetupData.boardSize - 2),
        new Vector3(-(GameSetupData.boardSize - 2), GameSetupData.boardSize - 2, 0),
        new Vector3(GameSetupData.boardSize - 2, -(GameSetupData.boardSize - 2), 0),
        new Vector3(-(GameSetupData.boardSize - 2), 0, GameSetupData.boardSize - 2),
        new Vector3(GameSetupData.boardSize - 2, 0, -(GameSetupData.boardSize - 2))
    };

    // Use this for initialization
    void Start() {
        players = new Player[GameSetupData.numPlayers];

        cityPath = "Assets/Cards/Building Cards/City.asset";
        CardBuilding city = (CardBuilding)Card.CreateCard(CardType.Building, cityPath);

        for (int i = 0; i < players.Length; i++) {
            players[i] = Instantiate(playerObject);
            players[i].playerIndex = i;
            players[i].transform.SetParent(this.transform);
            Building building = (Building)GamePiece.CreatePiece(players[i], city, map.GetTileFromCoords(startLocations[i]));
            map.AddBuilding(building);
        }

    }
}
