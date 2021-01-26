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

    public enum InteractionType {
        USE,
        SABOTAGE,
        ESCAPE
    }

    public InteractionType Type {
        get; set;
    }

    public bool InteractionEnabled {
        get => interactionButton.interactable;

        set => interactionButton.interactable = value;
    }

    public bool EnableKillButton{
        get => killButton.gameObject.activeSelf;

        set => killButton.gameObject.SetActive(value);
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
