using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public struct PuzzleInformation : IEquatable<PuzzleInformation>
{
    public bool completed;

    public override bool Equals(object obj)
    {
        return obj is PuzzleInformation information && Equals(information);
    }

    public bool Equals(PuzzleInformation other)
    {
        return completed == other.completed;
    }
}
