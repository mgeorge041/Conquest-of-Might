using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class Deck : MonoBehaviour{

    int cardCount = 0;
    readonly int maxCards = 60;
    public List<Card> cards;
    public Transform cardCountLabel = null;
    public Transform drawButton = null;

    public string[] allCards;

    //Adds a card to the deck and updates label
    public void AddCard(Card card) {
        cards.Add(card);
        cardCount++;
        UpdateCardLabel();
    }

    //Removes a card from the deck and updates label
    public void RemoveCard(Card card) {
        cards.Remove(card);
        cardCount--;
        UpdateCardLabel();
    }

    //Reorders and shuffles the deck
    public void Shuffle() {
        List<Card> newDeck = new List<Card>();

        //Randomly selects a card from the deck and places into a new deck
        //Once all cards have been moved, it sets the old deck equal to the new deck
        for (int i = 0; i < cards.Count; i++) {
            int randomInt = Random.Range(0, cards.Count);
            Card cardToBeRemomved = cards[randomInt];
            newDeck.Add(cardToBeRemomved);
            cards.Remove(cardToBeRemomved);
        }
        cards = newDeck;
    }

	// Use this for initialization
	public void Start () {

        //Gets all the possible card types as asset paths
        allCards = AssetDatabase.FindAssets("t:Card");
        for (int i = 0; i < allCards.Length; i++) {
            allCards[i] = AssetDatabase.GUIDToAssetPath(allCards[i]);
        }
        
        //Creates a new deck of cards
        cards = new List<Card>();
        for (int i = 0; i < maxCards; i++) {
            int randomCard = Random.Range(0, allCards.Length);
            Card newCard;
            if (AssetDatabase.GetMainAssetTypeAtPath(allCards[randomCard]).Equals(typeof(CardUnit))) {
                newCard = Card.CreateCard(CardType.Unit, allCards[randomCard]);
            }
            else if (AssetDatabase.GetMainAssetTypeAtPath(allCards[randomCard]).Equals(typeof(CardBuilding))) {
                newCard = Card.CreateCard(CardType.Building, allCards[randomCard]);
            }
            else {
                newCard = Card.CreateCard(CardType.Spell, allCards[randomCard]);
            }

            AddCard(newCard);
        }

        //Updates the card count label
        cardCountLabel = this.transform.Find("Deck Card Count");
        UpdateCardLabel();

        //Sets the draw button for the deck
        drawButton = this.transform.Find("Draw Button");
	}

    //Updates the number of cards in the deck and updates the label
    void UpdateCardLabel() {
        cardCount = cards.Count;
        cardCountLabel.GetComponent<Text>().text = "x" + cardCount.ToString();
    }

    //Draws the top card from the deck and disables draw button if the deck is empty
    public Card DrawTopCard() {
        Card topCard = cards[0];
        RemoveCard(topCard);
        if (cardCount == 0)
            DisableDrawButton();
        return topCard;
    }

    //Disables the draw button
    public void DisableDrawButton() {
        drawButton.GetComponent<Button>().interactable = false;
    }

    //Enables the draw button
    public void EnableDrawButton() {
        drawButton.GetComponent<Button>().interactable = true;
    }

    public bool DrawButtonEnabled() {
        return drawButton.GetComponent<Button>().interactable;
    }
}
