using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeletransportUiConnect : MonoBehaviour
{
    [Tooltip("Panel to show when on teletranport mode")]
    [SerializeField]
    GameObject teletransportUiPanel;

    public void OpenUi()
    {
        teletransportUiPanel.SetActive(true);
    }

    public void HideUi()
    {
        teletransportUiPanel.SetActive(false);
    }
}
