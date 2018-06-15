using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    public void OnPointerEnter(PointerEventData eventData) {
        if (eventData.pointerDrag == null)
            return;

        //If enters into droppable zone, sets zone to destination
        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        d.finalParent = this.transform;
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (eventData.pointerDrag == null)
            return;

        //If leaves droppable zone, sets destination to origin
        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        d.finalParent = d.initialParent;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
