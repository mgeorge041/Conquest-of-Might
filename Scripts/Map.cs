using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class Map : MonoBehaviour {

    public int width = 3;
    public int height = 3;
    public int size = 3;
    public int numTiles = 1;
    public Tile prefabTile;

    Tile[] tiles;
    public Tile highlightedTile;

    public Mesh tileMesh;
    public Vector3[] vertices;
    public int[] triangles;
    public Color[] triangleColors;

    //Provides the coordinates to find tile neighbors
    Vector3[] neighbors = {
            new Vector3(-1, 0, 1),
            new Vector3(0, -1, 1),
            new Vector3(1, -1, 0),
            new Vector3(1, 0, -1),
            new Vector3(0, 1, -1),
            new Vector3(-1, 1, 0)
        };

    void Start() {

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
        tileMesh = new Mesh();
        tileMesh.vertices = vertices;
        tileMesh.triangles = triangles;
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

        Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        int edge = 100;

        //Finds the relative column position of the mouse
        float column = position.x / (3f / 2f * edge);
        int highColumn = (int)Mathf.Ceil(column);
        int lowColumn = (int)Mathf.Floor(column);

        //Finds the relative row position of the mouse
        float row = position.y / (Mathf.Sqrt(3) / 2f * edge);
        int highRow = (int)Mathf.Ceil(row);
        int lowRow = (int)Mathf.Floor(row);

        Tile[] closestTiles = new Tile[2];
        int tileNum = 0;

        //Finds the two closest tiles to the mouse cursor
        foreach (Tile tile in tiles) {
            int x = (int)tile.tileCoords[0];
            int y = (int)tile.tileCoords[1];
            int z = (int)tile.tileCoords[2];
            if ((x == lowColumn || x == highColumn) && (-y + z == lowRow || -y + z == highRow)) {
                closestTiles[tileNum] = tile;
                tileNum++;
            }
            if (tileNum > 1)
                break;
        }

        //If the mouse is off the edge of the map
        if (closestTiles[0] == null && closestTiles[1] != null) {
            if (GetDistanceFromMouseToTile(position, closestTiles[1]) < edge) {
                closestTiles[1].SetVertexColor(Color.green);
                if (highlightedTile != closestTiles[1]) {
                    highlightedTile.SetVertexColor(Color.blue);
                    highlightedTile = closestTiles[1];
                }
            }
        }
        //If the mouse is off the edge of the map
        else if (closestTiles[0] != null && closestTiles[1] == null) {
            if (GetDistanceFromMouseToTile(position, closestTiles[0]) < edge) {
                closestTiles[0].SetVertexColor(Color.green);
                if (highlightedTile != closestTiles[0]) {
                    highlightedTile.SetVertexColor(Color.blue);
                    highlightedTile = closestTiles[0];
                }
            }
        }
        //Gets the closest of the two closest tiles and sets as the highlighted tile
        else if (closestTiles[0] != null && closestTiles[1] != null) { 
            if (GetDistanceFromMouseToTile(position, closestTiles[0]) <= GetDistanceFromMouseToTile(position, closestTiles[1])) {
                closestTiles[0].SetVertexColor(Color.green);
                if (highlightedTile != closestTiles[0]) {
                    highlightedTile.SetVertexColor(Color.blue);
                    highlightedTile = closestTiles[0];
                }
            }
            else {
                closestTiles[1].SetVertexColor(Color.green);
                if (highlightedTile != closestTiles[1]) {
                    highlightedTile.SetVertexColor(Color.blue);
                    highlightedTile = closestTiles[1];
                }
            }
        }
    }

    //Finds distance from the mouse cursor to the tiles
    public float GetDistanceFromMouseToTile(Vector3 mouse, Tile tileA) {
        return Vector2.Distance(tileA.center, new Vector2(mouse[0], mouse[1]));
    }

    public void HighlightNeighbors(int range) {
        int possibleTiles = 0;
        int count = 0;
        int[] nextNeighbor = { 2, 3, 4, 5, 0, 1 };

        for (int i = range; i > 0; i--)
            possibleTiles += i * 6;

        Vector3[] neighborCoords = new Vector3[possibleTiles];
        Vector3 currentCoords = new Vector3();

        if (highlightedTile != null) {
            //Gets neighbors out to range
            for (int i = 0; i < range; i++) {
                currentCoords = highlightedTile.tileCoords;
                //Move out to neighbor 0 number of times equal to range
                for (int j = 0; j < i + 1; j++) {
                    currentCoords = currentCoords + neighbors[0];
                    if (j == i) {
                        neighborCoords[count] = currentCoords;
                        count++;
                    }
                }
                //Moves number of times around circle equal to range
                for (int k = 0; k < 5; k++) {
                    for (int j = 0; j < i + 1; j++) {
                        currentCoords = currentCoords + neighbors[nextNeighbor[k]];
                        neighborCoords[count] = currentCoords;
                        count++;
                    }
                }
                //Moves to last neighbor(s) number of times equal to range - 1
                for (int k = 0; k < i; k++) {
                    currentCoords = currentCoords + neighbors[nextNeighbor[5]];
                    neighborCoords[count] = currentCoords;
                    count++;
                }
            }

            foreach (Tile t in tiles) {
                if (!neighborCoords.Contains(t.tileCoords)) {
                    t.SetVertexColor(Color.blue);
                }
                else {
                    t.SetVertexColor(Color.green);
                }
            }
        }
    }

    public void Update() {
        TouchTile();
        HighlightNeighbors(3);
        GenerateMesh();
    }
}
