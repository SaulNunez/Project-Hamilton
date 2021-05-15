using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Shows a quick message to the player
/// </summary>
public class Messages : NetworkBehaviour
{
    [SerializeField]
    TextMeshProUGUI messageBox;

    [SerializeField]
    [Tooltip("How much time to show a new message on screen before dissapearing, in seconds")]
    [Range(1, 100)]
    public int timeOnScreen;

    /// <summary>
    /// Instance of messages, used around all game
    /// </summary>
    public static Messages instance;

    public override void OnStartServer()
    {
        base.OnStartServer();

        if(instance == null)
        {
            instance = this;
        }
    }

    /// <summary>
    /// Show a message to a specific player, invoked from the server
    /// </summary>
    /// <param name="networkConnection">NetworkConnection to player</param>
    /// <param name="message">The message to show</param>
    /// <remarks>Only usable form the server</remarks>
    [Server]
    public void ShowMessageIndividual(NetworkConnection networkConnection, string message)
    {
        TargetSendMessage(networkConnection, message);
    }

    /// <summary>
    /// Show a message globally to the whole game
    /// </summary>
    /// <param name="message">The message to show</param>
    [Server]
    public void ShowMessageToAll(string message)
    {
        RpcSendMessage(message);
    }

    /// <summary>
    /// Used to connect to a specific client
    /// </summary>
    /// <param name="target"></param>
    /// <param name="message">The message to show</param>
    [TargetRpc]
    void TargetSendMessage(NetworkConnection target, string message)
    {
        ShowMessage(message);
    }

    /// <summary>
    /// Used to connect to client to present global message
    /// </summary>
    /// <param name="message">The message to show</param>
    [ClientRpc]
    void RpcSendMessage(string message)
    {
        ShowMessage(message);
    }

    /// <summary>
    /// Show a message locally in this client. Can be used to show something computed on client
    /// </summary>
    /// <param name="message">The message to show</param>
    [Client]
    public void ShowMessage(string message)
    {
        CancelInvoke(nameof(ClearMessageBox));
        messageBox.text = message;

        Invoke(nameof(ClearMessageBox), timeOnScreen);
    }

    /// <summary>
    /// Force clear message box shown on screen on local screen
    /// </summary>
    /// <remarks>
    /// Only usable on client
    /// </remarks>
    private void ClearMessageBox()
    {
        messageBox.text = "";
    }
}
