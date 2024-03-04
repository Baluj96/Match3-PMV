using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int x, y;
    public Board board;

    public void Setup(int x_, int y_, Board board_)
    {
        this.x = x_;
        this.y = y_;
        this.board = board_;
    }

    void OnMouseDown()
    {
        board.TileDown(this);
    }

    private void OnMouseUp()
    {
        board.TileUp(this);
    }

    private void OnMouseEnter()
    {
        board.TileOver(this);
    }
}
