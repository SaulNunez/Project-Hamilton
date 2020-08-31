using UnityEngine;
using NativeWebSocket;
using System.Text;
using System;
using Assets.Scripts.Multiplayer.ResultPayload;
using Assets.Scripts.Multiplayer.RequestPayload;

public class Socket : MonoBehaviour
{
    public static Socket Instance;

    WebSocket websocket;

    public void EnterLobby(EnterLobbyPayload payload)
    {
        print("Enter lobby message sent");
        websocket?.SendText(JsonUtility.ToJson(new EnterLobbyRequest
        {
            type = "enter_lobby",
            payload = payload
        }));
    }

    public void GetAvailableCharacters() =>
        websocket?.SendText(JsonUtility.ToJson(
        new AvailableCharactersRequest
        {
            type = "get_available_characters"
        }));

    public void SelectCharacter(SelectCharacterPayload payload) =>
        websocket?.SendText(JsonUtility.ToJson(new SelectCharacterRequest
        {
            type = "select_character",
            payload = payload
        }));

    public delegate void LobbyJoinedDelegate(LobbyJoinedData data, string error = null);
    public event LobbyJoinedDelegate LobbyJoinedEvent;
    public delegate void AvailableCharactersUpdateDelegate(AvailableCharactersData data, string error = null);
    public event AvailableCharactersUpdateDelegate AvailableCharactersUpdateEvent;
    public delegate void PlayerSelectedCharacterDelegate(string error = null);
    public event PlayerSelectedCharacterDelegate PlayerSelectedCharacterEvent;

    private async void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning($"Only a single instance of Socket is needed, killing this.");
            Destroy(this);
        }
        var webHostUrl = new Uri(Application.absoluteURL);

        websocket = WebSocketFactory.CreateInstance($"wss://{webHostUrl.Host}/gameapi");

        websocket.OnOpen += () =>
        {
            Debug.Log("WS connected!");
        };

        websocket.OnMessage += (byte[] msg) =>
        {
            var messageContents = Encoding.UTF8.GetString(msg);

            var messageTypeObj = JsonUtility.FromJson<EventData>(messageContents);

            print($"Message received. Event type {messageTypeObj?.type}");

            switch (messageTypeObj.type)
            {
                case "lobby_joined":
                    InvokeLobbyJoinedEvent(messageContents);
                    break;
                case "available_characters_update":
                    InvokeAvailableCharactersUpdate(messageContents);
                    break;
                case "player_selection_sucess":
                    InvokePlayerSelectionSuccess(messageContents);
                    break;
            }
        };

        websocket.OnError += (string errMsg) =>
        {
            Debug.Log("WS error: " + errMsg);
        };

        websocket.OnClose += (WebSocketCloseCode code) =>
        {
            Debug.Log("WS closed with code: " + code.ToString());
        };

        await websocket.Connect();

    }

    private void InvokePlayerSelectionSuccess(string messageContents)
    {
        var eventData = JsonUtility.FromJson<EventData<PlayerSelectedCharacterData>>(messageContents);
        PlayerSelectedCharacterEvent?.Invoke(eventData.error);
    }

    private void InvokeAvailableCharactersUpdate(string messageContents)
    {
        var eventData = JsonUtility.FromJson<EventData<AvailableCharactersData>>(messageContents);
        if (eventData.error != null)
        {
            AvailableCharactersUpdateEvent?.Invoke(eventData.payload);
        }
        else
        {
            AvailableCharactersUpdateEvent?.Invoke(null, eventData.error);
        }
    }

    private void InvokeLobbyJoinedEvent(string messageContents)
    {
        var eventData = JsonUtility.FromJson<EventData<LobbyJoinedData>>(messageContents);
        if (eventData.error != null)
        {
            LobbyJoinedEvent?.Invoke(eventData.payload);
        }
        else
        {
            LobbyJoinedEvent?.Invoke(null, eventData.error);
        }
    }

    private async void OnDestroy()
    {
        await websocket.Close();
    }
}
