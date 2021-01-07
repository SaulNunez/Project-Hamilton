using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;

public class ActivityList : NetworkBehaviour
{
    [HideInInspector]
    public List<Activity> activities;

    public override void OnStartServer()
    {
        activities = GetComponentsInChildren<Activity>().ToList();

    }
}
