using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PuzzleCompletionInfoSerialization
{
    public static void WritePuzzleCompletionInfo(this NetworkWriter writer, 
        PuzzleCompletion.PuzzleCompletionInfo puzzleCompletionInfo)
    {
        writer.WritePuzzleId(puzzleCompletionInfo.Id);
        writer.WriteNetworkIdentity(puzzleCompletionInfo.netIdentity);
    }

    public static PuzzleCompletion.PuzzleCompletionInfo ReadPuzzleCompletionInfo(this NetworkReader reader)
    {
        var puzzleId = reader.ReadPuzzleId();
        var netIdentity = reader.ReadNetworkIdentity();
        return new PuzzleCompletion.PuzzleCompletionInfo {
            netIdentity = netIdentity
        };
    }
}
