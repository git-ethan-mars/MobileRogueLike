using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RoomSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] chestPrefabs;
    [SerializeField] private GameObject[] groundPrefabs;
    [SerializeField] private GameObject[] wallPrefabs;
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private GameObject[] spikePrefabs;
    [SerializeField] private GameObject[] platformPrefabs;
    [SerializeField] private GameObject[] jailGroundPrefabs;
    [SerializeField] private GameObject[] jailWallPrefabs;
    public List<MoveDirection?> directions;
    public RoomContainer RoomContainer { get; set; }

    private void Start()
    {
        RoomContainer = new RoomContainer(directions[0], directions[1]);
        foreach (Transform child in transform)
        {
            if (child.name == "GroundSpawners")
            {
                foreach (Transform grandChild in child)
                {
                    var instantiatedObject = Instantiate(groundPrefabs[Random.Range(0, groundPrefabs.Length)],
                        grandChild.position,
                        grandChild.rotation);
                    instantiatedObject.transform.parent = transform;
                    RoomContainer.Grounds.Add(instantiatedObject);
                }
            }
            else if (child.name == "WallSpawners")
            {
                foreach (Transform grandChild in child)
                {
                    var instantiatedObject = Instantiate(wallPrefabs[Random.Range(0, wallPrefabs.Length)],
                        grandChild.position,
                        grandChild.rotation);
                    instantiatedObject.transform.parent = transform;
                    RoomContainer.Walls.Add(instantiatedObject);
                }
            }
            else if (child.name == "EnemySpawners")
            {
                foreach (Transform grandChild in child)
                {
                    var instantiatedObject = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)],
                        grandChild.position,
                        grandChild.rotation);
                    instantiatedObject.transform.parent = transform;
                    RoomContainer.Enemies.Add(instantiatedObject);
                }
            }
            else if (child.name == "SpikeSpawners")
            {
                foreach (Transform grandChild in child)
                {
                    var instantiatedObject = Instantiate(spikePrefabs[Random.Range(0, spikePrefabs.Length)],
                        grandChild.position,
                        grandChild.rotation);
                    instantiatedObject.transform.parent = transform;
                    RoomContainer.Spikes.Add(instantiatedObject);
                }
            }
            else if (child.name == "PlatformSpawners")
            {
                foreach (Transform grandChild in child)
                {
                    var instantiatedObject = Instantiate(platformPrefabs[Random.Range(0, platformPrefabs.Length)],
                        grandChild.position,
                        grandChild.rotation);
                    instantiatedObject.transform.parent = transform;
                    RoomContainer.Platforms.Add(instantiatedObject);
                }
            }
            else if (child.name == "ChestSpawners")
            {
                var count = 0;
                var number = Random.Range(0, 2);
                foreach (Transform grandChild in child.transform)
                {
                    if (count == number)
                    {
                        var instantiatedObject = Instantiate(chestPrefabs[0], grandChild.position,
                            grandChild.rotation);
                        instantiatedObject.transform.parent = transform;
                        RoomContainer.Chests.Add(instantiatedObject);
                    }

                    count += 1;
                }
            }
        }

        CreateDoors();
    }

    private void CreateDoors()
    {
        foreach (Transform doorSpawner in transform.Find("DoorSpawners"))
        {
            if (doorSpawner.name == "LeftDoorSpawner" &&
                directions.Contains(MoveDirection.Left))
            {
                var instantiatedObject = Instantiate(jailWallPrefabs[Random.Range(0, jailWallPrefabs.Length)],
                    doorSpawner.position,
                    doorSpawner.rotation);
                instantiatedObject.transform.parent = transform;
                RoomContainer.LeftDoors.Add(instantiatedObject);
                continue;
            }

            if (doorSpawner.name == "RightDoorSpawner" && directions.Contains(MoveDirection.Right))
            {
                var instantiatedObject = Instantiate(jailWallPrefabs[Random.Range(0, jailWallPrefabs.Length)],
                    doorSpawner.position,
                    doorSpawner.rotation);
                instantiatedObject.transform.parent = transform;
                RoomContainer.RightDoors.Add(instantiatedObject);
                continue;
            }

            if (doorSpawner.name == "UpDoorSpawner" &&
                directions.Contains(MoveDirection.Up))
            {
                var instantiatedObject = Instantiate(jailGroundPrefabs[Random.Range(0, jailGroundPrefabs.Length)],
                    doorSpawner.position,
                    doorSpawner.rotation);
                instantiatedObject.transform.parent = transform;
                RoomContainer.UpperDoors.Add(instantiatedObject);
                continue;
            }

            if (doorSpawner.name == "DownDoorSpawner" &&
                directions.Contains(MoveDirection.Down))
            {
                var instantiatedObject = Instantiate(jailGroundPrefabs[Random.Range(0, jailGroundPrefabs.Length)],
                    doorSpawner.position,
                    doorSpawner.rotation);
                instantiatedObject.transform.parent = transform;
                    RoomContainer.LowerDoors.Add(instantiatedObject);
                continue;
            }

            if (doorSpawner.name is "LeftDoorSpawner" or "RightDoorSpawner")
            {
                var instantiatedObject = Instantiate(wallPrefabs[Random.Range(0, wallPrefabs.Length)],
                    doorSpawner.position, doorSpawner.rotation);
                instantiatedObject.transform.parent = transform;
                    RoomContainer.Walls.Add(instantiatedObject);
            }
            else
            {
                var instantiatedObject = Instantiate(groundPrefabs[Random.Range(0, groundPrefabs.Length)],
                    doorSpawner.position, doorSpawner.rotation);
                instantiatedObject.transform.parent = transform;
                    RoomContainer.Grounds.Add(instantiatedObject);
            }
        }
    }
}