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

    public override void OnStartClient()
    {
        base.OnStartClient();

        GameUI.onGeneralClick += InteractWithEnvironment;
    }

    public void InteractWithEnvironment()
    {
        if (objectNear)
        {
            CmdInteractWithNearObject();
        }
    }

    [Server]
    public void CmdInteractWithNearObject()
    {
        Collider2D somethingNear = Physics2D.OverlapCircle(transform.position, 3f, taskMask);
        if(somethingNear){
            var interactuables = somethingNear.GetComponents<InteractuableBehavior>();
            foreach(var interactuable in interactuables){
                interactuable.OnApproach(gameObject);
            }
        }
    }

    public override void OnStopClient()
    {
        base.OnStopClient();

        GameUI.onGeneralClick -= InteractWithEnvironment;
    }
}
