using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sets the network addresss in network connection to test server when running on the web 
/// </summary>
[RequireComponent(typeof(NetworkConnection))]
public class SetServerUrlToNetworkManagerOnStart : MonoBehaviour
{
    void Start()
    {
        if(Application.platform == RuntimePlatform.WebGLPlayer)
        {
            var networkManager = GetComponent<NetworkManager>();
            networkManager.networkAddress = "servidorpruebas.saulnunez.com";
        }
    }
}
