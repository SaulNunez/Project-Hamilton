using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyUIHelper : NetworkBehaviour
{
    [SerializeField]
    GameObject readyUi;
    HamiltonNetworkRoomManager nrm;

    void Start()
    {
        readyUi.SetActive(hasAuthority);

        nrm = NetworkManager.singleton as HamiltonNetworkRoomManager;
        nrm.OnSceneChanged += OnSceneChange;
    }

    private void OnSceneChange()
    {
        readyUi.SetActive(nrm.RoomScene == SceneManager.GetActiveScene().name && hasAuthority);
    }

    public void SetAsReady(bool ready)
    {
        var networkPlayer = GetComponent<NetworkRoomPlayer>();
        networkPlayer.CmdChangeReadyState(ready);
    }
}
