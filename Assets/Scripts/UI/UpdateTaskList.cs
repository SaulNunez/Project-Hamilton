using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Can be added anywhere accepts a Text to update task list 
/// </summary>
public class UpdateTaskList : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI updateTask;

    private void Start()
    {
        Emergency.OnTimeRemaingForEmergencyChanged += OnEmergencyCountdownUpdate;
    }

    private void OnEmergencyCountdownUpdate(int timeRemaining)
    {
        updateTask.text = $"Emergencia, {timeRemaining} segundos restantes";
    }

    private void OnDestroy()
    {
        Emergency.OnTimeRemaingForEmergencyChanged -= OnEmergencyCountdownUpdate;
    }

}
