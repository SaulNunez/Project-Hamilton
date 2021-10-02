using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CompletingForPuzzle : PuzzleBase
{
    const int MORE_THAN = 0;
    const int MORE_OR_EQUAL = 1;
    const int LESS_OR_EQUAL = 2;
    const int LESS_THAN = 3;
    const int EQUAL = 4;

    [SerializeField]
    int expectedIterations = 0;

    [Header("UI")]
    [SerializeField]
    TMP_InputField initialValueInputField;

    [SerializeField]
    TMP_InputField endVerificationValueInputField;

    [SerializeField]
    TMP_Dropdown operatorDropdownUi;

    [Header("Execution settings")]
    [SerializeField]
    int stopVerificationAtIteration = 50;

    // Client variables
    int currentStartCount = 0;
    int currentEndVerificationValue = 0;
    int operatorSelected = 0;

    public override void OnStartClient()
    {
        base.OnStartClient();

        operatorDropdownUi.onValueChanged.AddListener(SetCompareType);
        initialValueInputField.onValueChanged.AddListener(SetStartCount);
        endVerificationValueInputField.onValueChanged.AddListener(SetEndVerification);
    }

    void SetStartCount(string value)
    {
        try
        {
            currentStartCount = int.Parse(value);
        } catch (Exception)
        {

        }
    }

    void SetEndVerification(string value)
    {
        try
        {
            currentEndVerificationValue = int.Parse(value);
        } catch (Exception)
        {

        }
    }

    void SetCompareType(int selectedOperatorIndex)
    {
        operatorSelected = selectedOperatorIndex;
    }

    public void Verify()
    {
        CmdVerification(currentStartCount, currentEndVerificationValue, operatorSelected);
    }

    [Command(requiresAuthority = false)]
    void CmdVerification(int startCount, int endVerification, int compare, NetworkConnectionToClient sender = null)
    {
        int iterations = 0;
        switch (compare)
        {
            case MORE_THAN:
                for(int i = startCount; i > endVerification; i++)
                {
                    iterations++;
                    if(iterations >= stopVerificationAtIteration)
                    {
                        break;
                    }
                }
                break;
            case MORE_OR_EQUAL:
                for (int i = startCount; i >= endVerification; i++)
                {
                    iterations++;
                    if (iterations >= stopVerificationAtIteration)
                    {
                        break;
                    }
                }
                break;
            case LESS_OR_EQUAL:
                for (int i = startCount; i <= endVerification; i++)
                {
                    iterations++;
                    if (iterations >= stopVerificationAtIteration)
                    {
                        break;
                    }
                }
                break;
            case LESS_THAN:
                for (int i = startCount; i < endVerification; i++)
                {
                    iterations++;
                    if (iterations >= stopVerificationAtIteration)
                    {
                        break;
                    }
                }
                break;
            case EQUAL:
                for (int i = startCount; i == endVerification; i++)
                {
                    iterations++;
                    if (iterations >= stopVerificationAtIteration)
                    {
                        break;
                    }
                }
                break;
        }

        if(iterations == expectedIterations)
        {
            PuzzleCompletion.instance.MarkCompleted(PuzzleId.IfFlowerPicking, sender.identity);
            TargetClosePuzzle(sender);
            TargetRunCorrectFeedback(sender);
        }
        else
        {
            TargetRunWrongFeedback(sender);
        }
    }
}
