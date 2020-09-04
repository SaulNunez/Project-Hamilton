using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectionScreenBehavior : MonoBehaviour
{
    public GameObject playerSelectionScreen;
    public GameObject joinLobbyScreen;

    void Start()
    {
        Socket.LobbyJoined += OpenPlayerSelection;

        joinLobbyScreen.SetActive(true);
    }

    private void OnDestroy()
    {
        Socket.LobbyJoined -= OpenPlayerSelection;
    }

    private void OpenPlayerSelection(LobbyJoinedData data, string error = null)
    {
        print($"Hey, data: ${data}, error: ${error}");
        if(error == null)
        {
            print("Showing player screen");
            joinLobbyScreen.SetActive(false);
            playerSelectionScreen.SetActive(true);
        }
    }
}
