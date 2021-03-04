using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureSabotagePuzzle : NetworkBehaviour
{


    [ClientRpc]
    void RpcClosePuzzle()
    {
        gameObject.SetActive(false);
    }
}
