using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;


public class LevelCreator : MonoBehaviour
{
    [SerializeField] private GameObject startRoom;
    [SerializeField] private GameObject portalRoom;
    [SerializeField] private GameObject finishRoom;
    [SerializeField] private GameObject[] roomPrefabs;
    [SerializeField] private int roomNumber;
    private const int Width = 24;
    private const int Height = 16;
    private List<GameObject> InstantiatedRooms { get; set; }
    private Queue<RoomContainer> RoomContainers { get; set; }
    private List<GameObject> DiedEnemies { get; set; }

    private class Room
    {
        public MoveDirection? previousPathDirection;
        public MoveDirection? nextPathDirection;
        public Vector3 position;
    }


    public void Start()
    {
        enabled = true;
        IsInitialized = false;
        RoomContainers = new Queue<RoomContainer>();
        InstantiatedRooms = new List<GameObject>();
        DiedEnemies = new List<GameObject>();
        GlobalEvents.OnEnemyDied.AddListener(enemy => DiedEnemies.Add(enemy));
        var directionByNumber = new Dictionary<int, MoveDirection>()
        {
            [1] = MoveDirection.Left,
            [2] = MoveDirection.Down,
            [3] = MoveDirection.Right,
        };
        var roomPath = new List<Room>();
        var currentRoomPosition = GameObject.Find("Player").transform.position;
        for (var i = 0; i < roomNumber + 2; i++)
        {
            var room = new Room();
            if (roomPath.Count == 0)
            {
                room.position = currentRoomPosition;
                room.nextPathDirection = directionByNumber[Random.Range(1, 4)];
                roomPath.Add(room);
            }
            else
            {
                room.previousPathDirection = ReverseDirection(roomPath.Last().nextPathDirection);
                if (roomPath.Count == roomNumber + 1)
                    room.nextPathDirection = MoveDirection.None;
                else
                    room.nextPathDirection = directionByNumber[Random.Range(1, 4)];
                while (room.previousPathDirection == room.nextPathDirection)
                {
                    room.nextPathDirection = directionByNumber[Random.Range(1, 4)];
                }

                switch (roomPath.Last().nextPathDirection)
                {
                    case MoveDirection.Left:
                        currentRoomPosition += new Vector3(-Width, 0);
                        room.position = currentRoomPosition;
                        roomPath.Add(room);
                        continue;
                    case MoveDirection.Down:
                        currentRoomPosition += new Vector3(0, -Height);
                        room.position = currentRoomPosition;
                        roomPath.Add(room);
                        continue;
                    case MoveDirection.Right:
                        currentRoomPosition += new Vector3(Width, 0);
                        room.position = currentRoomPosition;
                        roomPath.Add(room);
                        break;
                }
            }
        }

        var lastRoom = "";
        var number = 0;
        foreach (var room in roomPath)
        {
            GameObject roomPrefab;
            if (number == 0)
                roomPrefab = startRoom;
            else if (number == roomNumber + 1)
                roomPrefab = GameObject.Find("GameCreator").GetComponent<GameCreator>().levelCreators.Count == 0 ? finishRoom : portalRoom;
            else
                roomPrefab = roomPrefabs[Random.Range(1, roomPrefabs.Length - 2)];
            while (roomPrefab.name == lastRoom)
                roomPrefab = roomPrefabs[Random.Range(1, roomPrefabs.Length - 2)];
            lastRoom = roomPrefab.name;
            number += 1;
            var instantiatedRoom = Instantiate(roomPrefab, room.position, quaternion.identity);

            var roomSpawner = instantiatedRoom.GetComponent<RoomSpawner>();
            InstantiatedRooms.Add(instantiatedRoom);
            roomSpawner.directions = new List<MoveDirection?> {room.previousPathDirection, room.nextPathDirection};
            instantiatedRoom.transform.parent = transform;
        }
    }

    private void Update()
    {
        if (!IsInitialized)
        {
            if (InstantiatedRooms.Select(room => room.GetComponent<RoomSpawner>().RoomContainer)
                .All(roomContainer => roomContainer is not null))
            {
                foreach (var instantiatedRoom in InstantiatedRooms)
                {
                    RoomContainers.Enqueue(instantiatedRoom.GetComponent<RoomSpawner>().RoomContainer);
                }

                IsInitialized = true;
            }
        }
        else
        {
            if (RoomContainers.Count == 0)
            {
                enabled = false;
                return;
            }

            var roomContainer = RoomContainers.First();
            var i = 0;
            while (i < DiedEnemies.Count)
            {
                var enemy = DiedEnemies[i];
                if (roomContainer.Enemies.Remove(enemy))
                {
                    DiedEnemies.RemoveAt(i);
                }
                else
                    i++;
            }

            if (roomContainer.Enemies.Count != 0) return;
            if (roomContainer.NextPathDirection == MoveDirection.Down)
            {
                roomContainer.LowerDoors.ForEach(Destroy);
            }

            if (roomContainer.NextPathDirection == MoveDirection.Left)
            {
                roomContainer.LeftDoors.ForEach(Destroy);
            }

            if (roomContainer.NextPathDirection == MoveDirection.Right)
            {
                roomContainer.RightDoors.ForEach(Destroy);
            }

            RoomContainers.Dequeue();
            if (RoomContainers.Count == 0)
                return;
            roomContainer = RoomContainers.First();
            if (roomContainer.PreviousPathDirection == MoveDirection.Up)
            {
                roomContainer.UpperDoors.ForEach(Destroy);
            }

            if (roomContainer.PreviousPathDirection == MoveDirection.Down)
            {
                roomContainer.LowerDoors.ForEach(Destroy);
            }

            if (roomContainer.PreviousPathDirection == MoveDirection.Left)
            {
                roomContainer.LeftDoors.ForEach(Destroy);
            }

            if (roomContainer.PreviousPathDirection == MoveDirection.Right)
            {
                roomContainer.RightDoors.ForEach(Destroy);
            }
        }
    }

    private bool IsInitialized { get; set; }

    private static MoveDirection? ReverseDirection(MoveDirection? direction)
    {
        return direction switch
        {
            MoveDirection.Left => MoveDirection.Right,
            MoveDirection.Right => MoveDirection.Left,
            MoveDirection.Up => MoveDirection.Down,
            MoveDirection.Down => MoveDirection.Up,
            _ => direction
        };
    }
}