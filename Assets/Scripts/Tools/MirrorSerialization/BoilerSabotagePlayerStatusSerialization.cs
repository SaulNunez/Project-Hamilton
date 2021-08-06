using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BoilerSabotagePlayerStatusSerialization
{
    public static void WriteBoilerSabotagePlayerStatusSelection(this NetworkWriter writer,
    SabotageBoilers.PlayerStatus playerStatus)
    {
        writer.WriteNetworkIdentity(playerStatus.playerOnButton);
        writer.WriteUInt64((ulong)playerStatus.startClick.Ticks);
    }

    public static SabotageBoilers.PlayerStatus ReadBoilerSabotagePlayerStatusSelection(this NetworkReader reader)
    {
        return new SabotageBoilers.PlayerStatus {
            playerOnButton = reader.ReadNetworkIdentity(),
            startClick = new DateTime((long)reader.ReadUInt64())
        };
    }
}
