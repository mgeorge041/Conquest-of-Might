using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile {

    public static int edge = 2;
    public static float height;
    public static int width;

    public Vector3 tileCoords;
    public Vector2 center;
    public Vector3[] vertices;
    public Color[] vertexColors;

    public Tile[] neighborTiles;

    bool canDrop = false;
    bool hasPiece = false;
    GamePiece piece = null;

    public Tile(int x, int y, int z) {

        //Sets dimensions for hex tile size
        height = edge * Mathf.Sqrt(3);
        width = edge * 2;

        vertexColors = new Color[6];
        neighborTiles = new Tile[6];

        CreateVertices(x, y, z);
    }

    //Sets the center and vertices for the hex tile
    public void CreateVertices(int x, int y, int z) {

        //Sets hex tiles coordinates in xyz
        tileCoords = new Vector3(x, y, z);

        //Sets center of hex tile and moves rect transform
        center = new Vector3(x * 3f / 2f * edge, (-y + z) * edge / 2 * Mathf.Sqrt(3));

        //Sets hex tile vertices
        vertices = new Vector3[] {
            new Vector3(center[0] - edge, center[1], 0),
            new Vector3(center[0] - edge / 2, center[1] + height / 2, 0),
            new Vector3(center[0] + edge / 2, center[1] + height / 2, 0),
            new Vector3(center[0] + edge, center[1], 0),
            new Vector3(center[0] + edge / 2, center[1] - height / 2, 0),
            new Vector3(center[0] - edge / 2, center[1] - height / 2, 0)
        };

        SetVertexColor(Color.blue);
    }

    //Sets vertex colors
    public void SetVertexColor(Color color) {
        for (int i = 0; i < 6; i++)
            vertexColors[i] = color;
    }

    //Returns the vertices of the hex tile
    public Vector3[] GetVertices() {
        return vertices;
    }

    //Returns the array of vertex colors
    public Color[] GetVertexColors() {
        return vertexColors;
    }

    //Returns the array of vertex colors
    public Color GetVertexColor() {
        return vertexColors[0];
    }

    //Returns an individual neighbor
    public Tile GetNeighbor(int neighbor) {
        return neighborTiles[neighbor];
    }

    //Returns the array of neighbors
    public List<Tile> GetNeighbors() {
        List<Tile> neighbors = new List<Tile>();
        for(int i = 0; i < 6; i++) {
            if (neighborTiles[i] != null)
                neighbors.Add(neighborTiles[i]);
        }
        return neighbors;
    }

    public void SetPiece(GamePiece piece) {
        this.piece = piece;
        hasPiece = true;
        piece.transform.localPosition = center;
    }

    public void RemovePiece() {
        piece = null;
        hasPiece = false;
    }

    public CardType GetUnitType() {
        if (piece != null)
            return piece.GetCardType();
        return CardType.None;
    }

    public GamePiece GetPiece() {
        return piece;
    }

    public void SetHasPiece(bool hasPiece) {
        this.hasPiece = hasPiece;
    }

    public bool HasPiece() {
        return hasPiece;
    }

    public void SetCanDrop(bool canDrop) {
        this.canDrop = canDrop;
    }

    public bool CanDrop() {
        return canDrop;
    }
}
