﻿using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attached to player, control player killing and if it can kill another players
/// </summary>
public class Killing : NetworkBehaviour
{
    /// <summary>
    /// Is player near someone that can be killed
    /// </summary>
    [SyncVar(hook = nameof(OnKillStateChanged))]
    public bool canKillSomeone = false;

    /// <summary>
    /// If player can kill other players, is true they are against another players and have extra controls for their characters
    /// </summary>
    [SyncVar]
    private bool isAssasin = false;

    [SyncVar]
    private double endOfCooldown;

    [SerializeField]
    private LayerMask playersLayerMask;

    HubConfig config;

    Collider2D other;
    readonly Collider2D[] foundPlayerColliders = new Collider2D[3];

    public bool IsAssasin { get => isAssasin; set => isAssasin = value; }

    private void OnKillStateChanged(bool oldValue, bool newValue)
    {
        if (hasAuthority)
        {
            GameUI.Instance.CanInteractWithKillButton = newValue;
        }
    }

    public override void OnStartServer()
    {
        base.OnStartServer();

        var hubConfigGO = GameObject.FindGameObjectWithTag(Tags.HubConfig);
        config = hubConfigGO.GetComponent<HubConfig>();

        VotingManager.OnVotingEnded += OnVotingEnded;
    }

    /// <summary>
    /// Starts post voting cooldown 
    /// </summary>
    private void OnVotingEnded()
    {
        endOfCooldown = NetworkTime.time + config.secondsOfCooldownsForKill;
        canKillSomeone = false;
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        GameUI.onKillButtonClick += AttemptToKill;
    }

    private void AttemptToKill()
    {
        CmdMurder();
    }

    void Update()
    {
        if (isServer && isAssasin)
        {
            canKillSomeone = endOfCooldown <= NetworkTime.time &&
                Physics2D.OverlapCircleNonAlloc(transform.position, config.actDistance, foundPlayerColliders, playersLayerMask) > 0;

            foreach (var collider in foundPlayerColliders)
            {
                if(collider != null && collider.gameObject != this.gameObject)
                {
                    if (collider)
                    {
                        other = collider;
                    }
                    break;
                }
            }
        }
    }

    /// <summary>
    /// Kill another player if they are in range
    /// </summary>
    /// <remarks>Called by client, executed in server</remarks>
    [Command]
    public void CmdMurder()
    {
        if (canKillSomeone && isAssasin)
        {
            var dieComponent = other.gameObject.GetComponent<Die>();
            if (dieComponent != null)
            {
                dieComponent.SetDed();
                endOfCooldown = NetworkTime.time + config.secondsOfCooldownsForKill;
                canKillSomeone = false;
            }
        }
    }

    public override void OnStopClient()
    {
        base.OnStopClient();

        GameUI.onKillButtonClick -= AttemptToKill;
    }

    public override void OnStopServer()
    {
        base.OnStopServer();

        VotingManager.OnVotingEnded -= OnVotingEnded;
    }

    public override bool Equals(object obj)
    {
        return obj is Killing killing &&
               base.Equals(obj) &&
               canKillSomeone == killing.canKillSomeone;
    }

    public override int GetHashCode()
    {
        int hashCode = -472072723;
        hashCode = hashCode * -1521134295 + base.GetHashCode();
        hashCode = hashCode * -1521134295 + canKillSomeone.GetHashCode();
        return hashCode;
    }
}
