using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupDefaultFlowers: NetworkBehaviour {
    [SyncVar]
    string defaultFlowerType;

    public override void OnStartServer()
    {
        base.OnStartServer();

        defaultFlowerType = FlowerTypes.Types.PickRandom();

        var buttons = FlowerTypes.Types.PickRandom(6);
    }
}
