using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleBase : NetworkBehaviour
{
    [TargetRpc]
    protected virtual void TargetRunWrongFeedback(NetworkConnection target)
    {
        PuzzleSoundFeedback.instance.WrongAnswer();
    }

    [TargetRpc]
    protected virtual void TargetRunCorrectFeedback(NetworkConnection target)
    {
        PuzzleSoundFeedback.instance.CorrectAnswer();
    }

    [TargetRpc]
    protected virtual void TargetClosePuzzle(NetworkConnection target)
    {
        PuzzleUI.instance.ClosePuzzles();
    }
}
