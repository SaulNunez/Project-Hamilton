using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFlipOnVelocity : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Vector2 velocityDelta;
    Vector2 lastPosition;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        lastPosition = transform.position;
    }

    void FixedUpdate()
    {
        UpdateVelocityDelta();
        if (velocityDelta.x > 0)
        {
            spriteRenderer.flipX = false;
        } else if(velocityDelta.x < 0)
        {
            spriteRenderer.flipX = true;
        }
    }

    void UpdateVelocityDelta()
    {
        velocityDelta = (Vector2)transform.position - lastPosition;
        lastPosition = transform.position;
    }
}
