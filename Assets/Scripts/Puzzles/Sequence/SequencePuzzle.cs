using Extensions;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

/// <summary>
/// Sequence puzzle config
/// 
/// Currently messy, there's no way to make sure on server this has been completed correctly.
/// </summary>
public class SequencePuzzle : PuzzleBase
{
    const int NOTHING = 0;
    const int GO_LEFT = 1;
    const int GO_RIGHT = 2;
    const int GO_DOWN = 3;
    const int GO_UP = 4;

    [Tooltip("Puzzle we are controlling")]
    [SerializeField]
    SequenceConfig sequencePuzzle;
    
    [Tooltip("Sequence puzzle to mark as solved")]
    [SerializeField]
    PuzzleId sequenceId;

    [SerializeField]
    SequenceGrid sequenceGrid;

    [SerializeField]
    List<TMP_Dropdown> instructionDropdowns;

    /// <summary>
    /// Call after sequence has been completed to count it on server
    /// </summary>
    [Client]
    public void VerifyPuzzle()
    {
        int[] stepsChosen = new int[instructionDropdowns.Count];
        for (int i = 0; i < instructionDropdowns.Count; i++)
        {
            TMP_Dropdown dropdown = instructionDropdowns[i];

            stepsChosen[i] = dropdown.value;
        }
        print("Steps took: " + string.Join(",", stepsChosen));

        CmdCompletePuzzle(stepsChosen);

        var stepsTaken = CheckStepsForPuzzle(stepsChosen);
        StartCoroutine(ReplayStepsOnClient(stepsTaken));
    }

    [Client]
    IEnumerator ReplayStepsOnClient(Vector2Int[] steps)
    {
        foreach(var step in steps)
        {
            sequenceGrid.CurrentPosition = step;
            yield return new WaitForSeconds(0.5f);
        }

        /*
         * Algo sucio, porque esto que normalmente se hace en el servidor
         * 
         * Se hace en el cliente para que el jugador pueda ver el resultado de su codigo
         */
        if (steps.Last() == sequencePuzzle.endPosition)
        {
            PuzzleSoundFeedback.instance.CorrectAnswer();
            PuzzleUI.instance.ClosePuzzles();
        } else
        {
            PuzzleSoundFeedback.instance.WrongAnswer();
            sequenceGrid.ResetSequence();
        }
    }

    [Command(requiresAuthority = false)]
    void CmdCompletePuzzle(int[] stepsChosen, NetworkConnectionToClient sender = null)
    {
        print("Server steps took: " + string.Join(",", stepsChosen));
        var stepsTaken = CheckStepsForPuzzle(stepsChosen);
        if(stepsTaken.Last() == sequencePuzzle.endPosition)
        {
            PuzzleCompletion.instance.MarkCompleted(sequenceId, sender.identity);
        }
    }

    Vector2Int[] CheckStepsForPuzzle(int[] stepsChosen)
    {
        List<Vector2Int> steps = new List<Vector2Int>();

        int currentInstruction = 0;
        bool finished = currentInstruction >= stepsChosen.Length;
        Vector2Int currentPosition = Vector2Int.zero;
        steps.Add(sequencePuzzle.startPosition);

        while (!finished)
        {
            currentPosition = steps.Last();
            Vector2Int nextPosition = currentPosition;
            switch (stepsChosen[currentInstruction])
            {
                case NOTHING:
                    continue;
                case GO_RIGHT:
                    nextPosition += Vector2Int.right;
                    break;
                case GO_LEFT:
                    nextPosition += Vector2Int.left;
                    break;
                case GO_DOWN:
                    nextPosition += Vector2Int.up;
                    break;
                case GO_UP:
                    nextPosition += Vector2Int.down;
                    break;
            }

            print($"Sequence: {nextPosition.x}, {nextPosition.y}");
            print($"Current instruction: {currentInstruction}");
            if(nextPosition.y >= sequencePuzzle.verticalSize)
            {
                currentInstruction++;
            }
            else if(nextPosition.y < 0)
            {
                currentInstruction++;
            }
            else if (sequencePuzzle.floor[(nextPosition.y * sequencePuzzle.horizontalSize) + nextPosition.x])
            {
                print("Added movement");
                steps.Add(nextPosition);
            }
            else
            {
                currentInstruction++;
            }

            finished = currentInstruction >= stepsChosen.Length;
        }

        return steps.ToArray();
    }
}
