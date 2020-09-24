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
    public string lobbyCode;

    public void EnterLobby(string lobbyCode)
    {
        var webHostUrl = new Uri(Application.absoluteURL);
        int? port = webHostUrl.Port;
        if (webHostUrl.IsDefaultPort || webHostUrl.Port == -1)
        {
            port = null;
        }

        var websocketUrl = $"{(webHostUrl.Scheme == "https" ? "wss" : "ws")}://{webHostUrl.Host}{(port != null ? $":{port}" : "")}/gameapi/{lobbyCode}";
        print(websocketUrl);
        websocket = WebSocketFactory.CreateInstance(websocketUrl);

        websocket.OnOpen += () =>
        {
            Debug.Log("WS connected!");
            InvokeLobbyJoinedEvent();
        };

        websocket.OnMessage += (byte[] msg) =>
        {
            var messageContents = Encoding.UTF8.GetString(msg);

            var messageTypeObj = JsonUtility.FromJson<EventData>(messageContents);

            switch (messageTypeObj.type)
            {
                case "available_characters_update":
                    InvokeAvailableCharactersUpdate(messageContents);
                    break;
                case "player_selection_sucess":
                    InvokePlayerSelectionSuccess(messageContents);
                    break;
            }
        };

        websocket.OnError += async (string errMsg) =>
        {
            await websocket.Connect();
        };

        websocket.OnClose += async (WebSocketCloseCode code) =>
        {
            await websocket.Connect();
        };

        websocket.Connect();
    }

    public void GetAvailableCharacters() =>
        websocket?.SendText(JsonUtility.ToJson(
        new AvailableCharactersRequest
        {
            type = "get_available_characters",
            payload = new GetAvailableCharacterPayload
            {
                lobbyCode = lobbyCode
            }
        }));

    public void SelectCharacter(SelectCharacterPayload payload) =>
        websocket?.SendText(JsonUtility.ToJson(new SelectCharacterRequest
        {
            type = "select_character",
            payload = payload
        }));

    public delegate void LobbyJoinedHandler();
    public static event LobbyJoinedHandler LobbyJoined;
    public delegate void AvailableCharactersUpdateHandler(AvailableCharactersData data, string error = null);
    public static event AvailableCharactersUpdateHandler AvailableCharactersUpdate;
    public delegate void PlayerSelectedCharacterHandler(string error = null);
    public static event PlayerSelectedCharacterHandler PlayerSelectedCharacter;

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
    }

    private void InvokePlayerSelectionSuccess(string messageContents)
    {
        var eventData = JsonUtility.FromJson<EventData<PlayerSelectedCharacterData>>(messageContents);
        PlayerSelectedCharacter?.Invoke(eventData.error);
    }

    private void InvokeAvailableCharactersUpdate(string messageContents)
    {
        var eventData = JsonUtility.FromJson<EventData<AvailableCharactersData>>(messageContents);
        if (eventData.error != null)
        {
            AvailableCharactersUpdate?.Invoke(eventData.payload);
        }
        else
        {
            AvailableCharactersUpdate?.Invoke(null, eventData.error);
        }
    }

    private void InvokeLobbyJoinedEvent()
    {
        LobbyJoined?.Invoke();

    }

    private async void OnDestroy()
    {
        await websocket.Close();
    }
}
