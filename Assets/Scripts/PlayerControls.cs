using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : NetworkBehaviour
{
    public float speed = 4f;
    public float timeToMax = 0.4f;
    public new Rigidbody2D rigidbody2D;

    bool waitToCloseUi = false;

    public override void OnStartAuthority()
    {
        base.OnStartAuthority();

        PuzzleUI.OnOpenPuzzleInClient += OnUIShown;
        PuzzleUI.OnClosePuzzleInClient += OnUiClose;
    }

    private void OnUiClose()
    {
        waitToCloseUi = false;
    }

    private void OnUIShown()
    {
        waitToCloseUi = true;
    }

    void FixedUpdate()
    {
        if (hasAuthority && !waitToCloseUi)
        {
            var x = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
            var y = Input.GetAxis("Vertical") * speed * Time.deltaTime;

            rigidbody2D.MovePosition(new Vector2(x + transform.position.x, y + transform.position.y));
        }
    }

    public override void OnStopAuthority()
    {
        base.OnStopAuthority();


    }
}
