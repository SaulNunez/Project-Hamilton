using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : NetworkBehaviour
{
    public float speed = 4f;
    public float timeToMax = 0.4f;
    public new Rigidbody2D rigidbody2D;

    void FixedUpdate()
    {
        if (hasAuthority)
        {
            var x = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
            var y = Input.GetAxis("Vertical") * speed * Time.deltaTime;

            rigidbody2D.MovePosition(new Vector2(x + transform.position.x, y + transform.position.y));
        }
    }
}
