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
        }
    }

    public bool CanInteractWithKillButton {
        get => killButton.interactable;

        set => killButton.interactable = value;
    }

    public static GameUI Instance = null;

    public static event Action onGeneralClick;

    public static event Action onKillButtonClick;

    void Start() {
        if(Instance == null){
            Instance = this;
        }

        interactionButton.onClick.AddListener(GeneralClick);
        killButton.onClick.AddListener(KillClick);
    }

    void GeneralClick(){
        onGeneralClick?.Invoke();
    }

    void KillClick(){
        onKillButtonClick?.Invoke();
    }

    void OnDestroy(){
        Instance = null;

        interactionButton.onClick.RemoveListener(GeneralClick);
        killButton.onClick.RemoveListener(KillClick);
    }
}
