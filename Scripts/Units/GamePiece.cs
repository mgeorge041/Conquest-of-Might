using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class GamePiece : MonoBehaviour {

    protected string pieceName;

    protected int maxHealth;
    protected int currentHealth;

    protected Race race;
    protected CardType cardType;

    public string assetPath;
    protected Player player;
    protected Tile tile;

    protected bool moveable;

    void Start() {
        this.transform.localScale = new Vector3(5, 5, 1);
    }

    public static GamePiece CreatePiece(Player player, Card card, Tile tile) {
        GamePiece piece;

        if (card.cardType == CardType.Unit) {
            piece = Instantiate(player.unitPrefab);
        }
        else {
            piece = Instantiate(player.buildingPrefab);
        }

        piece.SetAssetPath(card.GetAssetPath());
        piece.Initialize(player);
        piece.transform.SetParent(player.transform);
        piece.SetTile(tile);
        tile.SetPiece(piece);
        tile.SetHasPiece(true);

        return piece;
    }

    public CardType GetCardType() {
        return cardType;
    }

    public void SetAssetPath(string assetPath) {
        this.assetPath = assetPath;
    }

    public abstract void Initialize(Player player);

    public Tile GetTile() {
        return tile;
    }

    public void SetTile(Tile tile) {
        this.tile = tile;
    }

    public Player GetPlayer() {
        return player;
    }
}
