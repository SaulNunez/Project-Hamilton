using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

/// <summary>
/// Updates a given text with the time remaining for voting. For use with the voting screen.
/// </summary>
public class UpdateTimeRemaining : MonoBehaviour
{
    [TextArea]
    [SerializeField]
    string counterTextTemplate = "Tiempo faltante: {0}";


    [SerializeField]
    TMP_Text text;

    void OnEnable()
    {
        var hubConfigGameObject = GameObject.FindGameObjectWithTag(Tags.HubConfig);
        var hubConfig = hubConfigGameObject.GetComponent<HubConfig>();
        counter = hubConfig.secondsForVoting;
        UpdateCounter();
    }

    /// <summary>
    /// Local countdown timer
    /// </summary>
    private int counter;

    /// <summary>
    /// Recursive function, updates on screen counter, and if counter is still running, setup to be called again in a sec
    /// </summary>
    private void UpdateCounter()
    {
        text.text = string.Format(counterTextTemplate, counter);
        counter--;

        if(counter > 0)
        {
            Invoke(nameof(UpdateCounter), 1f);
        }
    }

}
