using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebGLInputTacticalIgnore : MonoBehaviour
{
#if UNITY_WEBGL
    void Start() => WebGLInput.captureAllKeyboardInput = false;
#endif
}
