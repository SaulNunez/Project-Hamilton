using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    [SyncVar(hook = nameof(AllTasksUpdated))]
    int allTasks;

    private void DoneTasksUpdated(int _, int newValue)
    {
        taskText.text = $"{doneTasks}/{allTasks}";
    }

    private void AllTasksUpdated(int _, int newValue)
    {
        taskText.text = $"{doneTasks}/{allTasks}";
    }

    public override void OnStartServer()
    {
        base.OnStartServer();

        PuzzleCompletion.OnPuzzleCompleted += UpdatePuzzleCount;
        allTasks = PuzzleCompletion.instance.PuzzlesAvailable;
    }

    private void UpdatePuzzleCount(PuzzleId id, NetworkIdentity doneBy)
    {
        doneTasks++;
    }

    public override void OnStopServer()
    {
        base.OnStopServer();

        PuzzleCompletion.OnPuzzleCompleted -= UpdatePuzzleCount;
    }
}
