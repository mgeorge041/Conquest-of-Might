﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

    public Player player;
    public Transform initialParent = null;

    GameObject placeholder = null;

    public void OnBeginDrag(PointerEventData eventData) {
        if (eventData == null)
            return;

        

        //Sets initial parent for dragged object
        initialParent = this.transform.parent;

        //Creates placeholder card
        placeholder = new GameObject();
        placeholder.transform.SetParent(initialParent);
        LayoutElement le = placeholder.AddComponent<LayoutElement>();
        le.preferredWidth = this.GetComponent<LayoutElement>().preferredWidth;
        le.preferredHeight = this.GetComponent<LayoutElement>().preferredHeight;
        placeholder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());

        //Sets card's parent to hand panel
        this.transform.SetParent(initialParent.parent);

        //Stops the card from blocking raycasts
        this.GetComponent<CanvasGroup>().blocksRaycasts = false;

        player.map.SetAvailableTiles(player);
        Debug.Log(this.transform.GetComponent<CardUnitDisplay>().card.GetAssetPath());
    }

    public void OnDrag(PointerEventData eventData) {
        if (eventData == null)
            return;

        int newIndex = this.transform.GetSiblingIndex();

        //Moves card around in the hand
        for (int i = 0; i < initialParent.childCount; i++) {
            Transform handCard = initialParent.GetChild(i);

            //Starting from left, sees if dragged card is left of card in hand
            if (this.transform.position.x < handCard.position.x) {
                newIndex = i;

                //If it is left of card in hand and right of placeholder, decrements placeholder index
                if (placeholder.transform.GetSiblingIndex() < newIndex)
                    newIndex--;

                break;
            }
            //If beyond the furthest right card in hand, sets to end card
            else if (this.transform.position.x > handCard.position.x) {
                newIndex = i;
            }
                
        }

        //Sets the new placeholder index
        placeholder.transform.SetSiblingIndex(newIndex);

        //Moves the card
        this.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData) {
        if (eventData == null)
            return;

        if (player.PlayCard(this, eventData)) {
            Destroy(this.gameObject);
            Destroy(placeholder);
        }
        else {
            //Sets the card's new final destination
            this.transform.SetParent(initialParent);
            this.transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());

            //Deletes the placeholder card
            Destroy(placeholder);

            //Enables the card to block raycasts
            this.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
        player.map.ClearAvailableNeighbors();
    }

    public void SetPlayer(Player player) {
        this.player = player;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
