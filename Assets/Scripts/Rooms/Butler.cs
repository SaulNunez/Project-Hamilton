//using Microsoft.AspNetCore.SignalR.Client;
using Assets.Scripts.Rooms;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Butler : MonoBehaviour
{
    public GameObject topFloorRoomsParent;
    public GameObject basementRoomsParent;
    public GameObject mainFloorRoomsParent;

    [Serializable]
    public class RoomInfo
    {
        public GameObject roomGameObject;
        public string name;
        public int x;
        public int y;
        public Floors floor;

        public Vector2 PositionOnWorld() => new Vector2(x * 8, y * 8);
    }

    private readonly List<RoomInfo> rooms = new List<RoomInfo>();

    private Floors currentFloor;

    private void Start()
    {
        HamiltonHub.Instance.OnGameHasStarted += GetRoomPositions;
    }
    

    private void GetRoomPositions(Assets.Scripts.Multiplayer.ResultPayload.GameStartPayload gameStartInfo)
    {
        gameStartInfo.RoomPositions.MainFloor.ForEach(rInfo =>
        {
            var roomGameObject = mainFloorRoomsParent.transform.Find(rInfo.RoomId);
            if(roomGameObject != null)
            {
                var roomInfo = new RoomInfo
                {
                    roomGameObject = roomGameObject.gameObject,
                    name = rInfo.Name,
                    x = rInfo.X,
                    y = rInfo.Y,
                    floor = Floors.MAIN_FLOOR
                };

                rooms.Add(roomInfo);

                roomGameObject.position = roomInfo.PositionOnWorld();
                // Habilitar cuando esten en la lista, en caso que no todos esten en la lista que nos manda el servidor
                roomGameObject.gameObject.SetActive(true);
            } else
            {
                Debug.LogError($"Didn't found room with name: {rInfo.RoomId}");
            }
        });

        gameStartInfo.RoomPositions.Basement.ForEach(rInfo =>
        {
            var roomGameObject = basementRoomsParent.transform.Find(rInfo.RoomId);
            if (roomGameObject != null)
            {
                var roomInfo = new RoomInfo
                {
                    roomGameObject = roomGameObject.gameObject,
                    name = rInfo.Name,
                    x = rInfo.X,
                    y = rInfo.Y,
                    floor = Floors.BASEMENT
                };

                rooms.Add(roomInfo);

                roomGameObject.position = roomInfo.PositionOnWorld();
                // Habilitar cuando esten en la lista, en caso que no todos esten en la lista que nos manda el servidor
                roomGameObject.gameObject.SetActive(false);
            }
            else
            {
                Debug.LogError($"Didn't found room with name: {rInfo.RoomId}");
            }
        });

        gameStartInfo.RoomPositions.TopFloor.ForEach(rInfo =>
        {
            var roomGameObject = topFloorRoomsParent.transform.Find(rInfo.RoomId);
            if (roomGameObject != null)
            {
                var roomInfo = new RoomInfo
                {
                    roomGameObject = roomGameObject.gameObject,
                    name = rInfo.Name,
                    x = rInfo.X,
                    y = rInfo.Y,
                    floor = Floors.TOP_FLOOR
                };

                rooms.Add(roomInfo);

                roomGameObject.position = roomInfo.PositionOnWorld();
                // Habilitar cuando esten en la lista, en caso que no todos esten en la lista que nos manda el servidor
                roomGameObject.gameObject.SetActive(false);
            }
            else
            {
                Debug.LogError($"Didn't found room with name: {rInfo.RoomId}");
            }
        });
    }

    public void SetFloorAsActive(Floors floor)
    {
        currentFloor = floor;

        switch (floor)
        {
            case Floors.BASEMENT:
                basementRoomsParent.SetActive(false);
                break;
            case Floors.MAIN_FLOOR:
                mainFloorRoomsParent.SetActive(false);
                break;
            case Floors.TOP_FLOOR:
                topFloorRoomsParent.SetActive(false);
                break;
        }
    }

    private void OnDestroy()
    {
        HamiltonHub.Instance.OnGameHasStarted -= GetRoomPositions;
    }
}
