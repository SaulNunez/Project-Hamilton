using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class HubConfig : NetworkBehaviour
{
    [SyncVar]
    public int numberOfImpostors = 1;

    [SyncVar]
    public int numberOfTasks = 4;

    [SyncVar]
    public int secondsOfCooldownToEmergencies = 30;

    [SyncVar]
    public int secondsOfCooldownsForKill = 20;

    [SyncVar]
    public int secondsEmergencyDuration = 30;
    
    [SyncVar]
    public int secondsOfCooldownForSabotage = 25;
    
    [SyncVar]
    public float actDistance = 2f;

    [SyncVar]
    public int secondsForVoting = 30;
}
