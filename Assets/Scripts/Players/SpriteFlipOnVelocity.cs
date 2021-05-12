using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFlipOnVelocity : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    new Rigidbody2D rigidbody2D;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(rigidbody2D.velocity.x > 0)
        {
            spriteRenderer.flipX = false;
        } else if(rigidbody2D.velocity.x < 0)
        {
            spriteRenderer.flipX = true;
        }
    }
}
