using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILobbyConnect : MonoBehaviour
{
    [SerializeField]
    private NetworkManager networkManager;

    public void LookForGameOnServer()
    {
        networkManager.StartClient();
    }
}
