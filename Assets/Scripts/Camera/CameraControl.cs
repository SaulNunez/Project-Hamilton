using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform currentPosition;
    private Vector3 target;
    public float movementSpeed = 0.5f;

    void Update()
    {
        //target.x = currentPosition.transform.position.x;
        //target.y = currentPosition.transform.position.y;
        //target.z = transform.position.z;

        //transform.position = Vector3.Lerp(transform.position, target, movementSpeed * Time.deltaTime);
    }
}
