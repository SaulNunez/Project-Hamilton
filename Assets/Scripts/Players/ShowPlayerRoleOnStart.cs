using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
[RequireComponent(typeof(Killing))]
public class ShowPlayerRoleOnStart : NetworkBehaviour
{
    public override void OnStartAuthority()
    {
        base.OnStartAuthority();

        var killing = GetComponent<Killing>();
        if (killing.canKillSomeone)
        {
            Messages.instance.ShowMessage("Eres un asesino, mata a los programadores para ganar");
        } else
        {
            Messages.instance.ShowMessage("Eres un programador, resuelve los acertijos alrededor del mapa para ganar");
        }
    }
}
