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
    List<GameObject> filaPieces = new List<GameObject>();
    List<GameObject> colmnPieces = new List<GameObject>();

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
                var o = Instantiate(tileObject, new Vector3(x, y, -5), Quaternion.identity, gameObject.transform.GetChild(0).transform);
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
                var o = Instantiate(avalaiblePieces[r], new Vector3(x, y, -5), Quaternion.identity, gameObject.transform.GetChild(1).transform);
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
        CheckMatch3(EndPiece);

        Pieces[endTile.x, endTile.y] = StartPiece;
        CheckMatch3(StartPiece); 
    }

    void CheckMatch3(Piece piece_)
    {
        //Limpia las listas
        colmnPieces.Clear();
        filaPieces.Clear();

        //Añade la pieza a ambas listas
        colmnPieces.Add(piece_.gameObject);
        filaPieces.Add(piece_.gameObject);

        if (piece_.y + 1 <= 9)
        {
            //Si la pieza de arriba está dentro de los márgenes comprueba en dicha dirección
            CheckMatch3Up(piece_);
        }
        if (piece_.y - 1 >= 0)
        {
            //Si la pieza de abajo está dentro de los márgenes comprueba en dicha dirección
            CheckMatch3Down(piece_);
        }
        if (piece_.x + 1 <= 5)
        {
            //Si la pieza de la derecha está dentro de los márgenes comprueba en dicha dirección
            CheckMatch3Right(piece_);
        }
        if (piece_.x - 1 >= 0)
        {
            //Si la pieza de la izquierda está dentro de los márgenes comprueba en dicha dirección
            CheckMatch3Left(piece_);
        }

        if (colmnPieces.Count >= 3)
        {
            //Si hay una columna de al menos 3 piezas de largo, las destruye
            for (int i = 0; i < colmnPieces.Count; i++)
            {
                Destroy(colmnPieces[i]);
            }
        }

        if (filaPieces.Count >= 3)
        {
            //Si hay una fila de al menos 3 piezas de largo, las destruye
            for (int i = 0; i < colmnPieces.Count; i++)
            {
                Destroy(filaPieces[i]);
            }
        }
    }


    void CheckMatch3Up(Piece piece_)
    {
        Debug.Log(piece_.y + 1);
        Piece pieceUp = Pieces[piece_.x, piece_.y + 1];

        if (piece_.GetComponent<Piece>().pieceType == pieceUp.GetComponent<Piece>().pieceType)
        {
            //Si son iguales, añade la pieza a la lista de columna
            colmnPieces.Add(pieceUp.gameObject);

            if (pieceUp.y + 1 <= 9)
            {
                //Si la pieza de arriba está dentro de los márgenes comprueba en dicha dirección
                CheckMatch3Up(pieceUp);
            }
        }
    }

    void CheckMatch3Down(Piece piece_)
    {
        Piece pieceDown = Pieces[piece_.x, piece_.y - 1];

        if (piece_.GetComponent<Piece>().pieceType == pieceDown.GetComponent<Piece>().pieceType)
        {
            //Si son iguales, añade la pieza a la lista de columna
            colmnPieces.Add(pieceDown.gameObject);

            if (pieceDown.y - 1 >= 0)
            {
                //Si la pieza de abajo está dentro de los márgenes comprueba en dicha dirección
                CheckMatch3Down(pieceDown);
            }            
        }
    }

    void CheckMatch3Right(Piece piece_)
    {
        Piece pieceRight = Pieces[piece_.x + 1, piece_.y];

        if (piece_.GetComponent<Piece>().pieceType == pieceRight.GetComponent<Piece>().pieceType)
        {
            //Si son iguales, añade la pieza a la lista de fila
            filaPieces.Add(pieceRight.gameObject);

            if (pieceRight.x + 1 <= 5)
            {
                //Si la pieza de la derecha está dentro de los márgenes comprueba en dicha dirección
                CheckMatch3Right(pieceRight);
            }
        }
    }

    void CheckMatch3Left(Piece piece_)
    {
        Piece pieceLeft = Pieces[piece_.x - 1, piece_.y];

        if (piece_.GetComponent<Piece>().pieceType == pieceLeft.GetComponent<Piece>().pieceType)
        {
            //Si son iguales, añade la pieza a la lista de fila
            filaPieces.Add(pieceLeft.gameObject);

            if (pieceLeft.x - 1 >= 0)
            {
                //Si la pieza de la izquierda está dentro de los márgenes comprueba en dicha dirección
                CheckMatch3Left(pieceLeft);
            }
        }
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
