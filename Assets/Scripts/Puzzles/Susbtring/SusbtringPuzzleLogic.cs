using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SusbtringPuzzleLogic : NetworkBehaviour
{
    /// <summary>
    /// Set on server, value expected for start slider
    /// </summary>
    [SyncVar]
    int setStartValue;
    /// <summary>
    /// Set on server, value expected for end slider
    /// </summary>
    [SyncVar]
    int setEndValue;

    /// <summary>
    /// Current value of start value, set on client when editing
    /// </summary>
    int currentStartValue;
    /// <summary>
    /// Current value of end value, set on client when editing
    /// </summary>
    int currentEndValue;

    /// <summary>
    /// Substring expected. Used for displaying on clients
    /// </summary>
    [SyncVar(hook = nameof(OnSelectedValueSet))]
    string selectValue;

    [Tooltip("Text to use to substring")]
    [SerializeField]
    [TextArea]
    string defaultText = "Hola mundo";

    [Tooltip("Instructions to what substring. Use {0} to insert expected substring into instructions.")]
    [SerializeField]
    [TextArea]
    string instructionText;

    [Tooltip("Color to use in UI to highlight substring")]
    [SerializeField]
    Color substringColorInUi;

    [Tooltip("Slider to use to set start value of substring function")]
    [Header("UI Elements")]
    [SerializeField]
    Slider startSlider;

    [Tooltip("Slider to use to set end value of substring function")]
    [SerializeField]
    Slider endSlider;

    [Tooltip("Text used to set the instructions of the expected substring")]
    [SerializeField]
    TextMeshProUGUI instructionsText;

    [Tooltip("Text to show player how the input affect the value")]
    [SerializeField]
    TextMeshProUGUI substringPlaygroundText;


    void OnSelectedValueSet(string oldValue, string newValue)
    {
        instructionsText.text = string.Format(instructionText, newValue);
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        startSlider.onValueChanged.AddListener(OnStartSliderChanged);
        startSlider.maxValue = defaultText.Length;

        endSlider.onValueChanged.AddListener(OnEndSliderChanged);
        endSlider.maxValue = defaultText.Length;
    }

    [Client]
    private void OnEndSliderChanged(float value)
    {
        currentEndValue = (int)value;

        UpdateScreen();
        CheckValues();
    }

    [Client]
    private void OnStartSliderChanged(float value)
    {
        currentStartValue = (int)value;

        UpdateScreen();
        CheckValues();
    }

    [Client]
    private void UpdateScreen()
    {
        var preSusbtring = defaultText.Substring(0, currentStartValue);
        var substring = defaultText.Substring(currentStartValue, currentEndValue);
        var postSubstring = defaultText.Substring(currentEndValue);

        substringPlaygroundText.text = $"{preSusbtring}<color={ColorUtility.ToHtmlStringRGBA(substringColorInUi)}>{substring}</color>{postSubstring}";
    }

    [Client]
    void CheckValues()
    {
        CmdCheckIsCorrect(currentStartValue, currentEndValue);
    }
    
    [Command]
    void CmdCheckIsCorrect(int start, int end, NetworkConnectionToClient sender = null)
    {
        if(start == setStartValue && end == setEndValue)
        {
            PuzzleCompletion.instance.MarkCompleted(PuzzleId.Substring);
            TargetClosePuzzle(sender);
        }
    }

    [TargetRpc]
    void TargetClosePuzzle(NetworkConnection target)
    {
        ShowPuzzle.instance.ClosePuzzles();
    }

    public override void OnStartServer()
    {
        base.OnStartServer();

        setStartValue = Random.Range(0, defaultText.Length - 1);

        setEndValue = Random.Range(setStartValue, defaultText.Length);

        selectValue = defaultText.Substring(setStartValue, setEndValue);
    }
}
