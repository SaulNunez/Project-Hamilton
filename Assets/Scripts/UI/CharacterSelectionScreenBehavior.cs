using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectionScreenBehavior : MonoBehaviour
{
    public GameObject playerSelectionScreen;
    public GameObject joinLobbyScreen;

    void Start()
    {
        HamiltonHub.Instance.onEnteredLobby += OpenPlayerSelection;

        joinLobbyScreen.SetActive(true);
    }

    private void OnDestroy()
    {
        HamiltonHub.Instance.onEnteredLobby -= OpenPlayerSelection;
    }

    private void OpenPlayerSelection(string lobbyCode)
    {
        print("Showing player screen");
        joinLobbyScreen.SetActive(false);
        playerSelectionScreen.SetActive(true);

    }
}
