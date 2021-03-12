using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskProgress : NetworkBehaviour
{
    [SerializeField]
    Slider taskSlider;

    [SyncVar(hook = nameof(DoneTasksUpdated))]
    int doneTasks;

    [SyncVar(hook = nameof(AllTasksUpdated))]
    int allTasks;

    private void DoneTasksUpdated(int oldValue, int newValue)
    {
        taskSlider.value = newValue;
    }

    private void AllTasksUpdated(int oldValue, int newValue)
    {
        taskSlider.maxValue = allTasks;
    }

    public override void OnStartServer()
    {
        base.OnStartServer();

        PuzzleCompletion.OnPuzzleCompleted += UpdatePuzzleCount;
        allTasks = Enum.GetNames(typeof(PuzzleId)).Length;
    }

    private void UpdatePuzzleCount(PuzzleId obj)
    {
        doneTasks++;
    }

    public override void OnStopServer()
    {
        base.OnStopServer();

        PuzzleCompletion.OnPuzzleCompleted -= UpdatePuzzleCount;
    }
}
