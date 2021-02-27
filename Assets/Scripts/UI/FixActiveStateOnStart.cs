using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Workaround on failing to passing gameobject active property when spawning UI on client.
/// Disables UI to prevent poluding the screen on the user.
/// </summary>
public class FixActiveStateOnStart : NetworkBehaviour
{
    public override void OnStartClient()
    {
        base.OnStartClient();

        gameObject.SetActive(false);
    }
}
