using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : NetworkBehaviour
{
    public float speed = 4f;
    public Rigidbody2D rigidbody2D;

    private Vector2 velocity = Vector2.zero;

    private void Update()
    {
        velocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    void FixedUpdate()
    {
        if (hasAuthority)
        {
            rigidbody2D.MovePosition((Vector2)transform.position + (velocity * speed * Time.deltaTime));
        }
    }
}
