using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles activating a canvas on clients with authority. AKA in the client they belong.
/// </summary>
public class LobbyUIHelper : NetworkBehaviour
{
    [SerializeField]
    GameObject readyUi;
    HamiltonNetworkRoomManager nrm;

    public override void OnStartClient()
    {
        base.OnStartClient();

        readyUi.SetActive(hasAuthority);

        nrm = NetworkManager.singleton as HamiltonNetworkRoomManager;
        nrm.OnSceneChanged += OnSceneChange;
    }

    private void OnSceneChange()
    {
        readyUi.SetActive(nrm.RoomScene == SceneManager.GetActiveScene().name && hasAuthority);
    }

    /// <summary>
    /// Updates ready state on the network player
    /// </summary>
    /// <param name="ready"></param>
    public void SetAsReady(bool ready)
    {
        var networkPlayer = GetComponent<NetworkRoomPlayer>();
        networkPlayer.CmdChangeReadyState(ready);
    }
}
