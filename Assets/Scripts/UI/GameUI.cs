using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField]
    Button interactionButton;

    [SerializeField]
    Button killButton;

    [SerializeField]
    Button sabotageButton;

    [SerializeField]
    Button reportButton;

    [SerializeField]
    Button ghostButton;

    public bool InteractionEnabled {
        get => interactionButton.interactable;

        set => interactionButton.interactable = value;
    }

    private bool enableAssasinExtras = false;
    public bool EnableAssasinExtras
    {
        get => enableAssasinExtras;

        set
        {
            enableAssasinExtras = value;
            killButton.gameObject.SetActive(value);
            sabotageButton.gameObject.SetActive(value);
            ghostButton.gameObject.SetActive(value);
        }
    }

    public bool CanInteractWithKillButton {
        get => killButton.interactable;

        set => killButton.interactable = value;
    }

    public bool CanInteractWithGhostingButton
    {
        get => ghostButton.interactable;

        set => ghostButton.interactable = value;
    }

    public bool CanInteractWithReportButton
    {
        get => reportButton.interactable;

        set => reportButton.interactable = value;
    }

    public static GameUI Instance = null;

    public static event Action onGeneralClick;

    public static event Action onKillButtonClick;

    public static event Action OnReportClick;

    /// <summary>
    /// Invoked when player clicks button to start ghost mode
    /// </summary>
    public static event Action OnGhostingInvoked;

    void Start() {
        if(Instance == null){
            Instance = this;
        }

        interactionButton.onClick.AddListener(GeneralClick);
        killButton.onClick.AddListener(KillClick);
        reportButton.onClick.AddListener(ReportClick);
        ghostButton.onClick.AddListener(GhostModeActivationClick);
    }

    void GeneralClick(){
        onGeneralClick?.Invoke();
    }

    void KillClick(){
        onKillButtonClick?.Invoke();
    }

    void ReportClick() => OnReportClick?.Invoke();

    void GhostModeActivationClick() => OnGhostingInvoked?.Invoke();

    void OnDestroy(){
        Instance = null;

        interactionButton.onClick.RemoveListener(GeneralClick);
        killButton.onClick.RemoveListener(KillClick);
        reportButton.onClick.RemoveListener(ReportClick);
        ghostButton.onClick.RemoveListener(GhostModeActivationClick);
    }
}
