using System;

/// <summary>
/// Контейнер для передачи фишки из события
/// </summary>
public class PiecesEventArgs : EventArgs
{
    public Piece Value { get; private set; }

    public PiecesEventArgs(Piece piece)
    {
        Value = piece;
    }
}
