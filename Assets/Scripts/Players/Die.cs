using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : NetworkBehaviour
{
    public bool isDead = false;

    [SerializeField]
    GameObject deathPrefab;
    Animator anim;

    public override void OnStartServer()
    {
        var lobbyConfigs = GameObject.FindGameObjectWithTag(Tags.HubConfig);

        anim = GetComponent<Animator>();
    }

    [Server]
    public void SetDed()
    {
        anim.SetBool("Dead", true);

        //Mostrar tumbita
        var deathRemainings = Instantiate(deathPrefab, transform.position, Quaternion.identity);
        NetworkServer.Spawn(deathRemainings);

        var ghostLayer = LayerMask.NameToLayer("Ghost");
        gameObject.layer = ghostLayer;
    }
}
