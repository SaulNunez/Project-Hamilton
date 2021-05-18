using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WorldInteraction : NetworkBehaviour
{
    public delegate void ObjectStartDetected();
    public event ObjectStartDetected OnObjectStartDetected;

    public delegate void ObjectEndDetected();
    public event ObjectEndDetected OnObjectEndDetected;

    bool objectNear = false;

    [SerializeField]
    LayerMask taskMask;

    HubConfig hubConfig;

    void Update()
    {
        if(hasAuthority){
            //Objeto es
            bool somethingNear = Physics2D.OverlapCircle(transform.position, 3f, taskMask);

            if(somethingNear != objectNear)
            {
                if (somethingNear)
                {
                    OnObjectStartDetected?.Invoke();
                } else
                {
                    OnObjectEndDetected?.Invoke();
                }
            }
            objectNear = somethingNear;
        }
    }

    void EnableInteractOnUi(){
        GameUI.Instance.InteractionEnabled = true;
    }

    void DisableInteractOnUi(){
        GameUI.Instance.InteractionEnabled = false;
    }

    public override void OnStartServer()
    {
        base.OnStartServer();

        var lobbyConfigs = GameObject.FindGameObjectWithTag(Tags.HubConfig);
        hubConfig = lobbyConfigs.GetComponent<HubConfig>();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        GameUI.onGeneralClick += InteractWithEnvironment;

        OnObjectStartDetected += EnableInteractOnUi;
        OnObjectEndDetected += DisableInteractOnUi;
    }

    public void InteractWithEnvironment()
    {
        if (objectNear)
        {
            CmdInteractWithNearObject();
        }
    }

    [Command]
    public void CmdInteractWithNearObject()
    {
        Collider2D somethingNear = Physics2D.OverlapCircle(transform.position, hubConfig.actDistance, taskMask);
        if(somethingNear){
            var interactuables = somethingNear.GetComponents<IInteractuableBehaviour>();
            foreach(var interactuable in interactuables){
                interactuable.OnApproach(gameObject);
            }
        }
    }

    public override void OnStopClient()
    {
        base.OnStopClient();

        GameUI.onGeneralClick -= InteractWithEnvironment;

        OnObjectStartDetected -= EnableInteractOnUi;
        OnObjectEndDetected -= DisableInteractOnUi;
    }
}
