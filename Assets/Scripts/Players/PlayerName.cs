using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerName : NetworkBehaviour
{
    public string Name { get; set; }

    public override void OnStartClient()
    {
        base.OnStartClient();

        var name = (NetworkManager.singleton as HamiltonNetworkRoomManager).PlayerName;
        Name = name;
    }
}
