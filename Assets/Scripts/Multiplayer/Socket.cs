using System.Collections.Generic;
using UnityEngine;
using System;
using Microsoft.AspNetCore.SignalR.Client;
using System.Threading.Tasks;

public class Socket: MonoBehaviour
{
    public static Socket Instance;
    public string url;
    public HubConnection connection;

    private async void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        } else
        {
            Debug.LogWarning($"Only a single instance of Socket is needed, killing this.");
            Destroy(this);
        }

        connection = new HubConnectionBuilder()
                 //Servidor esta hospedado en el mismo lugar que el juego
                 .WithUrl($"{Application.absoluteURL}/game")
                 .WithAutomaticReconnect()
                 .Build();
        connection.Closed += async (error) =>
        {
            //await Task.Delay(new System.Random().Next(0, 5) * 1000);
            //await connection.StartAsync();
        };

        connection.Reconnecting += error =>
        {
            return Task.CompletedTask;
        };

        try
        {
            await connection.StartAsync();
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
        }
    }

    private async void OnDestroy()
    {
        await connection.DisposeAsync();
    }
}
