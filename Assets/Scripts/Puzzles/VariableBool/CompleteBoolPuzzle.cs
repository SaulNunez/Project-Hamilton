using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Used for puzzle regarding booleans. Uses a lever as a way to teach concept of booleans.
/// </summary>
public class CompleteBoolPuzzle : PuzzleBase
{
    [SyncVar]
    bool gen1IsTurnOn;

    [SyncVar]
    bool gen2IsTurnOn;

    [SerializeField]
    TextMeshProUGUI generator1IsTurnOnText;
    [SerializeField]
    TextMeshProUGUI generator2IsTurnOnText;
    [SerializeField]
    Button supportGenTurnOnButton;
    [SerializeField]
    Button supportGenDoesntTurnOnButton;

    private string BoolToPSeIntLiteral(bool value) => value ? "Verdadero" : "Falso";

    public override void OnStartServer()
    {
        base.OnStartServer();

        gen1IsTurnOn = Random.value > 0.5f;
        gen2IsTurnOn = Random.value > 0.5f;
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        generator1IsTurnOnText.text = BoolToPSeIntLiteral(gen1IsTurnOn);
        generator2IsTurnOnText.text = BoolToPSeIntLiteral(gen2IsTurnOn);
    }

    [Client]
    public void TurnsOn() => CmdSendPuzzleState(turnsOnBackup: true);

    [Client]
    public void TurnsOff() => CmdSendPuzzleState(turnsOnBackup: false);

    [Command(requiresAuthority = false)]
    void CmdSendPuzzleState(bool turnsOnBackup, NetworkConnectionToClient sender = null)
    {
        bool puzzleLogicResult = !(gen1IsTurnOn || gen2IsTurnOn);
        if ((turnsOnBackup && puzzleLogicResult)
            || (!turnsOnBackup && !puzzleLogicResult))
        {
            PuzzleCompletion.instance.MarkCompleted(PuzzleId.VariableBoolean, sender.identity);
            TargetClosePuzzle(sender);
            TargetRunCorrectFeedback(sender);
        } else
        {
            TargetRunWrongFeedback(sender);
        }
    }
}
