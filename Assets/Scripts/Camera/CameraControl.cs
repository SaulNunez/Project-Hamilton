using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls camera position to follow the player or an override target
/// </summary>
public class CameraControl : MonoBehaviour
{
    [Tooltip("Seconds to reach target, used by smoothing")]
    [SerializeField]
    [Range(0.01f, 10f)]
    float maxReachTime = 1.0f;

    /// <summary>
    /// Player to target, only used to read `transform.position`
    /// </summary>
    [HideInInspector]
    public Transform target;

    float xCurrentSpeed = 0, yCurrentSpeed = 0;

    /// <summary>
    /// If is targeting a specific position, or if disabled the player
    /// </summary>
    bool onOverride = false;

    /// <summary>
    /// Position to target on override
    /// </summary>
    Vector2 overrideTarget;

    void Update(){
        if(target != null) {
            transform.position = new Vector3(
                Mathf.SmoothDamp(transform.position.x, onOverride? overrideTarget.x : target.position.x, 
                                    ref xCurrentSpeed, maxReachTime),
                Mathf.SmoothDamp(transform.position.y, onOverride? overrideTarget.y : target.position.y, 
                                    ref yCurrentSpeed, maxReachTime), 
                transform.position.z);
        }
    }

    /// <summary>
    /// Enables override targeting
    /// </summary>
    public void MoveCameraToTarget(Vector2 newTarget)
    {
        overrideTarget = newTarget;
        onOverride = true;
    }

    /// <summary>
    /// Disables override targeting, making camera follow player
    /// </summary>
    public void FollowPlayer()
    {
        onOverride = false;
    }
}
