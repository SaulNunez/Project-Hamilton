using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : NetworkBehaviour
{
    public float speed = 4f;
    public float timeToMax = 0.4f;
    public Rigidbody2D rigidbody2D;

    private Vector2 newPositionVec = Vector2.zero;

    private float xInputVelocity;
    private float yInputVelocity;

    private void Update()
    {
        float xComponent = Mathf.SmoothDamp(transform.position.x, 
            transform.position.x + (Input.GetAxis("Horizontal") * speed), ref xInputVelocity, timeToMax);
        float yComponent = Mathf.SmoothDamp(transform.position.y, 
            transform.position.y + (Input.GetAxis("Vertical") * speed), ref yInputVelocity, timeToMax);

        newPositionVec = new Vector2(xComponent, yComponent);
    }

    void FixedUpdate()
    {
        if (hasAuthority)
        {
            rigidbody2D.MovePosition(newPositionVec);
        }
    }
}
