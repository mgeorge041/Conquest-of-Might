using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardUnitDisplay : MonoBehaviour {

    public CardUnit card;

    public Text nameText;
    public Text descText;

    public Text healthText;
    public Text mightText;
    public Text rangeText;
    public Text speedText;

    public Image artwork;

    Player player;

    //public string assetPath;


	// Use this for initialization
	void Start () {

        nameText.text = card.cardName;
        descText.text = card.cardDesc;

        artwork.sprite = card.artwork;

        //assetPath = card.assetPath;

        /*
        healthText.text = card.health.ToString();
        mightText.text = card.might.ToString();
        rangeText.text = card.range.ToString();
        speedText.text = card.speed.ToString();
	    	*/
	}

    public void SetPlayer(Player player) {
        this.player = player;
    }
}
