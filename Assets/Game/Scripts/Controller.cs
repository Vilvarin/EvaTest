using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// Контроллер. Обрабатывает нажатие на фишку
/// </summary>
public class Controller : MonoBehaviour {
    Piece _firstClick = null;
    bool _locked = false;
    Board _board;

    void Start()
    {
        _board = GetComponent<Board>();
        Piece[,] pieces = _board.GetPieces();

        foreach (Piece piece in pieces)
        {
            piece.clickAction += piece_clickAction;
        }

        _board.endOfCycle += _board_endOfCycle;
    }

    void _board_endOfCycle(object sender, EventArgs e)
    {
        _locked = false;
    }

    void piece_clickAction(object sender, PiecesEventArgs e)
    {
        if (_locked)
        {
            return;
        }
        else if (_firstClick == null)
        {
            _firstClick = e.Value;
            _firstClick.AddSelected();
        }
        else if(Board.CheckForSwap(_firstClick, e.Value) && _firstClick != e.Value)
        {
            _locked = true;
            _firstClick.RemoveSelected();
            StartCoroutine(_board.Swap(_firstClick, e.Value));
            _firstClick = null;
        }
        else
        {
            _firstClick.RemoveSelected();
            _firstClick = null;
        }
    }
}
