using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HamiltonNetworkPlayer : NetworkRoomPlayer
{
    [Header("Player stats")]
    public bool alive = true;
    public bool isImpostor = false;

}
