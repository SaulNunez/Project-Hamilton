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
public class SabotageBoilers : SabotagePuzzle
{
    [SerializeField]
    Image gaugeImage;

    [Header("Setups")]
    [SerializeField]
    List<BoilerSabotageOptions> gaugesSetups;

    /// <summary>
    /// Minimum set in the UI
    /// </summary>
    /// <remarks>
    /// Only usable on client
    /// </remarks>
    int minimum;
    
    /// <summary>
    /// Maximum set in the UI
    /// </summary>
    /// <remarks>
    /// Only usable on client
    /// </remarks>
    int maximum;

    [SyncVar(hook = nameof(OnSetupDefined))]
    int setupUsed;

    public override void OnStartServer()
    {
        base.OnStartServer();

        SetNewSetup();
    }


    /// <summary>
    /// To be used to select a new setup. 
    /// Main usage is to set a different gauge setup when it's new sabotage to the boilers.
    /// </summary>
    /// <remarks>
    /// Only to be called on the server
    /// </remarks>
    [Server]
    public void SetNewSetup()
    {
        setupUsed = Random.Range(0, gaugesSetups.Count);
    }

    void OnSetupDefined(int oldValue, int newValue)
    {
        gaugeImage.sprite = gaugesSetups[newValue].gauge;
    }

    /// <summary>
    /// To be used on the client UI to update internal state of the minimum
    /// </summary>
    /// <param name="input">Current text of the UI element that handles minimum</param>
    [Client]
    public void SetMinimum(string input)
    {
        try
        {
            var processedInput = int.Parse(input);
            minimum = processedInput;

            CmdCheckCorrect(minimum, maximum);
        }
        catch(Exception e)
        {
            Debug.LogError(e);
        }
    }

    [Client]
    public void SetMaximum(string input)
    {
        try
        {
            var processedInput = int.Parse(input);
            maximum = processedInput;

            CmdCheckCorrect(minimum, maximum);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    [Command(ignoreAuthority = true)]
    void CmdCheckCorrect(int min, int max, NetworkConnectionToClient sender = null)
    {
        if(max <= gaugesSetups[setupUsed].maximum && min >= gaugesSetups[setupUsed].minimum)
        {
            SetPuzzleAsCompleted(sender);
        }
    }

    protected override void OnPuzzleCompleted()
    {
        base.OnPuzzleCompleted();

        ClosePuzzle();
    }
}
