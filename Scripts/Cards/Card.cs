using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public abstract class Card : ScriptableObject {

    GameObject cardObject;
    string assetPath;

    public string cardName;
    public string cardDesc;

    public Sprite artwork;

    public Race race;
    public CardType cardType;
    public Resource res1;
    public Resource res2;
    public int res1Cost;
    public int res2Cost;

    //Creates a card of card type and sets the asset path
    public static Card CreateCard(CardType cardType, string assetPath) {
        Card newCard;

        if (cardType == CardType.Unit) {
            newCard = CreateInstance<CardUnit>();
        }
        else if (cardType == CardType.Building) {
            newCard = CreateInstance<CardBuilding>();
        }
        else {
            newCard = CreateInstance<CardSpell>();
        }

        newCard.SetAssetPath(assetPath);
        newCard.cardType = cardType;

        return newCard;
    }

    public void Initialize(Hand hand, Card card) {
        Debug.Log(card.assetPath);
        if (cardType == CardType.Unit) {
            cardObject = Instantiate(hand.cardUnitPrefab);
            cardObject.GetComponent<CardUnitDisplay>().card = (CardUnit)AssetDatabase.LoadAssetAtPath(assetPath, typeof(CardUnit));
            cardObject.GetComponent<CardUnitDisplay>().card.SetAssetPath(assetPath);
            cardObject.GetComponent<CardDrag>().SetPlayer(hand.player);
        }
        else if (cardType == CardType.Building) {
            cardObject = Instantiate(hand.cardBuildingPrefab);
            cardObject.GetComponent<CardBuildingDisplay>().card = (CardBuilding)AssetDatabase.LoadAssetAtPath(assetPath, typeof(CardBuilding));
            cardObject.GetComponent<CardBuildingDisplay>().card.SetAssetPath(assetPath);
            cardObject.GetComponent<CardDrag>().SetPlayer(hand.player);
        }
        //else
        //   cardObject.GetComponent<CardSpellDisplay>().card = (CardSpell)AssetDatabase.LoadAssetAtPath(assetPath, typeof(ScriptableObject));

        cardObject.GetComponent<CardDrag>().player = hand.player;
        cardObject.transform.SetParent(hand.transform);
        cardObject.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
    }

    public void SetAssetPath(string assetPath) {
        this.assetPath = assetPath;
    }

    public string GetAssetPath() {
        return assetPath;
    }

}
