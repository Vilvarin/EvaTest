using UnityEngine;
using System;
using System.Collections;

public class ScoreEventArgs: EventArgs
{
    public int Value { get; private set; }

    public ScoreEventArgs(int score)
    {
        Value = score;
    }
}
