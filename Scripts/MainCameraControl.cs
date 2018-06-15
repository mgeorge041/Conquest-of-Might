using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraControl : MonoBehaviour {

    int moveSpeed = 10;
    float scrollSpeed = 100;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        CameraMove();
        CameraResetZoom();
	}

    public void CameraMove() {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        float moveZ = Input.GetAxis("Mouse ScrollWheel");

        this.transform.position = new Vector3(this.transform.position.x + moveX * moveSpeed,
            this.transform.position.y + moveY * moveSpeed, -10);
        this.GetComponent<Camera>().orthographicSize = Mathf.Clamp(this.GetComponent<Camera>().orthographicSize - moveZ * scrollSpeed, 350, 750);
    }

    public void CameraResetZoom() {
        if (Input.GetKey("z"))
            this.GetComponent<Camera>().orthographicSize = 500;
    }
}
