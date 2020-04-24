using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.AspNetCore.SignalR.Client;

public class CharacterMovement : MonoBehaviour
{
    public GameObject ui;

    private int x;
    private int y;

    public bool CanGoUp() => false;
    public bool CanGoDown() => false;
    public bool CanGoLeft() => false;
    public bool CanGoRight() => false;

    public async void MoveUp()
    {
        await Socket.Instance.connection.InvokeAsync("GoToTop", new {
            UserAuth = "",
            HubCode = ""
        });
    }

    public async void MoveDown()
    {
        await Socket.Instance.connection.InvokeAsync("GoToBottom", new {
            UserAuth = "",
            HubCode = ""
        });
    }

    public async void MoveLeft()
    {
        await Socket.Instance.connection.InvokeAsync("GoToLeft", new {
            UserAuth = "",
            HubCode = ""
        });
    }

    public async void MoveRight()
    {
        await Socket.Instance.connection.InvokeAsync("GoToRight", new {
            UserAuth = "",
            HubCode = ""
        });
    }

    public void ShowMovementUI()
    {
        ui.SetActive(true);
    }

    void HideMovementUI()
    {
        ui.SetActive(false);
    }

    private void Awake()
    {
        Socket.Instance.connection.On<string, int, int>("MoveTo", (character, x, y) =>
        {
            this.x = x;
            this.y = y;
        });
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveLeft();
            HideMovementUI();
        } else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveRight();
            HideMovementUI();
        } else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveUp();
            HideMovementUI();
        } else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveDown();
            HideMovementUI();
        }
    }
}
