using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HandCollapse : MonoBehaviour {

    Transform handPanel = null;
    Transform hand = null;
    RectTransform handPanelRect = null;

    Transform[] cards; 

    bool collapsed = false;

    public void CollapseHand() {
        //If hand is expanded, collapse
        if (collapsed == false) {
            //Sets anchor to bottom left corner and manually sets width
            handPanelRect.anchorMax = new Vector2(0.1f, 0.15f);
            handPanelRect.sizeDelta = new Vector4(0, 0, 0, 0);

            //GameObject thing = new GameObject();

            hand.GetComponent<HorizontalLayoutGroup>().spacing = 2;

            UpdateCardsInHand();

            Debug.Log(cards.Length);

            //foreach (Transform card in cards) {
            //    CardCollapse newCard = Instantiate<CardCollapse>(newCard);
            //}

            /*Transform hand = handPanel.GetChild(0);
            foreach (Transform card in hand) {
                GameObject tempCard = (GameObject)Instantiate(Resources.Load("Prefabs/Card Collapsed"));
                tempCard.transform.parent = hand;
                tempCard.transform.SetSiblingIndex(card.GetSiblingIndex());
                tempCard.GetComponent<CardDisplay>().nameText.text = card.GetComponent<CardDisplay>().nameText.text;
                Destroy(card);

            }*/


            collapsed = true;
        }
        //If hand is collapsed, expand
        else {
            //Sets anchor to bottom left corner and stretches along width
            handPanelRect.anchorMax = new Vector2(0.9f, 0.15f);
            handPanelRect.sizeDelta = new Vector4(0, 0, 0, 0);

            hand.GetComponent<HorizontalLayoutGroup>().spacing = 5;

            collapsed = false;
        }

        //Flips collapse button
        this.transform.Rotate(0, 0, 180);
    }

	// Use this for initialization
	void Start () {
        handPanel = this.transform.parent;
        handPanelRect = handPanel.transform.GetComponent<RectTransform>();
        hand = this.transform.parent.GetChild(0);

        UpdateCardsInHand();

        collapsed = false;
	}

    void Update() {

    }

    void UpdateCardsInHand() {
        cards = new Transform[hand.childCount];
        for (int i = 0; i < hand.childCount; i++)
            cards[i] = hand.GetChild(i);
    }


    public bool IsCollapse() {
        return collapsed;
    }
}
