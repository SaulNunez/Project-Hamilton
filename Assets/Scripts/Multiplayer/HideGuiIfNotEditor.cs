using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NetworkManagerHUD))]
public class HideGuiIfNotEditor : MonoBehaviour
{
    void Start()
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            var hud = GetComponent<NetworkManagerHUD>();
            hud.showGUI = false;
        }
    }
}
