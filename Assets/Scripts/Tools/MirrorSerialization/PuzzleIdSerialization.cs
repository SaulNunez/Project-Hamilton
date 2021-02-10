using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PuzzleIdSerialization
{
    public static void WritePuzzleId(this NetworkWriter writer, PuzzleId puzzleId)
    {
        writer.WriteInt32((int)puzzleId);
    }

    public static PuzzleId ReadPuzzleId(this NetworkReader reader)
    {
        return (PuzzleId)reader.ReadInt32();
    }
}
