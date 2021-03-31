using Mirror;

/// <summary>
/// Custom network player, has some player information that persists between sessions.
/// </summary>
public class HamiltonNetworkPlayer : NetworkRoomPlayer
{
    [SyncVar]
    public CharacterTypes characterType;

    [SyncVar]
    public bool isImpostor = false;


    public override void OnStartServer()
    {
        base.OnStartServer();
    }

}
