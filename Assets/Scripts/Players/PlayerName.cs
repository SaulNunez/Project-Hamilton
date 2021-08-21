using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerName : NetworkBehaviour
{
    public string Name { get => playerName; }

    [SerializeField]
    TextMeshPro playerNameTag;

    [SyncVar(hook = nameof(ChangeNameOnScreen))]
    private string playerName;

    [Server]
    public void SetName(string name)
    {
        this.playerName = name;
    }

    void ChangeNameOnScreen(string oldValue, string newValue)
    {
        playerNameTag.text = newValue;
    }
}
