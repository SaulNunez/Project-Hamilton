using UnityEngine;
using System;
using BestHTTP.SignalRCore;
using BestHTTP.SignalRCore.Encoders;
using System.Threading.Tasks;
using System.Collections.Generic;
using Assets.Scripts.Multiplayer.ResultPayload;
using System.Linq;

public class HamiltonHub
{
    private static readonly Lazy<HamiltonHub>
        lazy =
        new Lazy<HamiltonHub>
            (() => new HamiltonHub());

    public static HamiltonHub Instance { get { return lazy.Value; } }

    readonly HubConnection hubConnection;

    public delegate void OnEnteredLobby(string lobbyCode);
    public event OnEnteredLobby onEnteredLobby;

    public delegate void CharacterAvailableChangedDelegate(List<CharacterData> characters);
    public event CharacterAvailableChangedDelegate OnCharacterAvailableChanged;

    public delegate void PlayerSelectedCharacterDelegate();
    public event PlayerSelectedCharacterDelegate OnPlayerSelectedCharacter;

    public string LobbyCode { get; private set; }
    public string PlayerToken { get; private set; }

    private HamiltonHub()
    {
        string connection = "https://hamilton-service-a.herokuapp.com/";
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            connection = Application.absoluteURL;
        }

        var currentPageUrl = new Uri(connection);
        var rootUrl = new Uri(currentPageUrl.GetLeftPart(UriPartial.Authority));
        hubConnection = new HubConnection(new Uri(rootUrl, "gameapi"), new JsonProtocol(new LitJsonEncoder()));

        hubConnection.OnConnected += HubConnection_OnConnected;
        hubConnection.OnError += HubConnection_OnError;

        hubConnection.StartConnect();
    }

    private void HubConnection_OnError(HubConnection arg1, string arg2)
    {
        
    }

    private void HubConnection_OnConnected(HubConnection obj)
    {
        
    }

   public async Task<bool> ConnectToLobby(string lobbyCode)
    {
        var enteredLobby = await hubConnection.InvokeAsync<bool>("EnterLobby", lobbyCode);

        if (enteredLobby)
        {
            LobbyCode = lobbyCode;
            onEnteredLobby?.Invoke(lobbyCode);
        }

        return enteredLobby;
    }

    public async Task<CharacterAvailableResult> GetAvailableCharactersInLobby()
    {
        return await hubConnection.InvokeAsync<CharacterAvailableResult>("GetAvailableCharacters", new { lobbyCode = LobbyCode });
    }

    public async Task<string> SelectCharacter(string playerName, string characterToUse)
    {
        var result = await hubConnection.InvokeAsync<PlayerSelectionResult>("SelectCharacter", new { character = characterToUse, name = playerName });
        if(result != null)
        {
            PlayerToken = result.playerToken;
        }

        // Retornar null si llamar a funcion del servidor no retorna nada
        // Como si el jugador ya ha sido seleccionado
        return result?.playerToken ?? null;
    }
}
