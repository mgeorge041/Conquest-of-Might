using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.EventSystems;

public class Hand : MonoBehaviour {

    public Player player;
    List<Card> cards;
    int cardCount = 0;

    int foodCount = 0;
    int woodCount = 0;
    int manaCount = 0;

    public GameObject cardUnitPrefab;
    public GameObject cardBuildingPrefab;
    

    void Start() {
        cards = new List<Card>();
    }

    public void DrawCard(Card card) {
        cards.Add(card);
        cardCount++;
        
        card.Initialize(this, card);

        UpdateResourceCount();
    }

    public void PlayCard(Card card) {

        cards.Remove(card);
        cardCount--;
    }

    public void RemoveCard(CardDrag cardDrag) {
        for (int i = 0; i < this.transform.childCount; i++) {
            if (this.transform.GetChild(i).Equals(cardDrag)) {
                Destroy(this.transform.GetChild(i));
            }
        }
    }

    public void UpdateResourceCount() {
        int food = 0;
        int wood = 0;
        int mana = 0; 

        foreach (Card card in cards) {
            if (card.cardType == CardType.Resource) {
                if (card.cardName == "Food")
                    food++;
                else if (card.cardName == "Wood")
                    wood++;
                else
                    mana++;
            }
        }

        foodCount = food;
        woodCount = wood;
        manaCount = mana;
    }

    public int GetCardCount() {
        return cardCount;
    }
}
