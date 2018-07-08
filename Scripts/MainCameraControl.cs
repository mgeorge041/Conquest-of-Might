using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainCameraControl : MonoBehaviour {

    float moveSpeed = 0.25f;
    float scrollSpeed = 10;
    float mapX;
    float mapY;
    float maxXPos;
    float minXPos;
    float maxYPos;
    float minYPos;
    float maxHorizontal;
    float maxVertical;

    float prevMouseX;
    float prevMouseY;

    bool scrollMode = false;
    public Map map;
    public Image scrollImage;
    public GameUI gameUI;

	// Use this for initialization
	void Start () {
        this.GetComponent<Camera>().orthographicSize = 15;
        mapX = Tile.edge * 1.5f * (GameSetupData.boardSize + 3);
        mapY = Tile.edge * Mathf.Sqrt(3) * (GameSetupData.boardSize + 1);

        scrollImage = Instantiate(scrollImage);
        scrollImage.gameObject.SetActive(false);
        scrollImage.GetComponent<RectTransform>().sizeDelta = new Vector2(30, 30);
        scrollImage.transform.SetParent(gameUI.transform);
    }
	
	// Update is called once per frame
	void Update () {
        CameraMove();
        CameraResetZoom();
	}

    //Checks for user input for camera movement across board
    public void CameraMove() {
        CalculateCameraBounds();

        //Sets camera into scroll mode if map is not in select mode
        if (Input.GetKeyDown(KeyCode.Mouse2) && !map.InSelectMode()) {
            scrollMode = !scrollMode;
            prevMouseX = Input.mousePosition.x;
            prevMouseY = Input.mousePosition.y;
            scrollImage.gameObject.SetActive(!scrollImage.gameObject.activeSelf);
            scrollImage.transform.position = new Vector2(prevMouseX, prevMouseY);
        }

        //Camera movement variables
        float moveX = 0;
        float moveY = 0;    
        float moveZ = Input.GetAxis("Mouse ScrollWheel");

        //If camera is zoomed in or out
        if (moveZ != 0) {
            this.GetComponent<Camera>().orthographicSize = Mathf.Clamp(this.GetComponent<Camera>().orthographicSize - moveZ * scrollSpeed, 10, 20);
        }

        //Not in scroll mode, move via arrow keys
        //In scroll mode, move via mouse
        if (!scrollMode) {
            moveX = Input.GetAxis("Horizontal");
            moveY = Input.GetAxis("Vertical");
            if (moveX != 0 || moveY != 0) {
                this.transform.position = new Vector3(Mathf.Clamp(this.transform.position.x + moveX * moveSpeed, minXPos, maxXPos),
                    Mathf.Clamp(this.transform.position.y + moveY * moveSpeed, minYPos, maxYPos), -10);
            }
        }
        else {
            if (Input.mousePosition.x > prevMouseX + 15) {
                moveX = 1;
            }
            else if (Input.mousePosition.x < prevMouseX - 15) {
                moveX = -1;
            }
            if (Input.mousePosition.y > prevMouseY + 15) {
                moveY = 1;
            }
            else if (Input.mousePosition.y < prevMouseY - 15) {
                moveY = -1;
            }

            if (moveX != 0 || moveY != 0) {
                this.transform.position = new Vector3(Mathf.Clamp(this.transform.position.x + moveX * moveSpeed, minXPos, maxXPos),
                    Mathf.Clamp(this.transform.position.y + moveY * moveSpeed, minYPos, maxYPos), -10);
            }
        }
    }

    //Determines the range of the camera
    public void CalculateCameraBounds() {
        maxHorizontal = this.GetComponent<Camera>().orthographicSize * Screen.width / Screen.height;
        maxVertical = this.GetComponent<Camera>().orthographicSize;

        maxXPos = mapX - maxHorizontal;
        minXPos = -mapX + maxHorizontal;
        maxYPos = mapY - maxVertical;
        minYPos = -mapY + maxVertical;
    }

    //Resets camera zoom to starting zoom
    public void CameraResetZoom() {
        if (Input.GetKey(KeyCode.Z))
            this.GetComponent<Camera>().orthographicSize = 15;
    }

    //Centers camera to given tile
    public void CenterOnTile(Tile tile) {
        CalculateCameraBounds();
        this.transform.position = new Vector3(Mathf.Clamp(tile.center.x, minXPos, maxXPos),
            Mathf.Clamp(tile.center.y, minYPos, maxYPos), -10);
    }

    //Returns whether game is in scroll mode
    public bool InScrollMode() {
        return scrollMode;
    }
}
