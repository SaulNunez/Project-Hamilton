using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class Socket: MonoBehaviour
{
    public static Socket Instance;

    Uri u = new Uri(Application.absoluteURL);

    private void Start()
    {
       
        Instance = this;
    }

    public void Receive(string eventType, string content)
    {
        Debug.Log($"Received event {eventType} with: {content}");
        listeners
            .FindAll(listener => listener.eventToListen == eventType).ForEach(listener => listener.listener(content));
    }

    public void OnConnect()
    {
        onConnected?.Invoke();
    }

    public void OnDisconnect(string reason)
    {
        onDisconnected?.Invoke(reason);
    }

    public delegate void OnConnected();
    public event OnConnected onConnected;

    public delegate void OnDisconnected(string reason);
    public event OnDisconnected onDisconnected;

    public delegate void OnReceive(string eventType, string content);
    public event OnReceive onReceive;

    private List<SocketEventListenerInfo> listeners = new List<SocketEventListenerInfo>();

    public void Send(string eventType, string content)
    {
        
    }

    public void RegisterListener(MonoBehaviour component, 
        Action<string> listener, string eventToListen)
    {
        listeners.Add(new SocketEventListenerInfo(component, listener, eventToListen));
        
    }

    public void RemoveListener(Action<string> listener)
    {
        listeners.RemoveAll(listenerItem => listenerItem.listener == listener);
    }

    public void RemoveAllGameObjectListeners(GameObject gameObject)
    {
        listeners.RemoveAll(listener => listener.component == gameObject);
    }

    private void OnDestroy()
    {
        
    }

    struct SocketEventListenerInfo
    {
        public SocketEventListenerInfo(MonoBehaviour component,
            Action<string> listener, string eventToListen)
        {
            this.component = component;
            this.listener = listener;
            this.eventToListen = eventToListen;
        }

        public MonoBehaviour component;
        public Action<string> listener;
        public string eventToListen;
    }
}
