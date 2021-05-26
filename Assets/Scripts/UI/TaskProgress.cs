using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Updates task progress bar in client
/// </summary>
public class TaskProgress : NetworkBehaviour
{
    [Tooltip("Slider that doubles duty as progress bar for tasks in UI")]
    [SerializeField]
    Slider taskSlider;

    [SyncVar(hook = nameof(DoneTasksUpdated))]
    int doneTasks;

    [SyncVar(hook = nameof(AllTasksUpdated))]
    int allTasks;

    private void DoneTasksUpdated(int _, int newValue)
    {
        taskSlider.value = newValue;
    }

    private void AllTasksUpdated(int _, int newValue)
    {
        taskSlider.maxValue = allTasks;
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
