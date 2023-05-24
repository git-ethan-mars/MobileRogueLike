using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RoomContainer
{
    public List<GameObject> Grounds { get; }
    public List<GameObject> Walls { get; }
    public List<GameObject> Enemies { get; }
    public List<GameObject> LeftDoors { get; }
    public List<GameObject> UpperDoors { get; }
    public List<GameObject> RightDoors { get; }
    public List<GameObject> LowerDoors { get; }
    public List<GameObject> Spikes { get; }
    public List<GameObject> Chests { get; }
    public List<GameObject> Platforms { get; }
    public MoveDirection? PreviousPathDirection { get; }
    public MoveDirection? NextPathDirection { get; }

    public RoomContainer(MoveDirection? previousPathDirection, MoveDirection? nextPathDirection)
    {
        Grounds = new List<GameObject>();
        Walls = new List<GameObject>();
        Enemies = new List<GameObject>();
        LeftDoors = new List<GameObject>();
        RightDoors = new List<GameObject>();
        UpperDoors = new List<GameObject>();
        LowerDoors = new List<GameObject>();
        Spikes = new List<GameObject>();
        Chests = new List<GameObject>();
        Platforms = new List<GameObject>();
        PreviousPathDirection = previousPathDirection;
        NextPathDirection = nextPathDirection;
    }
}