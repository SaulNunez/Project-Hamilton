using Mirror;
using Mirror.SimpleWeb;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EditServerLocation : MonoBehaviour
{
    [SerializeField]
    TMP_InputField serverAddressInput;
    [SerializeField]
    TMP_InputField portInput;

    [SerializeField]
    NetworkManager networkManager;

    [SerializeField]
    SimpleWebTransport transport;

    void Start()
    {
        serverAddressInput.onEndEdit.AddListener(UpdateServerAddress);
        portInput.onEndEdit.AddListener(UpdatePort);

        Invoke(nameof(LoadInfoToUIAfterStart), 0.5f);
    }

    void LoadInfoToUIAfterStart()
    {
        serverAddressInput.text = networkManager.networkAddress;
        portInput.text = transport.port.ToString();
    }

    void UpdateServerAddress(string newValue)
    {
        networkManager.networkAddress = newValue;
    }
    
    void UpdatePort(string newValue)
    {
        if (ushort.TryParse(newValue, out ushort port))
        {
            transport.port = port;
        }
        else
        {
            portInput.text = transport.port.ToString();
        }
    }
}
