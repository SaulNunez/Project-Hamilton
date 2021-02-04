using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class UpdateTimeRemaining : MonoBehaviour
{
    [SerializeField]
    TMP_Text text;

    private void Start()
    {
        VotingManager.CurrentTimeRemaining += UpdateCounter;
    }

    private void UpdateCounter(int timeRemaining)
    {

        text.text = $"Tiempo faltante: {timeRemaining}";
    }

    private void OnDestroy()
    {
        VotingManager.CurrentTimeRemaining -= UpdateCounter;
    }
}
