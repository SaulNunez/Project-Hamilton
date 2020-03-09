using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;

public class Socket: MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void StartSocket(string gameObjectToCall, string connectMethod, string disconnectMethod);

    [DllImport("__Internal")]
    private static extern void EndConnection();

    [DllImport("__Internal")]
    private static extern void SendToSocket(string eventDesriptor, string data);

    [DllImport("__Internal")]
    private static extern void AddEvent(string eventDescriptor, string handlerMethod);

    public static Socket Instance;

    private void Start()
    {
        StartSocket(gameObject.name, "OnConnect", "OnDisconnect");

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
        SendToSocket(eventType, content);
    }

    public void RegisterListener(GameObject gameObject, 
        Action<string> listener, string eventToListen)
    {
        listeners.Add(new SocketEventListenerInfo(gameObject, listener, eventToListen));
        AddEvent(eventToListen, "Receive");
    }

    public void RemoveListener(Action<string> listener)
    {
        listeners.RemoveAll(listenerItem => listenerItem.listener == listener);
    }

    public void RemoveAllGameObjectListeners(GameObject gameObject)
    {
        listeners.RemoveAll(listener => listener.gameObject == gameObject);
    }

    private void OnDestroy()
    {
        EndConnection();
    }

    struct SocketEventListenerInfo
    {
        public SocketEventListenerInfo(GameObject gameObject,
            Action<string> listener, string eventToListen)
        {
            this.gameObject = gameObject;
            this.listener = listener;
            this.eventToListen = eventToListen;
        }

        public GameObject gameObject;
        public Action<string> listener;
        public string eventToListen;
    }
}
