﻿using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WhilePuzzle : PuzzleBase
{
    [Tooltip("Default value to ask users to input")]
    [SerializeField]
    int defaultBucketValue = 6;

    [Tooltip("InputField to use to get the expected value to solve the puzzle")]
    [SerializeField]
    TMP_InputField input;

    public override void OnStartClient()
    {
        base.OnStartClient();

        input.onEndEdit.AddListener(CheckInput);
    }

    public void CheckInput(string input)
    {
        try
        {
            var turnsInput = int.Parse(input);
            CmdCheckInput(turnsInput);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    [Command(requiresAuthority = false)]
    void CmdCheckInput(int input, NetworkConnectionToClient sender = null)
    {
        if(input == defaultBucketValue)
        {
            PuzzleCompletion.instance.MarkCompleted(PuzzleId.WhileFillingBucket, sender.identity);
            TargetClosePuzzle(sender);
            TargetRunCorrectFeedback(sender);
        } else
        {
            TargetRunWrongFeedback(sender);
        }
    }


}
