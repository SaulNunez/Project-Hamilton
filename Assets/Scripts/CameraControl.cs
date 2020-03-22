using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public GameObject character;
    private Vector3 target;
    public float movementSpeed = 0.5f;

    void Start()
    {
        
    }

    void Update()
    {
        target.x = character.transform.position.x;
        target.y = character.transform.position.y;
        target.z = transform.position.z;

        transform.position = Vector3.Lerp(transform.position, target, movementSpeed * Time.deltaTime);
    }
}
