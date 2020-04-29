using UnityEngine;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

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
                 .Build();
        connection.Closed += async (error) =>
        {
            await Task.Delay(new System.Random().Next(0, 5) * 1000);
            await connection.StartAsync();
        };
    }

    private async void OnDestroy()
    {
        await connection.DisposeAsync();
    }
}
