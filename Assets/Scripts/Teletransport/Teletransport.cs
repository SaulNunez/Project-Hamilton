using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teletransport : InteractuableBehavior
{
    public Transform teletransportTo;

    public override void OnApproach(GameObject approachedBy)
    {
        base.OnApproach(approachedBy);

        approachedBy.transform.position = (Vector2)teletransportTo.position;
    }
}
