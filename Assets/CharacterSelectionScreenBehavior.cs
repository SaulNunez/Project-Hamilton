using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectionScreenBehavior : MonoBehaviour
{
    public GameObject playerSelectionScreen;
    public GameObject joinLobbyScreen;

    void Start()
    {
        Socket.Instance.LobbyJoinedEvent += OpenPlayerSelection;

        joinLobbyScreen.SetActive(true);
    }

    private void OnDestroy()
    {
        Socket.Instance.LobbyJoinedEvent -= OpenPlayerSelection;
    }

    private void OpenPlayerSelection(LobbyJoinedData data, string error = null)
    {
        if(data != null)
        {
            joinLobbyScreen.SetActive(false);
            playerSelectionScreen.SetActive(true);
        }
    }
}
