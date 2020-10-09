using Assets.Scripts.Multiplayer.ServerRequestsPayload;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;

public class PuzzleManager : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void ShowEditor(string initialStateXml,
        string instructions, string documentation,
        string gameobjectWithCallback, string functionCallbackName);

    [DllImport("__Internal")]
    private static extern void SetErrorInCodeEditor(string errors);

    [DllImport("__Internal")]
    private static extern void SetOutputInEditor(string output);

    [DllImport("__Internal")]
    private static extern void CloseCodeEditor();

    private void Start() => HamiltonHub.Instance.OnNeedToSolvePuzzle += StartPuzzle;
    private void OnDestroy() => HamiltonHub.Instance.OnNeedToSolvePuzzle -= StartPuzzle;

    private string currentPuzzleId = null;

    public UnityEvent onPuzzleCorrect;

    public void StartPuzzle(ShowPuzzleRequestPayload showPuzzleRequestPayload)
    {
        currentPuzzleId = showPuzzleRequestPayload.PuzzleId;

        ShowEditor(
            showPuzzleRequestPayload.InitialWorkspaceConfiguration ?? null,
            showPuzzleRequestPayload.Instructions,
            showPuzzleRequestPayload.Documentation,
            name,
            "SendCodeToServer"
        );
    }

    public async void SendCodeToServer(string code)
    {
        var result = await HamiltonHub.Instance.GetPuzzleResult(code, currentPuzzleId);

        SetOutputInEditor(result.Output);

        if (result.Correct)
        {
            CloseCodeEditor();
        }
    }
}
