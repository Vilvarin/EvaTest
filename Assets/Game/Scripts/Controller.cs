using UnityEngine;
using System;
using System.Collections;

public class Controller : MonoBehaviour {
    Piece _firstClick = null;
    Board _board;

    void Start()
    {
        _board = GetComponent<Board>();
        Piece[,] pieces = _board.GetPieces();

        foreach (Piece piece in pieces)
        {
            piece.clickAction += piece_clickAction;
        }
    }

    void piece_clickAction(object sender, PiecesEventArgs e)
    {
        if (_firstClick == null)
        {
            _firstClick = e.Value;
            _firstClick.AddSelected();
        }
        else if(Board.CheckForSwap(_firstClick, e.Value) && _firstClick != e.Value)
        {
            _firstClick.RemoveSelected();
            _board.Swap(_firstClick, e.Value);
            _firstClick = null;
        }
        else
        {
            _firstClick.RemoveSelected();
            _firstClick = null;
        }
    }
}
