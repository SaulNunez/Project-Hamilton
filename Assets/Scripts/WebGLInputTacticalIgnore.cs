using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// For WebGL, on complex templates, makes sure inputs are usable in the host web template
/// </summary>
public class WebGLInputTacticalIgnore : MonoBehaviour
{
#if UNITY_WEBGL
    void Start() => WebGLInput.captureAllKeyboardInput = false;
#endif
}
