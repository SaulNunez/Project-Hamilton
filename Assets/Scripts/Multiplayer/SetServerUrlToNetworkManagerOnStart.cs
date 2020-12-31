using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetServerUrlToNetworkManagerOnStart : MonoBehaviour
{
    void Start()
    {
        if(Application.platform == RuntimePlatform.WebGLPlayer)
        {
            var networkManager = GetComponent<NetworkManager>();
            var serverUrlUri = new Uri(Application.absoluteURL);
            networkManager.networkAddress = serverUrlUri.GetLeftPart(UriPartial.Authority);
        }
    }
}
