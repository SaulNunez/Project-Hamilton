using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Used for puzzle regarding booleans. Uses a lever as a way to teach concept of booleans.
/// </summary>
public class CompleteBoolPuzzle : PuzzleBase
{
    [Tooltip("Sprite to use for on state")]
    [SerializeField]
    Sprite onLever;

    [Tooltip("UI element for puzzle that show current lever state")]
    [SerializeField]
    Image leverElement;

    /// <summary>
    /// For UI, it sends a server command to complete and sets button to show the lever has been activated
    /// </summary>
    public void CompletePuzzle()
    {
        CmdSendPuzzleState(true);
        leverElement.sprite = onLever;
    }

    [Command(requiresAuthority = false)]
    void CmdSendPuzzleState(bool toggleState, NetworkConnectionToClient sender = null)
    {
        if (toggleState)
        {
            PuzzleCompletion.instance.MarkCompleted(PuzzleId.VariableBoolean, sender.identity);
            TargetClosePuzzle(sender);
            TargetRunCorrectFeedback(sender);
        }
    }
}
