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
        /**
         * Cambiar luego
         * Este mensaje se llama por el jugador al cambiar la escena y crear el jugador
         * Y como que ocurre antes de Start en el resto de la escena
         * lo que hace que messages sea null en este punto y sea un dolor de cabeza 
         */
        Invoke(nameof(ShowMessage), 1f);
    }

    void ShowMessage()
    {
        var killing = GetComponent<Killing>();
        if (killing.canKillSomeone)
        {
            Messages.Instance.ShowMessage("Eres un asesino, mata a los programadores para ganar");
        }
        else
        {
            Messages.Instance.ShowMessage("Eres un programador, resuelve los acertijos alrededor del mapa para ganar");
        }
    }
}
