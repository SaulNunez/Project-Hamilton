using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Extensions;

/// <summary>
/// Updates task progress bar in client
/// </summary>
public class TaskProgress : NetworkBehaviour
{
    [Tooltip("Slider that doubles duty as progress bar for tasks in UI")]
    [SerializeField]
    TextMeshProUGUI taskText;

    [SyncVar(hook = nameof(DoneTasksUpdated))]
    int doneTasks;


    private void DoneTasksUpdated(int oldValue, int newValue)
    {
        taskText.text = $"{doneTasks}/{PuzzleCompletion.instance.PuzzlesAvailable}";
    }

    private void AllTasksUpdated(int oldValue, int newValue)
    {
        taskText.text = $"{doneTasks}/{PuzzleCompletion.instance.PuzzlesAvailable}";
    }

    public override void OnStartServer()
    {
        base.OnStartServer();

        PuzzleCompletion.OnPuzzleCompleted += UpdatePuzzleCount;
    }

    private void UpdatePuzzleCount(PuzzleId id, NetworkIdentity doneBy)
    {
        var assasinComponent = doneBy.GetComponent<Killing>();
        bool isAssasin = assasinComponent != null && assasinComponent.IsAssasin;
        this.SuperPrint($"Puzzle {id} set as completed. Solved by assasin: {isAssasin}");
        if (!isAssasin)
        {
            doneTasks++;
        }
    }

    public override void OnStopServer()
    {
        base.OnStopServer();

        PuzzleCompletion.OnPuzzleCompleted -= UpdatePuzzleCount;
    }
}
