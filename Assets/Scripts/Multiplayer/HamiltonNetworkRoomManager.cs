using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HamiltonNetworkRoomManager : NetworkRoomManager
{
    public delegate void SceneChanged();
    public event SceneChanged OnSceneChanged;
    
    public override void OnRoomClientSceneChanged(NetworkConnection conn)
    {
        OnSceneChanged?.Invoke();
    }
}
