using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;

public class ActivityList: NetworkBehaviour
{
    public SyncList<ActivityInfo> activities = new SyncList<ActivityInfo>();

    public override void OnStartServer()
    {
        activities.AddRange(Resources.LoadAll<ActivityInfo>("Activities"));
    }
}
