using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEmergencyMeeting : InteractuableBehavior
{
    public override void OnApproach(GameObject approachedBy) {
        var votingManagerGameObject = GameObject.FindGameObjectWithTag(Tags.VotingManager);
        var votingManager = votingManagerGameObject.GetComponent<VotingManager>();

        votingManager.StartVoting();

        print("Button pressed");
    }
}
