using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractuableBehavior : NetworkBehaviour
{
    public virtual void OnApproach(GameObject approachedBy) { }
}
