using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
[RequireComponent(typeof(Killing))]
public class ActivateGameplayUI : NetworkBehaviour
{
    public override void OnStartAuthority()
    {
        base.OnStartAuthority();

        var killing = GetComponent<Killing>();
        //Enable assasin buttons when player is one
        GameUI.Instance.EnableAssasinExtras = killing.IsAssasin;
    }
}
