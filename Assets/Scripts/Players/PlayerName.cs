using Mirror;
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

        Name = (NetworkManager.singleton as HamiltonNetworkRoomManager).PlayerName;
    }
}
