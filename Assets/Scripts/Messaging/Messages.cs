using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Messages : NetworkBehaviour
{
    [SerializeField]
    TextMeshProUGUI messageBox;

    [SerializeField]
    [Tooltip("How much time to show a new message on screen before dissapearing, in seconds")]
    [Range(1, 100)]
    public int timeOnScreen;

    public static Messages instance;

    public override void OnStartServer()
    {
        base.OnStartServer();

        if(instance == null)
        {
            instance = this;
        }
    }

    [Server]
    public void ShowMessageIndividual(NetworkConnection networkConnection, string message)
    {
        TargetSendMessage(networkConnection, message);
    }

    [Server]
    public void ShowMessageToAll(string message)
    {
        RpcSendMessage(message);
    }

    [TargetRpc]
    void TargetSendMessage(NetworkConnection target, string message)
    {
        ShowMessage(message);
    }

    [ClientRpc]
    void RpcSendMessage(string message)
    {
        ShowMessage(message);
    }

    [Client]
    void ShowMessage(string message)
    {
        CancelInvoke(nameof(ClearMessageBox));
        messageBox.text = message;

        Invoke(nameof(ClearMessageBox), timeOnScreen);
    }

    private void ClearMessageBox()
    {
        messageBox.text = "";
    }
}
