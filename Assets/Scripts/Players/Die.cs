using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Controls alive status on client and what to do when player dies.
/// </summary>
public class Die : NetworkBehaviour
{
    public bool isDead = false;

    [SerializeField]
    GameObject deathPrefab;
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();

        /// print($"Current culling mask {Camera.main.cullingMask}, ghost layer: {(1 << LayerMask.NameToLayer(Layers.Ghost))}");
    }

    /// <summary>
    /// Handles death when killed and a tomb is spawned
    /// </summary>
    [Server]
    public void SetDed()
    {
        SetSimpleDeath();

        //Mostrar tumbita
        var deathRemainings = Instantiate(deathPrefab, transform.position, Quaternion.identity);
        NetworkServer.Spawn(deathRemainings);
    }

    /// <summary>
    /// Handles death when killed by votes
    /// </summary>
    [Server]
    public void SetSimpleDeath()
    {
        if (hasAuthority)
        {
            anim.SetBool("Dead", true);
        }
        RpcConvertDeadPlayerToGhost();
        TargetOnDeathConfig(netIdentity.connectionToClient);
    }

    [ClientRpc]
    void RpcConvertDeadPlayerToGhost()
    {
        var ghostLayer = LayerMask.NameToLayer("Ghost");
        gameObject.layer = ghostLayer;
    }

    [TargetRpc]
    void TargetOnDeathConfig(NetworkConnection target)
    {
        print("Show ghosts");
        //Mostrar capa de fantasmas
        Camera.main.cullingMask = Camera.main.cullingMask | (1 << LayerMask.NameToLayer(Layers.Ghost));
        PuzzleUI.instance.ClosePuzzles();

        if (hasAuthority)
        {
            anim.SetBool("Dead", true);
        }
    }
}
