using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeletransportManager : NetworkBehaviour
{
    SyncDictionary<NetworkIdentity, GameObject> onTeletrasportMode = new SyncDictionary<NetworkIdentity, GameObject>();

    public static TeletransportManager instance = null;

    public override void OnStartServer()
    {
        base.OnStartServer();

        if(instance == null)
        {
            instance = this;
        }
    }

    [Server]
    public void AddToTeletransportList(NetworkIdentity player, GameObject currentVent)
    {
        onTeletrasportMode.Add(player, currentVent);

        TargetAskPermisionForTeletrasport(player.connectionToClient);

        //Move camera target
        var camera = GameObject.FindGameObjectWithTag(Tags.MainCamera);

        var cameraControl = camera.GetComponent<CameraControl>();
        cameraControl.MoveCameraToTarget(currentVent.transform.position);

        //Move outside of world to not appear on screen - 200 IQ Move
        var netTransform = player.GetComponent<NetworkTransform>();
        if(netTransform != null)
        {
            netTransform.ServerTeleport(new Vector2(200, 200));
        }
    }

    [Server]
    public void RemoveToTeletransportList(NetworkIdentity player)
    {
        onTeletrasportMode.Remove(player);
    }

    [Client]
    public void MoveToNextVent() => CmdMoveToNextVent();

    [Command(ignoreAuthority = true)]
    public void CmdMoveToNextVent(NetworkConnectionToClient sender = null)
    {
        var vent = onTeletrasportMode[sender.identity];
        var ventTechnology = vent.GetComponent<Teletransport>();

        onTeletrasportMode[sender.identity] = ventTechnology.teletransportTo.gameObject;

        MovePlayerCameraToVent(sender);
    }

    [TargetRpc]
    void MovePlayerCameraToVent(NetworkConnection target)
    {
        //Move camera target
        var camera = GameObject.FindGameObjectWithTag(Tags.MainCamera);

        var cameraControl = camera.GetComponent<CameraControl>();
        cameraControl.MoveCameraToTarget(onTeletrasportMode[target.identity].transform.position);
    }


    [TargetRpc]
    void TargetAskPermisionForTeletrasport(NetworkConnection target)
    {
        var helperGO = GameObject.FindGameObjectWithTag(Tags.TeletransportHelper);
        var helper = helperGO.GetComponent<TeletransportUiConnect>();
        helper.OpenUi();
    }

    [Client]
    public void ComeOut() => CmdComeOut();

    [Command(ignoreAuthority = true)]
    void CmdComeOut(NetworkConnectionToClient sender = null)
    {
        //Teletransport is done server side, and server doesn't have authority in gameobject
        var networkTransform = sender.identity.gameObject.GetComponent<NetworkTransform>();
        var ventPosition = onTeletrasportMode[sender.identity].transform.position;
        networkTransform.ServerTeleport(ventPosition);

        RemoveToTeletransportList(sender.identity);

        TargetDisablePermisionForTeletransport(sender);

        //Come out vent
        var camera = GameObject.FindGameObjectWithTag(Tags.MainCamera);

        var cameraControl = camera.GetComponent<CameraControl>();
        cameraControl.FollowPlayer();
    }

    [TargetRpc]
    void TargetDisablePermisionForTeletransport(NetworkConnection target)
    {
        var helperGO = GameObject.FindGameObjectWithTag(Tags.TeletransportHelper);
        var helper = helperGO.GetComponent<TeletransportUiConnect>();
        helper.HideUi();
    }
}
