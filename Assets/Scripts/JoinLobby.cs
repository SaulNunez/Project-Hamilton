using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class JoinLobby : MonoBehaviour
{
    public InputField codeInput;
    public Button sendToServer;

    private void Start()
    {
        Socket.LobbyJoined += OnJoinedLobby;
    }

    private void OnDestroy()
    {
        Socket.LobbyJoined -= OnJoinedLobby;
    }

    void OnJoinedLobby()
    {
    }

    public void TryToJoinLobby()
    {
        Socket.Instance.EnterLobby(codeInput.text);
        sendToServer.interactable = false;
    }
}
