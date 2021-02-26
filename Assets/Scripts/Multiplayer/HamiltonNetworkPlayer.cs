using Mirror;

/// <summary>
/// Custom network player, has some player information that persists between sessions.
/// </summary>
public class HamiltonNetworkPlayer : NetworkRoomPlayer
{
    [SyncVar]
    public string playerName;

    [SyncVar]
    public CharacterTypes characterType;


    public override void OnStartServer()
    {
        base.OnStartServer();

        playerName = $"Player{index}";
    }
}
