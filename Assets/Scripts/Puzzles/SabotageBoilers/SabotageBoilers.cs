using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

/// <summary>
/// 
/// </summary>
public class SabotageBoilers : NetworkBehaviour
{
    [SerializeField]
    Image gaugeImage;

    [Header("Setups")]
    [SerializeField]
    List<BoilerSabotageOptions> gaugesSetups;

    [SyncVar]
    int minimum;

    [SyncVar]
    int maximum;

    [SyncVar(hook = nameof(SetupScreen))]
    int setupUsed;

    public override void OnStartServer()
    {
        base.OnStartServer();

        setupUsed = Random.Range(0, gaugesSetups.Count);
    }

    void SetupScreen(int oldValue, int newValue)
    {
        gaugeImage.sprite = gaugesSetups[newValue].gauge;
    }

    [Client]
    public void SetMinimum(string input)
    {
        try
        {
            var processedInput = int.Parse(input);
            CmdUpdateMinimum(processedInput);
        }catch(Exception e)
        {
            Debug.LogError(e);
        }
    }

    [Command]
    void CmdUpdateMinimum(int min)
    {
        minimum = min;

        CheckCorrect();
    }

    [Client]
    public void SetMaximum(string input)
    {
        try
        {
            var processedInput = int.Parse(input);
            CmdUpdateMaximum(processedInput);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    [Command]
    void CmdUpdateMaximum(int max)
    {
        maximum = max;

        CheckCorrect();
    }

    void CheckCorrect()
    {
        var isCorrect = false;

        if(maximum <= gaugesSetups[setupUsed].maximum && minimum >= gaugesSetups[setupUsed].minimum)
        {
            isCorrect = true;
        }

        if (isCorrect)
        {
            PuzzleCompletion.instance.MarkCompleted(PuzzleId.SabotageBoilerPressure);
            RpcClosePuzzle();
        }
    }

    [ClientRpc]
    void RpcClosePuzzle()
    {
        gameObject.SetActive(false);
    }
}
