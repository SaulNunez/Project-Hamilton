﻿using Mirror;
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

    public override void OnStartServer()
    {
        anim = GetComponent<Animator>();
    }

    [Server]
    public void SetDed()
    {
        SetSimpleDeath();

        //Mostrar tumbita
        var deathRemainings = Instantiate(deathPrefab, transform.position, Quaternion.identity);
        NetworkServer.Spawn(deathRemainings);
    }

    /// <summary>
    /// Muerte cuando el jugador no spawnea esqueleto donde murio.
    /// </summary>
    [Server]
    public void SetSimpleDeath()
    {
        anim.SetBool("Dead", true);
        var ghostLayer = LayerMask.NameToLayer("Ghost");
        gameObject.layer = ghostLayer;
        TargetOnDeathConfig(netIdentity.connectionToClient);
    }

    [TargetRpc]
    void TargetOnDeathConfig(NetworkConnection target)
    {
        //Mostrar capa de fantasmas
        Camera.main.cullingMask = Camera.main.cullingMask | LayerMask.NameToLayer(Layers.Ghost);
    }
}