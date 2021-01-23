using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HamiltonNetworkPlayer : NetworkRoomPlayer
{
    public SyncList<ActivityInfo> activitiesToDo = new SyncList<ActivityInfo>();

    public override void OnStartServer()
    {
        var activities = GameObject.FindGameObjectWithTag("Activities");
        var activitiesList = activities.GetComponent<ActivityList>();
        if(activitiesList != null){
            //TODO: Leer de configuracion de lobby cantidad de tareas a spawnear
            //activitiesToDo.AddRange(activitiesList.activities.PickRandom(6));
        }
    }
}
