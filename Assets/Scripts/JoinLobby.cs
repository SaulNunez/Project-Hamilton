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
        gameObject.SetActive(false);
        if(error != null)
        {
            sendToServer.interactable = true;
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
