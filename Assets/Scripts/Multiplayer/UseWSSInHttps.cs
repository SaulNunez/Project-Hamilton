using Mirror.SimpleWeb;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseWSSInHttps : MonoBehaviour
{
    SimpleWebTransport transport;

    void Start()
    {
        transport = GetComponent<SimpleWebTransport>();

        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            var serverUrlUri = new Uri(Application.absoluteURL);

            transport.clientUseWss = serverUrlUri.Scheme == Uri.UriSchemeHttps;
        }
    }
}
