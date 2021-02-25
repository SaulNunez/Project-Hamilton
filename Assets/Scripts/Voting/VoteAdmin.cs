using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Maneja realizar voto para un unico jugador
/// </summary>
public class VoteAdmin : NetworkBehaviour
{
    [SyncVar]
    public bool hasCastedVote;

    [SyncVar]
    public GameObject votesFor = null;

    // -- VARIABLES SOLO SERVIDOR
    VotingManager votingManager;

    [Command]
    public void CmdVoteFor(GameObject gameObject)
    {
        if(votesFor != null)
        {
            votesFor = gameObject;
        }
    } 
}
