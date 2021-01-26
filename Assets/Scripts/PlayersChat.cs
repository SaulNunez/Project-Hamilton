using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;
using System;

public class PlayersChat : NetworkBehaviour
{
    [SerializeField] 
    private TMP_Text chatBox;
    [SerializeField] 
    private TMP_InputField messageBox;

    public static event Action<string, string> onNewMessage;

    public override void OnStartClient()
    {
        onNewMessage += PrintOnScreen;
    }

    public void PrintOnScreen(string userName, string message)
    {
        messageBox.text += $"[{userName}]: {message}\n";
    }

    [Client]
    public void SendMessageOnEnter()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            CmdTransmitMessage("a",messageBox.text);
            //Clean textbox;
            messageBox.text = string.Empty;
        }
    }

    [Client]
    public void SendMessage()
    {
        CmdTransmitMessage("a", messageBox.text);
        //Clean textbox;
        messageBox.text = string.Empty;
    }

    [Command]
    public void CmdTransmitMessage(string userName, string message)
    {
        PrintMessage(userName, message);
    }

    [ClientRpc]
    public void PrintMessage(string userName, string message)
    {
        onNewMessage?.Invoke(userName, message);
    }

    public override void OnStopAuthority()
    {
        onNewMessage -= PrintOnScreen;
    }
}
