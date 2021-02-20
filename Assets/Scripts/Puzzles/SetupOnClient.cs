using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupOnClient : MonoBehaviour
{
    private void Start()
    {
        var canvas = GameObject.FindGameObjectWithTag(Tags.UiManager);

        this.transform.parent = canvas.transform;
    }
}
