using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// String puzzle logic
/// </summary>
public class HelloNanabot : NetworkBehaviour
{
    public TMP_InputField textbox;

    /// <summary>
    /// Gets value of textbox and sends it to the server
    /// </summary>
    [Client]
    public void CommitPuzzle()
    {
        CheckMessage(textbox.text);
    }

    [Command(ignoreAuthority = true)]
    void CheckMessage(string message, NetworkConnectionToClient sender = null)
    {
        // Si quisieramos hacer mayor verificacion.
        // Ej. revisar que no tenga algun mensaje obsceno, aqui sucederia 
        var helloMessage = $"Hola {message}";
        SayOnClient(sender, helloMessage);
        PuzzleCompletion.instance.MarkCompleted(PuzzleId.VariableString);
        TargetClosePuzzle(sender);
    }

    [TargetRpc]
    void SayOnClient(NetworkConnection target, string message)
    {
        var nanabot = GameObject.FindGameObjectWithTag(Tags.Nanabot);
        var nanabotSays = nanabot.GetComponent<NanaBot>();
        nanabotSays.Talk(message);
    }

    [TargetRpc]
    void TargetClosePuzzle(NetworkConnection target)
    {
        PuzzleUI.instance.ClosePuzzles();
    }
}
