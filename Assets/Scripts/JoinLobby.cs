using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class JoinLobby : MonoBehaviour
{
    public InputField codeInput;
    public Button sendToServer;

    public async void TryToJoinLobby()
    {
        sendToServer.interactable = false;
        var connectedToLobby = await HamiltonHub.Instance.ConnectToLobby(codeInput.text);
        sendToServer.interactable = true;
        gameObject.SetActive(!connectedToLobby);
    }
}
