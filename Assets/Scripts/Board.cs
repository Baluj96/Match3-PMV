using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public int width, height;
    public GameObject tileObject;
    public float camSizeOffset, camVerticalOffset;
    public GameObject[] avalaiblePieces;

    Tile[,] Tiles;
    Piece[,] Pieces;

    Tile startTile, endTile;

    void Start()
    {
        Tiles = new Tile[width, height];
        Pieces = new Piece[width, height];

        SetupBoard();
        PositionCamera();
        SetupPieces();
    }

    void SetupBoard()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var o = Instantiate(tileObject, new Vector3(x, y, -5), Quaternion.identity, gameObject.transform);
                Tiles[x, y] = o.GetComponent<Tile>();
                Tiles[x, y]?.Setup(x, y, this);
            }
        }
    }

    void PositionCamera()
    {
        float newPosX = (float)width / 2f - 0.5f;
        float newPosY = (float)height / 2f - 0.5f + camVerticalOffset;
        Camera.main.transform.position = new Vector3(newPosX, newPosY, -10f);

        float horizontal = width + 1;
        float vertical = height / 2 + 1;
        Camera.main.orthographicSize = horizontal > vertical ? horizontal + camSizeOffset : vertical;
    }

    void SetupPieces()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int r = Random.Range(0, avalaiblePieces.Length);
                var o = Instantiate(avalaiblePieces[r], new Vector3(x, y, -5), Quaternion.identity, gameObject.transform);
                o.GetComponent<Piece>().Setup(x,y, this);
                Pieces[x, y] = o.GetComponent<Piece>();
                Pieces[x, y]?.Setup(x, y, this);
            }
        }
    }

    public void TileDown(Tile tile_)
    {
        startTile = tile_;
    }

    public void TileOver(Tile tile_)
    {
        endTile = tile_;
    }

    public void TileUp(Tile tile_)
    {
        if (startTile != null && endTile != null && IsCloseUp(startTile,endTile))
        {
            SwapTiles();
        }
    }

    void SwapTiles()
    {
        var StartPiece = Pieces[startTile.x, startTile.y];
        var EndPiece = Pieces[endTile.x, endTile.y];

        StartPiece.Move(endTile.x, endTile.y);
        EndPiece.Move(startTile.x, startTile.y);

        Pieces[startTile.x, startTile.y] = EndPiece;
        Pieces[endTile.x, endTile.y] = StartPiece;
    }

    public bool IsCloseUp(Tile start, Tile end)
    {
        if (Mathf.Abs(start.x - end.x) == 1 && start.y == end.y)
        {
            return true;
        }

        if (Mathf.Abs(start.y - end.y) == 1 && start.x == end.x)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
