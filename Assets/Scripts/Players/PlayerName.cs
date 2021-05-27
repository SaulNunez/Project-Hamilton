using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerName : NetworkBehaviour
{
    [SerializeField]
    TextMesh playerNameLabel;

    public string Name { get => playerNameLabel.text; set => playerNameLabel.text = value; }

    public override void OnStartClient()
    {
        base.OnStartClient();

        var name = (NetworkManager.singleton as HamiltonNetworkRoomManager).PlayerName;
        if (name != null && name.Length > 0)
        {
            Name = name;
        } else
        {
            Name = $"Jugador {new Guid()}";
        }
    }
}
