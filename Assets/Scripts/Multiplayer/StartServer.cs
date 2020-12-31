using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
