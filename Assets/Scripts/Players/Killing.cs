using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killing : NetworkBehaviour
{
    [HideInInspector]
    [SyncVar(hook = nameof(OnKillStateChanged))]
    public bool canKill;

    [SerializeField]
    private LayerMask playersLayerMask;

    HubConfig config;

    Collider2D other;

    Collider2D[] foundPlayerColliders = new Collider2D[3];

    private void OnKillStateChanged(bool oldValue, bool newValue)
    {
        print("bbbbb");
        if (hasAuthority)
        {
            print("aaaaa");
            GameUI.Instance.CanInteractWithKillButton = newValue;
        }
    }

    public override void OnStartServer()
    {
        base.OnStartServer();

        var hubConfigGO = GameObject.FindGameObjectWithTag(Tags.HubConfig);
        config = hubConfigGO.GetComponent<HubConfig>();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        GameUI.onKillButtonClick += GameUI_onKillButtonClick;
    }

    private void GameUI_onKillButtonClick()
    {
        CmdMurder();
    }

    void Update()
    {
        if (isServer)
        {
            if(Physics2D.OverlapCircleNonAlloc(transform.position, config.actDistance, foundPlayerColliders, playersLayerMask) > 0)
            {
                canKill = true;
            } else
            {
                canKill = false;
            }

            foreach(var collider in foundPlayerColliders)
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

    [Command]
    public void CmdMurder()
    {
        if (canKill)
        {
            var dieComponent = other.gameObject.GetComponent<Die>();
            if (dieComponent != null)
            {
                dieComponent.SetDed();
            }
        }
    }

    public override void OnStopClient()
    {
        base.OnStopClient();

        GameUI.onKillButtonClick -= GameUI_onKillButtonClick;
    }
}
