using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEmergencyMeeting : NetworkBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        CmdStartMeeting();
    }

    [Command]
    private void CmdStartMeeting()
    {
        var votingManagerGameObject = GameObject.FindGameObjectWithTag(Tags.VotingManager);
        var votingManager = votingManagerGameObject.GetComponent<VotingManager>();

        votingManager.StartVoting();
    }
}
