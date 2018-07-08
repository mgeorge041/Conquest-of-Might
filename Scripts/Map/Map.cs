using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class Map : MonoBehaviour {

    int size;
    int numTiles = 1;

    Tile[] tiles;
    Tile highlightedTile;
    Color prevHighlightedTileColor = Color.blue;

    Mesh tileMesh;
    Vector3[] vertices;
    int[] triangles;
    Color[] triangleColors;

    bool gamePaused = false;
    bool selectMode = false;

    public List<Unit> units;
    public List<Building> buildings;
    public List<Player> players;
    public List<Tile> tilesWithPieces;
    public List<Tile> availableNeighbors;

    public MainCameraControl mainCamera;
    public float keyDelay = 0.1f;
    float timePassed = 0f; 

    //Provides the coordinates to find tile neighbors
    readonly Vector3[] neighbors = {
            new Vector3(-1, 0, 1),
            new Vector3(0, -1, 1),
            new Vector3(1, -1, 0),
            new Vector3(1, 0, -1),
            new Vector3(0, 1, -1),
            new Vector3(-1, 1, 0)
        };

    void Start() {
        size = GameSetupData.boardSize;
        units = new List<Unit>();
        buildings = new List<Building>();
        players = new List<Player>();
        tilesWithPieces = new List<Tile>();
        availableNeighbors = new List<Tile>();


        //Finds number of tiles in map based on map radius
        for (int i = size; i > 0; i--)
            numTiles += i * 6;
        tiles = new Tile[numTiles];
        highlightedTile = new Tile(0, 0, 0);

        //Mesh arrays
        triangles = new int[numTiles * 12];
        vertices = new Vector3[numTiles * 6];
        triangleColors = new Color[vertices.Length];

        //Mesh triangle coordinates
        triangles[0] = 1;
        triangles[1] = 5;
        triangles[2] = 0;
        triangles[3] = 1;
        triangles[4] = 4;
        triangles[5] = 5;
        triangles[6] = 1;
        triangles[7] = 2;
        triangles[8] = 4;
        triangles[9] = 2;
        triangles[10] = 3;
        triangles[11] = 4;

        //Creates all hex tiles in hexagon pattern
        for (int i = -size, count = 0; i <= size; i++) {
            int r1 = Mathf.Max(-size, -i - size);
            int r2 = Mathf.Min(size, -i + size);
            for (int j = r1; j <= r2; j++) {
                tiles[count] = new Tile(i, j, -i - j);

                //Adds vertexes to the mesh
                for (int k = 0; k < 6; k++) {
                    vertices[k + count * 6] = tiles[count].GetVertices()[k];
                }
                //Adds the triangles to the mesh
                for (int l = 0; l < 12 && count > 0; l++) {
                    triangles[l + count * 12] = triangles[l + (count - 1) * 12] + 6;
                }
                count++;
            }
        }

        //Sets all the tile neighbors
        foreach (Tile tile in tiles) {
            foreach (Tile tile2 in tiles) {
                for (int i = 0; i < 6; i++) {
                    if (tile.tileCoords + neighbors[i] == tile2.tileCoords) {
                        tile.neighborTiles[i] = tile2;
                        break;
                    }
                }
            }
        }

        //Creates the mesh  
        tileMesh = new Mesh {
            vertices = vertices,
            triangles = triangles
        };
        GenerateMesh();
    }

    //Updates the mesh 
    public void GenerateMesh() {
        UpdateMeshColor();
        tileMesh.colors = triangleColors;
        GetComponent<MeshFilter>().mesh = tileMesh;
        GetComponent<MeshCollider>().sharedMesh = tileMesh;
    }

    //Updates the tile colors
    public void UpdateMeshColor() {
        int count = 0;
        foreach (Tile tile in tiles) {
            for (int j = 0; j < 6; j++) {
                triangleColors[j + count * 6] = tile.GetVertexColors()[j];
            }
            count++;
        }
    }

    //Checks if the mouse is touching a tile
    public void TouchTile() {
        //Returns if it is over a UI element
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        //Does not update if mouse is outside of game window
        Rect screenRect = new Rect(0, 0, Screen.width, Screen.height);
        if (!screenRect.Contains(Input.mousePosition))
            return;

        //Gets tile being touched by the mouse
        Tile newHighlightedTile = GetTileFromPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        if (highlightedTile != newHighlightedTile) {
            SetNewHighlightedTile(newHighlightedTile);
        }
    }

    //Sets the highlighted tile equal to the new tile
    public void SetNewHighlightedTile(Tile newHighlightedTile) {
        highlightedTile.SetVertexColor(prevHighlightedTileColor);
        prevHighlightedTileColor = newHighlightedTile.GetVertexColor();
        highlightedTile = newHighlightedTile;
        highlightedTile.SetVertexColor(Color.green);
    }

    //Gets tile from the given mouse position
    public Tile GetTileFromPosition(Vector3 position) {
        int edge = Tile.edge;
        Tile overTile = null;

        //Finds the relative column position of the mouse
        float column = position.x / (3f / 2f * edge);
        int highColumn = (int)Mathf.Ceil(column);
        if (highColumn > size)
            highColumn--;
        int lowColumn = (int)Mathf.Floor(column);

        //Finds the relative row position of the mouse
        float row = position.y / (Mathf.Sqrt(3) / 2f * edge);
        int highRow = (int)Mathf.Ceil(row);
        int lowRow = (int)Mathf.Floor(row);

        Tile[] closestTiles = new Tile[2];
        int tileNum = 0;

        //Finds the low and high index to search the array of tiles
        int lowIndex = GetLowTileIndex(lowColumn);
        int highIndex = GetHighTileIndex(lowColumn, highColumn, lowIndex);

        //Finds the two closest tiles to the mouse cursor
        for (int i = lowIndex; i <= highIndex; i++) {
            int x = (int)tiles[i].tileCoords[0];
            int y = (int)tiles[i].tileCoords[1];
            int z = (int)tiles[i].tileCoords[2];
            if ((x == lowColumn || x == highColumn) && (-y + z == lowRow || -y + z == highRow)) {
                closestTiles[tileNum] = tiles[i];
                tileNum++;
            }
            if (tileNum > 1)
                break;
        }

        //If the mouse is off the edge of the map
        if (closestTiles[0] == null && closestTiles[1] != null) {
            if (GetDistanceFromMouseToTile(position, closestTiles[1]) < edge) {
                overTile = closestTiles[1];
            }
        }
        //If the mouse is off the edge of the map
        else if (closestTiles[0] != null && closestTiles[1] == null) {
            if (GetDistanceFromMouseToTile(position, closestTiles[0]) < edge) {
                overTile = closestTiles[0];
            }
        }
        //Gets the closest of the two closest tiles and sets as the over tile
        else if (closestTiles[0] != null && closestTiles[1] != null) {
            if (GetDistanceFromMouseToTile(position, closestTiles[0]) <= GetDistanceFromMouseToTile(position, closestTiles[1])) {
                overTile = closestTiles[0];
            }
            else { 
                overTile = closestTiles[1];
            }
        }

        return overTile;
    }

    //Finds distance from the mouse cursor to the tiles
    public float GetDistanceFromMouseToTile(Vector3 mouse, Tile tileA) {
        return Vector2.Distance(tileA.center, new Vector2(mouse[0], mouse[1]));
    }

    //Gets all tiles within range 
    public List<Tile> GetNeighborTilesInRange(Tile startTile, int range) {
        List<Tile> neighborTiles = new List<Tile>();

        GetNeighborTiles(startTile, range, neighborTiles);
        neighborTiles.Remove(startTile);
        return neighborTiles;
    }

    //Recursively gets all tiles in range
    public void GetNeighborTiles(Tile startTile, int range, List<Tile> neighborTiles) {
        if (range == 0)
            return;
        for (int i = 0; i < 6; i++) {
            Tile neighbor = startTile.GetNeighbor(i);
            if (neighbor != null) {
                if (!neighborTiles.Contains(neighbor)) {
                    neighborTiles.Add(neighbor);
                }
                GetNeighborTiles(neighbor, range - 1, neighborTiles);
            }   
        }
    }

    //Highlights all neighbors without a game piece
    public void HighlightAvailableNeighbors(List<Tile> neighbors) {
        foreach (Tile neighborTile in neighbors) {
            if (neighborTile.HasPiece() == false) {
                neighborTile.SetVertexColor(Color.cyan);
                neighborTile.SetCanDrop(true);
                availableNeighbors.Add(neighborTile);
            }
        }
        Debug.Log(availableNeighbors.Count);
    }

    //Sets all tile colors back to blue
    public void ClearHighlightedTiles() {
        foreach (Tile tile in tiles) {
            tile.SetVertexColor(Color.blue);
            tile.SetCanDrop(false);
        }
    }

    //Sets all highlighted neighbor tiles to blue
    public void ClearAvailableNeighbors() {
        Debug.Log(availableNeighbors.Count);
        foreach (Tile tile in availableNeighbors) {
            tile.SetVertexColor(Color.blue);
            tile.SetCanDrop(false);   
        }
        availableNeighbors.Clear();
    }

    //Adds a unit to the board
    public void AddUnit(Unit unit) {
        //Adds unit to tile and tile to list of tiles with units
        tilesWithPieces.Add(unit.GetTile());
        units.Add(unit);
    }

    //Removes a unit from the board
    public void RemoveUnit(Unit unit) {
        foreach (Unit tileUnit in units) {
            if (tileUnit == unit) {
                tilesWithPieces.Remove(tileUnit.GetTile());
                units.Remove(tileUnit);
                Destroy(unit);
            }
        }
    }

    //Adds a building to the board
    public void AddBuilding(Building building) {
        //Adds building to tile and tile to list of tiles with buildings
        tilesWithPieces.Add(building.GetTile());
        buildings.Add(building);
    }

    //Removes a building from the board
    public void RemoveBuilding(Building building) {
        foreach (Building tileBuilding in buildings) {
            if (tileBuilding == building) {
                tilesWithPieces.Remove(tileBuilding.GetTile());
                buildings.Remove(tileBuilding);
                Destroy(building);
            }
        }
    }

    //Returns the tile from the given tile coordinates
    public Tile GetTileFromCoords(Vector3 tileCoords) {
        int xCoord = (int)tileCoords[0];
        int lowIndex = GetLowTileIndex(xCoord);
        int highIndex = GetHighTileIndex(xCoord, xCoord, lowIndex);

        //Sees if tile in range matches given tile coordinates
        for (int i = lowIndex; i <= highIndex; i++) {
            if (tiles[i].tileCoords == tileCoords)
                return tiles[i];
        }
        return null;
    }

    //Returns the lower index for the tile array
    public int GetLowTileIndex(int xCoord) {
        int lowIndex = 0;

        for (int i = xCoord - 1; i >= -size; i--) {
            lowIndex += 2 * size - Mathf.Abs(i) + 1;   
        }
        return lowIndex;
    }

    //Returns the upper index for the tile array
    public int GetHighTileIndex(int lowXCoord, int highXCoord, int lowIndex) {
        int highIndex = lowIndex;

        for (int i = highXCoord; i >= lowXCoord; i--) {
            highIndex += 2 * size - Mathf.Abs(i) + 1;
        }
        return highIndex - 1;
    }

    //Shows the potential locations for placement of a unit
    public void SetAvailableTiles(Player player) {
        foreach (Tile tileWithUnit in tilesWithPieces) {
            if (tileWithUnit.GetUnitType() == CardType.Building && tileWithUnit.GetPiece().GetPlayer().playerIndex == player.playerIndex) {
                HighlightAvailableNeighbors(GetNeighborTilesInRange(tileWithUnit, 1));
            }
        }
    }

    //Sets the game to paused
    public void SetGamePaused(bool gamePaused) {
        this.gamePaused = gamePaused;
    }

    //Returns if game is in select mode
    public bool InSelectMode() {
        return selectMode;
    }

    //Does actions on mousing over tiles
    public void OnMouseOver() {
        if (!gamePaused && !selectMode) {
            TouchTile();
            GenerateMesh();
        }
        
    }

    //Updates each frame
    void Update() {
        //Gets time that has passed
        timePassed += Time.deltaTime;

        //If key input and game is not paused
        if (Input.anyKey && !gamePaused) {
            if (selectMode && timePassed >= keyDelay) {

                //Moves highlighted tile depending on key pressed and centers camera
                if (Input.GetKey(KeyCode.W)) {
                    Tile neighbor = highlightedTile.GetNeighbor(0);
                    if (neighbor != null) {
                        SetNewHighlightedTile(neighbor);
                        mainCamera.CenterOnTile(neighbor);
                    }
                }
                else if (Input.GetKey(KeyCode.E)) {
                    Tile neighbor = highlightedTile.GetNeighbor(1);
                    if (neighbor != null) {
                        SetNewHighlightedTile(neighbor);
                        mainCamera.CenterOnTile(neighbor);
                    }
                }
                else if (Input.GetKey(KeyCode.R)) {
                    Tile neighbor = highlightedTile.GetNeighbor(2);
                    if (neighbor != null) {
                        SetNewHighlightedTile(neighbor);
                        mainCamera.CenterOnTile(neighbor);
                    }
                }
                else if (Input.GetKey(KeyCode.V)) {
                    Tile neighbor = highlightedTile.GetNeighbor(3);
                    if (neighbor != null) {
                        SetNewHighlightedTile(neighbor);
                        mainCamera.CenterOnTile(neighbor);
                    }
                }
                else if (Input.GetKey(KeyCode.C)) {
                    Tile neighbor = highlightedTile.GetNeighbor(4);
                    if (neighbor != null) {
                        SetNewHighlightedTile(neighbor);
                        mainCamera.CenterOnTile(neighbor);
                    }
                }
                else if (Input.GetKey(KeyCode.X)) {
                    Tile neighbor = highlightedTile.GetNeighbor(5);
                    if (neighbor != null) {
                        SetNewHighlightedTile(neighbor);
                        mainCamera.CenterOnTile(neighbor);
                    }
                }
                GenerateMesh();
                timePassed = 0;
            }

            //Enters select mode
            if (Input.GetKeyDown(KeyCode.S) && !mainCamera.InScrollMode())
                selectMode = !selectMode;
        } 
    }
}
