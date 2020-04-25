using Microsoft.AspNetCore.SignalR.Client;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Butler : MonoBehaviour
{
    public Dictionary<Position, GameObject> rooms = new Dictionary<Position, GameObject>();

    void Awake()
    {
        Socket.Instance.connection.On<Dictionary<Position, string>>("SetRoomList", (rooms) =>
        {

        });
    }
}
