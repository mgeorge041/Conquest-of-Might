using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Unit : GamePiece {

    public CardUnit cardUnit;

    int might;
    int range;
    int maxSpeed;
    int currentSpeed;

    override public void Initialize(Player player) {
        this.player = player;
        moveable = false;

        GetComponent<Unit>().cardUnit = (CardUnit)AssetDatabase.LoadAssetAtPath(assetPath, typeof(CardUnit));

        GetComponent<SpriteRenderer>().sprite = cardUnit.artwork;
        cardType = cardUnit.cardType;
        pieceName = cardUnit.cardName;

        maxHealth = cardUnit.health;
        currentHealth = maxHealth;
        might = cardUnit.might;
        range = cardUnit.range;
        maxSpeed = cardUnit.speed;
        currentSpeed = maxSpeed;
    }
}
