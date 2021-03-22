using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// In charge of controlling stuff that happens when the games end.
/// 
/// Actions:
/// * Shows screen for loosers and winners.
/// 
/// Results:
/// * Programmers win when they vote all assasins
/// * Programmers win when they finish all tasks
/// * Assasins win when there's the same number of programmers as of them, either by killing or by voting out
/// * Assasins win when players don't fix a sabotage on time
/// </summary>
public class SetWinnerLoserOnGameEnd : NetworkBehaviour
{
    [SerializeField]
    GameObject winnerScreen;

    [SerializeField]
    GameObject looserScreen;

    private EndGameResult? result = null;

    public enum EndGameResult
    {
        AssasinsWin,
        ProgrammersWin
    }

    public override void OnStartServer()
    {
        base.OnStartServer();

        Emergency.OnPlayersCouldntStopEmergency += OnPlayersCouldntStopEmergency;
        PuzzleCompletion.OnFinishedAllPuzzles += OnPuzzlesFinished;
    }

    private void OnPuzzlesFinished()
    {
        result = EndGameResult.ProgrammersWin;
    }

    private void OnPlayersCouldntStopEmergency()
    {
        result = EndGameResult.AssasinsWin;   
    }

    public override void OnStopServer()
    {
        base.OnStopServer();
    }
}
