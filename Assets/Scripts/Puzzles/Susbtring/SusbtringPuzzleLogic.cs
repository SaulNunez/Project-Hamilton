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
    int expectedLenghtValue;

    /// <summary>
    /// Current value of start value, set on client when editing
    /// </summary>
    int currentStartValue;
    /// <summary>
    /// Current value of end value, set on client when editing
    /// </summary>
    int currentLenght;

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
        endSlider.minValue = 1;
        endSlider.maxValue = defaultText.Length;
        endSlider.value = 1;
    }

    [Client]
    private void OnEndSliderChanged(float value)
    {
        currentLenght = (int)value;

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
        var preSusbtring = defaultText.Substring(0, Mathf.Clamp(currentStartValue, 0, int.MaxValue));
        var substring = defaultText.Substring(currentStartValue, Mathf.Clamp(currentLenght, 0, defaultText.Length - currentStartValue));
        var postSubstring = defaultText.Substring(currentStartValue+currentLenght);

        substringPlaygroundText.text = $"{preSusbtring}<b>{substring}</b>{postSubstring}";
    }

    [Client]
    void CheckValues()
    {
        print("Send verificatrion");
        CmdCheckIsCorrect(currentStartValue, currentLenght);
    }
    
    [Command(ignoreAuthority = true)]
    void CmdCheckIsCorrect(int start, int end, NetworkConnectionToClient sender = null)
    {
        print($"Check, start: {start}, end: {end}");
        if(start == setStartValue && end == expectedLenghtValue)
        {
            PuzzleCompletion.instance.MarkCompleted(PuzzleId.Substring, sender.identity);
            TargetClosePuzzle(sender);
        }
    }

    [TargetRpc]
    void TargetClosePuzzle(NetworkConnection target)
    {
        PuzzleUI.instance.ClosePuzzles();
    }

    public override void OnStartServer()
    {
        base.OnStartServer();

        setStartValue = Random.Range(0, defaultText.Length - 1);

        expectedLenghtValue = Random.Range(0, defaultText.Length-setStartValue);

        selectValue = defaultText.Substring(setStartValue, expectedLenghtValue);

        print($"Substring puzzle: start value {setStartValue}, lenght {expectedLenghtValue}, selectValue {selectValue}");
    }
}
