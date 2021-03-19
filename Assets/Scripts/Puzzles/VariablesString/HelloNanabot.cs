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

    public void CommitPuzzle()
    {
        CheckMessage(textbox.text);
    }

    [Command]
    void CheckMessage(string message)
    {
        // Si quisieramos hacer mayor verificacion.
        // Ej. revisar que no tenga algun mensaje obsceno, aqui sucederia 
        var helloMessage = $"Hola {message}";
        SayOnClient(netIdentity.connectionToClient, helloMessage);
        PuzzleCompletion.instance.MarkCompleted(PuzzleId.VariableString);
        RpcClosePuzzle();
    }

    [TargetRpc]
    void SayOnClient(NetworkConnection target, string message)
    {
        var nanabot = GameObject.FindGameObjectWithTag(Tags.Nanabot);
        var nanabotSays = nanabot.GetComponent<NanaBot>();
        nanabotSays.Talk(message);
    }

    [ClientRpc]
    void RpcClosePuzzle()
    {
        gameObject.SetActive(false);
    }
}
