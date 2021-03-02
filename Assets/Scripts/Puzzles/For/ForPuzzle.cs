using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Random = UnityEngine.Random;

public class ForPuzzle : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnTurnsToWashSet))]
    int turnsToWash;

    [SerializeField]
    TextMeshProUGUI instructionTextbox;

    /// <summary>
    /// Instructions to solve puzzle. Will be passed to string.format for setting variable turns.
    /// Use {0} to interpolate turnsToWash.
    /// </summary>
    [TextArea]
    [SerializeField]
    public string instructionText;

    /// <summary>
    /// Called on turnsToWash updated on client (called when screen is started). Updates instruction box to on server decided turnsToWash.
    /// </summary>
    /// <param name="_"></param>
    /// <param name="newValue"></param>
    void OnTurnsToWashSet(int _, int newValue)
    {
        instructionTextbox.text = string.Format(instructionText, newValue);
    }

    public override void OnStartServer()
    {
        base.OnStartServer();

        turnsToWash = Random.Range(5, 45);
    }

    /// <summary>
    /// Exposed to be added on editor to OnEndEdit on the InputField. Takes input and sends it to the server to be checked to be correct.
    /// </summary>
    /// <param name="result">Textbox content</param>
    public void CheckResultIsCorrect(string result)
    {
        try
        {
            var turnsInput = int.Parse(result);
            CmdCheckInputMatchesTurns(turnsInput);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    /// <summary>
    /// Checks client input to match turnsToWash.
    /// </summary>
    /// <param name="input"></param>
    [Command]
    void CmdCheckInputMatchesTurns(int input)
    {
        if(input == turnsToWash)
        {
            PuzzleCompletion.instance.MarkCompleted(PuzzleId.ForWashingBucket);
        }
    }
}
