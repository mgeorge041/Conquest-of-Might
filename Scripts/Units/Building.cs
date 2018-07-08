using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Building : GamePiece {

    public CardBuilding cardBuilding;

    override public void Initialize(Player player) {
        this.player = player;
        moveable = false;

        Debug.Log(assetPath);
        GetComponent<Building>().cardBuilding = (CardBuilding)AssetDatabase.LoadAssetAtPath(assetPath, typeof(CardBuilding));

        GetComponent<SpriteRenderer>().sprite = cardBuilding.artwork;
        cardType = cardBuilding.cardType;
        pieceName = cardBuilding.cardName;

        maxHealth = cardBuilding.health;
        currentHealth = maxHealth;
    }
}
