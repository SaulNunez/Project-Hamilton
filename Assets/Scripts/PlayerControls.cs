using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : NetworkBehaviour
{
    public float speed = 2f;

    void Update()
    {
        transform.position += new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0) * speed * Time.deltaTime;
    }
}
