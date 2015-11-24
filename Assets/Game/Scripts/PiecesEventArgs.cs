using System;

public class PiecesEventArgs : EventArgs
{
    public Piece Value { get; private set; }

    public PiecesEventArgs(Piece piece)
    {
        Value = piece;
    }
}
