using Mirror;

/// <summary>
/// Custom network player, has some player information that persists between sessions.
/// </summary>
public class HamiltonNetworkPlayer : NetworkRoomPlayer
{
    [SyncVar]
    public CharacterTypes characterType = CharacterTypes.AndreaLewis;

    [SyncVar]
    public string name;

    [Client]
    public void SetPlayerName(string name) => CmdSetPlayerName(name);

    [Command]
    void CmdSetPlayerName(string newName)
    {
        name = newName;
    }

    [SyncVar]
    public bool isImpostor = false;

    [Server]
    public void SetCharacter(CharacterTypes characterType)
    {
        this.characterType = characterType;
    }

}
