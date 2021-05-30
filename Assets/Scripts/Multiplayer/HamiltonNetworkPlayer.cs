using Mirror;

/// <summary>
/// Custom network player, has some player information that persists between sessions.
/// </summary>
public class HamiltonNetworkPlayer : NetworkRoomPlayer
{
    [SyncVar]
    public CharacterTypes characterType = CharacterTypes.AndreaLewis;

    [SyncVar]
    public bool isImpostor = false;


    public override void OnStartServer()
    {
        base.OnStartServer();
    }

}
