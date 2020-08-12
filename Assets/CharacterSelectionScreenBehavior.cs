using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectionScreenBehavior : MonoBehaviour
{
    public GameObject playerSelectionScreen;

    void Start()
    {
        Socket.Instance.LobbyJoinedEvent += OpenPlayerSelection;
    }

    private void OnDestroy()
    {
        Socket.Instance.LobbyJoinedEvent -= OpenPlayerSelection;
    }

    private void OpenPlayerSelection(LobbyJoinedData data, string error = null)
    {
        if(error == null)
        {
            playerSelectionScreen.SetActive(true);
            enabled = false;
        }
    }
}
