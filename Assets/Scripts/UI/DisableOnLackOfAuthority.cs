using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOnLackOfAuthority : NetworkBehaviour
{
    void Start()
    {
        if (!hasAuthority)
        {
            gameObject.SetActive(false);
        }
    }
}
