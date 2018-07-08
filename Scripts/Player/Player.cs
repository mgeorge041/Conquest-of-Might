using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEditor;

public class Player : MonoBehaviour {

    public const int maxCards = 15;
    public const int maxDeck = 60;

    public int playerIndex;

    public Hand hand;
    public Deck deck;
    public Map map;

    public List<Unit> units;
    public List<Building> buildings;

    public Unit unitPrefab;
    public Building buildingPrefab;

    public GameUI gameUI;
    public MainCameraControl mainCamera;

    public void Start () {
        //deck = new Deck();
    }

    public bool PlayCard(CardDrag cardDrag, PointerEventData eventData) {
        Tile tile = map.GetTileFromPosition(Camera.main.ScreenToWorldPoint(eventData.position));
        if (!tile.CanDrop()) {
            return false;
        }

        Unit unit = null;
        Building building = null;

        Card card;
        if (cardDrag.GetComponent<CardUnitDisplay>() == null) {
            card = cardDrag.GetComponent<CardBuildingDisplay>().card;
            building = (Building)GamePiece.CreatePiece(this, card, tile);
        }
        else {
            card = cardDrag.GetComponent<CardUnitDisplay>().card;
            unit = (Unit)GamePiece.CreatePiece(this, card, tile);
        }
        

        if (card.cardType == CardType.Unit) 
            map.AddUnit(unit);
        else if (card.cardType == CardType.Building)
            map.AddBuilding(building);
        hand.PlayCard(card);

        return true;
    }

    public void PlayBuilding(Vector3 tileCoords) {
        Tile tile = map.GetTileFromCoords(tileCoords);
        CardBuilding card = (CardBuilding)Card.CreateCard(CardType.Building, deck.allCards[0]);
        Building building = (Building)GamePiece.CreatePiece(this, card, tile);
        map.AddBuilding(building);
        //hand.PlayCard(card);
        if (!deck.DrawButtonEnabled())
            deck.EnableDrawButton();
    }

    public void DrawCard() {
        hand.DrawCard(deck.DrawTopCard());
        if (hand.GetCardCount() >= 15)
            deck.DisableDrawButton();
    }

    public void SetHand(Hand hand) {
        this.hand = hand;
    }

    public void SetGameUI(GameUI gameUI) {
        this.gameUI = gameUI;
    }
}
