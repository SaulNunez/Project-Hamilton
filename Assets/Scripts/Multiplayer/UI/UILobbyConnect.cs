using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILobbyConnect : MonoBehaviour
{

    public void LookForGameOnServer()
    {
        NetworkManager.singleton.StartClient();
    }
}
