using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Utility that can be added to any gameobject that needs to persist scene changes.
/// </summary>
public class DontKillOnSceneChange : MonoBehaviour
{
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
