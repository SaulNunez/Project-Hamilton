using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

/// <summary>
/// Keeps information about game lobby parameters
/// </summary>
public class HubConfig : NetworkBehaviour
{
    /// <summary>
    /// How many assasins to have in a game lobby
    /// </summary>
    [SyncVar]
    public int numberOfImpostors = 5;

    /// <summary>
    /// Seconds to wait before reactivating the kill button after killing someone
    /// </summary>
    [SyncVar]
    public int secondsOfCooldownsForKill = 20;

    /// <summary>
    /// How much time players have to solve an emergency
    /// </summary>
    [SyncVar]
    public int secondsEmergencyDuration = 30;
    
    /// <summary>
    /// how much time assasins have to wait before the game permits to activate another one
    /// </summary>
    [SyncVar]
    public int secondsOfCooldownForSabotage = 25;
    
    /// <summary>
    /// Distance for action buttons (killing, activating puzzles, etc.)
    /// </summary>
    [SyncVar]
    public float actDistance = 2f;

    /// <summary>
    /// How much time players have to vote
    /// </summary>
    [SyncVar]
    public int secondsForVoting = 30;
}
