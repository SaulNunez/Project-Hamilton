using UnityEngine;
using System;
using BestHTTP.SignalRCore;
using BestHTTP.SignalRCore.Encoders;
using System.Threading.Tasks;
using System.Collections.Generic;
using Assets.Scripts.Multiplayer.ResultPayload;
using System.Linq;
using Assets.Scripts.Multiplayer.ServerRequestsPayload;
using Assets.Scripts.Players;

public class HamiltonHub
{
    private static readonly Lazy<HamiltonHub>
        lazy =
        new Lazy<HamiltonHub>
            (() => new HamiltonHub());

    public static HamiltonHub Instance { get { return lazy.Value; } }

    readonly HubConnection hubConnection;

    public delegate void OnEnteredLobbyDelegate(string lobbyCode);
    public event OnEnteredLobbyDelegate OnEnteredLobby;

    public delegate void CharacterAvailableChangedDelegate(List<CharacterData> characters);
    public event CharacterAvailableChangedDelegate OnCharacterAvailableChanged;

    public delegate void PlayerSelectedCharacterDelegate(NewPlayerInfo newPlayerInfo);
    public event PlayerSelectedCharacterDelegate OnOtherPlayerSelectedCharacter;

    public delegate void CurrentPlayerSelectedCharacter();
    public event CurrentPlayerSelectedCharacter OnCurrentPlayerSelectedCharacter;

    public delegate void NeedToSolvePuzzleDelegate(ShowPuzzleRequestPayload showPuzzleRequestPayload);
    public event NeedToSolvePuzzleDelegate OnNeedToSolvePuzzle;

    public delegate void MoveUpdateDelegate(MovementRequest moveInfo);
    public event MoveUpdateDelegate OnMoveUpdate;

    public delegate void MovePlayerDelegate(AvailableMovementOptions options);
    public event MovePlayerDelegate OnMoveRequest;


    public delegate void StatsUpdateDelegate(NewStats newStats);
    public event StatsUpdateDelegate OnStatsUpdate;

    public delegate void TurnHasStartedDelegate();
    public event TurnHasStartedDelegate OnTurnHasStarted;

    public delegate void OnThrowDiceDelegate(int result);
    public event OnThrowDiceDelegate OnThrowDice;

    public string LobbyCode { get; private set; }
    public string PlayerToken { get; private set; }
    public string SelectedCharacter { get; private set; }

    private HamiltonHub()
    {
        string connection = "https://hamilton-service-a.herokuapp.com/";
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            connection = Application.absoluteURL;
        }

        var currentPageUrl = new Uri(connection);
        var rootUrl = new Uri(currentPageUrl.GetLeftPart(UriPartial.Authority));
        hubConnection = new HubConnection(
            new Uri(rootUrl, "gameapi"), 
            new JsonProtocol(new LitJsonEncoder())
            );

        hubConnection.OnConnected += HubConnection_OnConnected;
        hubConnection.OnError += HubConnection_OnError;

        hubConnection.On<ShowPuzzleRequestPayload>("SolvePuzzle", (payload) =>
        {
            OnNeedToSolvePuzzle?.Invoke(payload);
        });

        hubConnection.On<MovementRequest>("MoveCharacterToPosition", (movementRequest) =>
        {
            OnMoveUpdate?.Invoke(movementRequest);
        });

        hubConnection.On<AvailableMovementOptions>("GetDirection", (movementOptions) =>
        {
            OnMoveRequest?.Invoke(movementOptions);
        });

        hubConnection.On<NewStats>("ChangeStats", (newStats) =>
        {
            OnStatsUpdate?.Invoke(newStats);
        });

        hubConnection.On("GetDirection", () =>
        {

        });

        hubConnection.On<NewPlayerInfo>("PlayerSelectedCharacter", (playerInfo) => OnOtherPlayerSelectedCharacter?.Invoke(playerInfo));

        hubConnection.On("StartTurn", () => OnTurnHasStarted?.Invoke());

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
            OnEnteredLobby?.Invoke(lobbyCode);
        }

        return enteredLobby;
    }

    public async Task<List<UniqueItemResult>> GetItemInfo()
    {
        var items = await hubConnection
            .InvokeAsync<List<UniqueItemResult>>("GetItems", new { PlayerToken, LobbyCode });

        return items;
    }

    public async Task<PuzzleResult> GetPuzzleResult(string code, string puzzleId)
    {
        return await hubConnection.InvokeAsync<PuzzleResult>("CheckPuzzle", new { code, puzzleId });
    }

    public async Task<CharacterAvailableResult> GetAvailableCharactersInLobby()
    {
        return await hubConnection.InvokeAsync<CharacterAvailableResult>("GetAvailableCharacters", new { lobbyCode = LobbyCode });
    }

    public async Task<string> SelectCharacter(string playerName, string characterToUse)
    {
        var result = await hubConnection.InvokeAsync<PlayerSelectionResult>("SelectCharacter", new { lobbyCode = LobbyCode, character = characterToUse, name = playerName });
        if(result != null)
        {
            SelectedCharacter = characterToUse;
            PlayerToken = result.playerToken;

            OnCurrentPlayerSelectedCharacter?.Invoke();
        }

        // Retornar null si llamar a funcion del servidor no retorna nada
        // Como si el jugador ya ha sido seleccionado
        return result?.playerToken ?? null;
    }

    public async Task<MovementResult> SendPlayerWantedDirection(Direction direction) => 
        await hubConnection.InvokeAsync<MovementResult>("Move", new { moveDirection = direction });

    public async Task<int> ThrowADice()
    {
        return (await hubConnection.InvokeAsync<ThrowResult>("ThrowDice")).DiceThrow;
    }
}
