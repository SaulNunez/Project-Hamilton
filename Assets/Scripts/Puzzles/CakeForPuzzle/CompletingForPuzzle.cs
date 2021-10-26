using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CompletingForPuzzle : PuzzleBase
{
    [Header("Puzzle information")]

    [SerializeField]
    PuzzleId puzzleId;

    [SerializeField]
    int expectedIterations = 0;

    [Header("UI")]
    [SerializeField]
    TMP_InputField initialValueInputField;

    [SerializeField]
    TMP_InputField endVerificationValueInputField;

    [SerializeField]
    TMP_InputField stepInputField;

    [Header("Execution settings")]
    [SerializeField]
    int stopVerificationAtIteration = 50;

    // Client variables
    int currentStartCount = 0;
    int currentEndVerificationValue = 0;
    int stepCount = 0;

    public override void OnStartClient()
    {
        base.OnStartClient();

        stepInputField.onValueChanged.AddListener(SetSteps);
        initialValueInputField.onValueChanged.AddListener(SetStartCount);
        endVerificationValueInputField.onValueChanged.AddListener(SetEndVerification);
    }

    void SetStartCount(string value)
    {
        try
        {
            currentStartCount = int.Parse(value);
        }
        catch (Exception)
        {

        }
    }

    void SetEndVerification(string value)
    {
        try
        {
            currentEndVerificationValue = int.Parse(value);
        }
        catch (Exception)
        {

        }
    }

    void SetSteps(string value)
    {
        try
        {
            stepCount = int.Parse(value);
        } catch (Exception)
        {

        }
    }


    public void Verify()
    {
        CmdVerification(currentStartCount, currentEndVerificationValue, stepCount);
    }

    [Command(requiresAuthority = false)]
    void CmdVerification(int startCount, int endVerification, int steps, NetworkConnectionToClient sender = null)
    {
        int iterations = 0;

        for (int i = startCount; i <= endVerification; i+=steps)
        {
            iterations++;
            if (iterations >= stopVerificationAtIteration)
            {
                break;
            }
        }

        if (iterations == expectedIterations)
        {
            PuzzleCompletion.instance.MarkCompleted(puzzleId, sender.identity);
            TargetClosePuzzle(sender);
            TargetRunCorrectFeedback(sender);
        }
        else
        {
            TargetRunWrongFeedback(sender);
        }
    }
}
