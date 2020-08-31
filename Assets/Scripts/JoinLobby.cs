using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class JoinLobby : MonoBehaviour
{
    public InputField codeInput;
    public Button sendToServer;

    private void Start()
    {
        Socket.Instance.LobbyJoinedEvent += OnJoinedLobby;
    }

    private void OnDestroy()
    {
        Socket.Instance.LobbyJoinedEvent -= OnJoinedLobby;
    }

    void OnJoinedLobby(LobbyJoinedData data, string error = null)
    {
        if(error != null)
        {
            sendToServer.interactable = true;
            Debug.LogError(error);
        }
    }

    public void TryToJoinLobby()
    {
        Socket.Instance.EnterLobby(new EnterLobbyPayload { 
            lobbyCode = codeInput.text 
        });
        sendToServer.interactable = false;
    }
}
