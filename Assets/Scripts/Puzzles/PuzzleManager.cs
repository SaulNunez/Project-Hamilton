using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;

public class PuzzleManager : NetworkBehaviour
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


    private string currentPuzzleId = null;

    public UnityEvent onPuzzleCorrect;

    public static PuzzleManager Instance = null;

    public override void OnStartClient()
    {
        base.OnStartClient();

        if(Instance == null) Instance = this;
    }

    public void StartPuzzle(ShowPuzzleRequest showPuzzleRequest)
    {
        currentPuzzleId = showPuzzleRequest.PuzzleId;

        ShowEditor(
            showPuzzleRequest.InitialWorkspaceConfiguration ?? null,
            showPuzzleRequest.Instructions,
            showPuzzleRequest.Documentation,
            name,
            "SendCodeToServer"
        );
    }

    [Command]
    public void SendCodeToServer(string code, NetworkConnectionToClient sender = null)
    {
        var puzzleCheckGameObject = GameObject.FindGameObjectWithTag("PuzzleCheck");

        if (puzzleCheckGameObject != null)
        {
            var puzzleCheckSystem = puzzleCheckGameObject.GetComponent<PuzzleChecking>();

            puzzleCheckSystem.CheckPuzzle();
        } else
        {
            
        }
        /*var result = await HamiltonHub.Instance.GetPuzzleResult(code, currentPuzzleId);

        SetOutputInEditor(result.Output);

        if (result.Correct)
        {
            CloseCodeEditor();
        } */
    }

    public override void OnStopClient()
    {
        base.OnStopClient();

        Instance = null;
    }
}
