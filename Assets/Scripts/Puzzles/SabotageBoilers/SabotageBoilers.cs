﻿using Mirror;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
public class SabotageBoilers : SabotagePuzzle
{
    [SerializeField]
    SpecialButton buttonToClick;

    [Range(0.01f, 10f)]
    [Tooltip("Delay between a button release and when we will remove it from the button pressing players list")]
    [SerializeField]
    float stickynessInServer = 2f;

    /// <summary>
    /// Text in button
    /// </summary>
    /// <remarks>
    /// Only available on server
    /// </remarks>
    TextMeshProUGUI buttonToClickText;

    public string waitingOnPlayerPress;
    public string waitingOnOtherPlayers;

    public struct PlayerStatus
    {
        public NetworkIdentity playerOnButton;
        public DateTime startClick;
    }

    readonly SyncList<PlayerStatus> playersOnButton = new SyncList<PlayerStatus>();

    protected override bool AreEmergencyConditionsEnough(Emergency.EmergencyType type) =>
    type == Emergency.EmergencyType.ChangeBoilerPressure;

    public override void OnStartClient()
    {
        base.OnStartClient();
        buttonToClick.onPressedStarted.AddListener(StartPress);
        buttonToClick.onPressedStarted.AddListener(ClientStartPress);
        buttonToClick.onPressedEnded.AddListener(EndPress);
        buttonToClick.onPressedEnded.AddListener(ClientOnEndPress);

        buttonToClickText = buttonToClick.GetComponentInChildren<TextMeshProUGUI>();
        buttonToClickText.text = waitingOnPlayerPress;
    }

    public override void OnStopClient()
    {
        base.OnStopClient();
        buttonToClick.onPressedStarted.RemoveListener(StartPress);
        buttonToClick.onPressedStarted.RemoveListener(ClientStartPress);
        buttonToClick.onPressedEnded.RemoveListener(EndPress);
        buttonToClick.onPressedEnded.RemoveListener(ClientOnEndPress);
    }

    void StartPress() => CmdSetAsPressedButton();
    void EndPress() => CmdSetDepressedButton();

    void ClientStartPress()
    {
        buttonToClickText.text = waitingOnOtherPlayers;
    }

    void ClientOnEndPress()
    {
        buttonToClickText.text = waitingOnPlayerPress;
    }

    [Command(requiresAuthority = false)]
    void CmdSetAsPressedButton(NetworkConnectionToClient sender = null)
    {
        playersOnButton.Add(new PlayerStatus
        {
            playerOnButton = sender.identity,
            startClick = DateTime.Now,
        });

        if(playersOnButton.Count >= 2)
        {
            foreach(var player in playersOnButton)
            {
                SetPuzzleAsCompleted(player.playerOnButton.connectionToClient);
            }
        }
    }

    [Command(requiresAuthority = false)]
    void CmdSetDepressedButton(NetworkConnectionToClient sender = null)
    {
        var coroutine = OnButtonDepressedServer(sender);
        StartCoroutine(coroutine);
    }

    [Server]
    IEnumerator OnButtonDepressedServer(NetworkConnectionToClient sender)
    {
        yield return new WaitForSecondsRealtime(stickynessInServer);
        playersOnButton.RemoveAll(p => p.playerOnButton == sender.identity);
    }

    protected override bool ArePuzzleCompletionConditionsReached()
    {
        return playersOnButton.Count >= 2;
    }

    protected override void OnPuzzleCompleted()
    {
        base.OnPuzzleCompleted();
        playersOnButton.Clear();
    }
}
