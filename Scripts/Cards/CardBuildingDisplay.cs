using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardBuildingDisplay : MonoBehaviour {

    public Card card;

    public Text nameText;
    public Text descText;

    public Text healthText;
    public Text rangeText;

    public Image artwork;

    public Player player;


	// Use this for initialization
	void Start () {

        nameText.text = card.cardName;
        descText.text = card.cardDesc;

        artwork.sprite = card.artwork;
	    	
	}

    public void SetPlayer(Player player) {
        this.player = player;
    }
}
