using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField]
    [Range(0.01f, 10f)]
    float maxReachTime = 1.0f;

    [HideInInspector]
    public Transform target;

    float xCurrentSpeed = 0, yCurrentSpeed = 0;

    void Update(){
        if(target != null) {
            transform.position = new Vector3(
                Mathf.SmoothDamp(transform.position.x, target.position.x, 
                                    ref xCurrentSpeed, maxReachTime),
                Mathf.SmoothDamp(transform.position.y, target.position.y, 
                                    ref yCurrentSpeed, maxReachTime), 
                transform.position.z);
        }
    }
}
