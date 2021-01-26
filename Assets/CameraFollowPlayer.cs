using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class CameraFollowPlayer : NetworkBehaviour
{
    public override void OnStartAuthority()
    {
        base.OnStartAuthority();

        var camera = GameObject.FindGameObjectWithTag(Tags.MainCamera);

        var cameraControl = camera.GetComponent<CameraControl>();

        if(cameraControl != null){
            cameraControl.target = this.transform;
        } else {
            Debug.LogWarning("Camara no tiene Camera Control, seguir jugador no va a funcionar.");
        }
    }
}
