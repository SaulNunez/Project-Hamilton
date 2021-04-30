using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Starts server automatically when game is started unnatended (with flag -batchmode) 
/// </summary>
[RequireComponent(typeof(NetworkManager))]
public class StartServer : MonoBehaviour
{
    void Start()
    {
        if (Application.isBatchMode)
        {
            var networkManager = GetComponent<NetworkManager>();
            networkManager.StartServer();
        }
    }
}
