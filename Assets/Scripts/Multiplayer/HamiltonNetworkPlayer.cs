using Mirror;
using System.Collections.Generic;

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

    readonly static List<string> verbos = 
        new List<string> { "Saltarín", "Jugueton", "Comelon", "Correlon", "Sonriente", "Preocupado", "Hablador" };
    readonly static List<string> sustantivos = 
        new List<string> { "Jaguar", "Ocelote", "Guacamaya", "Correcaminos", "Aguila", "Ajolote", "Gato", "Perro", "Oso", "Ganso" };

    public override void OnStartServer()
    {
        base.OnStartServer();

        name = $"{sustantivos.PickRandom()} {verbos.PickRandom()}";
    }
}
