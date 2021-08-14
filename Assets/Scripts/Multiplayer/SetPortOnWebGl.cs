using Mirror.SimpleWeb;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPortOnWebGl : MonoBehaviour
{
    [SerializeField]
    ushort newPort;

    SimpleWebTransport transport;

    // Start is called before the first frame update
    void Start()
    {
        transport = GetComponent<SimpleWebTransport>();

        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            transport.port = newPort;
        }
    }

}
