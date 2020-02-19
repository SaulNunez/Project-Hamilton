using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Socket
{
    public Socket()
    {
        
    }

    public void Receive(string eventType, string content)
    {
        onReceive.Invoke(eventType, content);
    }

    public delegate void OnReceive(string eventType, string content);
    public event OnReceive onReceive;

    public void Send(string eventType, string content)
    {

    }
}
