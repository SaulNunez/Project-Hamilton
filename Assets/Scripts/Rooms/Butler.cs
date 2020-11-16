//using Microsoft.AspNetCore.SignalR.Client;
using Assets.Scripts.Rooms;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Butler : MonoBehaviour
{
    public GameObject topFloorRoomsParent;
    public GameObject basementRoomsParent;
    public GameObject mainFloorRoomsParent;

    private readonly Dictionary<string, GameObject> topFloor = new Dictionary<string, GameObject>();
    private readonly Dictionary<string, GameObject> basement = new Dictionary<string, GameObject>();
    private readonly Dictionary<string, GameObject> mainFloor = new Dictionary<string, GameObject>();

    private readonly Dictionary<Position, string> mainFloorRoomPositions = new Dictionary<Position, string>();
    private readonly Dictionary<Position, string> basementRoomPositions = new Dictionary<Position, string>();
    private readonly Dictionary<Position, string> topFloorRoomPositions = new Dictionary<Position, string>();

    private string currentRoom;
    private Floors currentFloor;

    private void Start()
    {
        HamiltonHub.Instance.OnGameHasStarted += GetRoomPositions;
        foreach (Transform room in topFloorRoomsParent.transform)
        {
            topFloor.Add(room.gameObject.name, room.gameObject);
        }

        foreach (Transform room in basementRoomsParent.transform)
        {
            basement.Add(room.gameObject.name, room.gameObject);
        }

        foreach (Transform room in mainFloorRoomsParent.transform)
        {
            mainFloor.Add(room.gameObject.name, room.gameObject);
        }

        SetRoomAsActive("entrance", Floors.MAIN_FLOOR);
    }

    private void GetRoomPositions(Assets.Scripts.Multiplayer.ResultPayload.GameStartPayload gameStartInfo)
    {
        gameStartInfo.RoomPositions.MainFloor.ForEach(rInfo =>
        {
            mainFloorRoomPositions.Add(new Position(rInfo.X, rInfo.Y), rInfo.RoomId);
        });

        gameStartInfo.RoomPositions.Basement.ForEach(rInfo =>
        {
            basementRoomPositions.Add(new Position(rInfo.X, rInfo.Y), rInfo.RoomId);
        });

        gameStartInfo.RoomPositions.TopFloor.ForEach(rInfo =>
        {
            topFloorRoomPositions.Add(new Position(rInfo.X, rInfo.Y), rInfo.RoomId);
        });
    }

    public void SetRoomAsActive(int x, int y, Floors floor)
    {
        switch (floor)
        {
            case Floors.BASEMENT:
                SetRoomAsActive(basementRoomPositions[new Position(x, y)], floor);
                break;
            case Floors.MAIN_FLOOR:
                SetRoomAsActive(mainFloorRoomPositions[new Position(x, y)], floor);
                break;
            case Floors.TOP_FLOOR:
                SetRoomAsActive(topFloorRoomPositions[new Position(x, y)], floor);
                break;
        }
    }

    public void SetRoomAsActive(string room, Floors floor)
    {
        if(room != currentRoom && floor != currentFloor)
        {
            return;
        }

        switch (currentFloor)
        {
            case Floors.BASEMENT:
                foreach(GameObject r in basement.Values)
                {
                    r.SetActive(false);
                }
                break;
            case Floors.MAIN_FLOOR:
                foreach (GameObject r in mainFloor.Values)
                {
                    r.SetActive(false);
                }
                break;
            case Floors.TOP_FLOOR:
                foreach (GameObject r in topFloor.Values)
                {
                    r.SetActive(false);
                }
                break;
        }

        switch (currentFloor)
        {
            case Floors.BASEMENT:
                basement[room].SetActive(true);
                break;
            case Floors.MAIN_FLOOR:
                mainFloor[room].SetActive(true);
                break;
            case Floors.TOP_FLOOR:
                topFloor[room].SetActive(true);
                break;
        }

        currentRoom = room;
        currentFloor = floor;
    }

    private void OnDestroy()
    {
        HamiltonHub.Instance.OnGameHasStarted -= GetRoomPositions;
    }
}
