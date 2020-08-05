//using Microsoft.AspNetCore.SignalR.Client;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class JoinLobby : MonoBehaviour
{
    public InputField codeInput;
    public Button sendToServer;

    void OnJoinedLobby()
    {
        gameObject.SetActive(false);
        sendToServer.interactable = false;
    }

    public async void TryToJoinLobby()
    {
        var lobby = new Models.JoinLobby(codeInput.text);

        //await Socket.Instance?.connection.InvokeAsync("EnterLobby", lobby);
    }
}
