using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Custom serialization because weaver is ignoring my struct
/// </summary>
public static class PuzzleInformationSerialization
{
    public static void WritePuzzleId(this NetworkWriter writer, PuzzleInformation pi)
    {
        writer.WriteBoolean(pi.completed);
    }

    public static PuzzleInformation ReadPuzzleInformation(this NetworkReader reader)
    {
        return new PuzzleInformation(reader.ReadBoolean());
    }
}
