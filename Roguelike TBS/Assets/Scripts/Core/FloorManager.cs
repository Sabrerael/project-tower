using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour {
    [SerializeField] GameObject[] roomArray = null;
    [SerializeField] GameObject[] doorArray = null;
    [SerializeField] int numberOfRooms = 10;
    [SerializeField] GameObject startingRoom = null;
    [SerializeField] GameObject endRoom = null;
    [SerializeField] GameObject shopRoom = null;
    [SerializeField] int shopRoomIndex = 8;
    [SerializeField] float xDeltaBetweenRooms = 18;
    [SerializeField] float yDeltaBetweenRooms = 14;

    [SerializeField] Character[] characters = null;
    [SerializeField] PauseMenu pauseMenu = null;

    // CACHE
    private GameObject roomsParent = null;
    private List<GameObject> rooms = new List<GameObject>();
    private GameManager gameManager = null;
    private LootManager lootManager = null;

    private int levelOfEnemies = 1;

    private void Awake() {
        gameManager = GameManager.instance;
        lootManager = LootManager.instance;
        
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player) {
            player.transform.position = new Vector3(6, -6, 0);
            player.GetComponent<Movement>().UpdateMinMaxValues();
        } else {
            Instantiate(characters[(int)gameManager.GetPlayerCharacter()], new Vector3(6, -6, 0), Quaternion.identity);
        }

        GenerateLevel();
    }

    public void TogglePause() {
        if (!pauseMenu.GetMenuBodyActive()) {
            PauseGame();
        } else if (pauseMenu.GetMenuBodyActive()) {
            UnPauseGame();
        }
    }

    private void GenerateLevel() {
        roomsParent = GameObject.Find("Rooms");

        // Add starting room to the array of rooms
        rooms.Add(startingRoom);

        for (int i = 0; i < rooms.Count; i++) {
            if (rooms.Count%5 == 0) {
                levelOfEnemies++;
            }

            MakeNewDoors(rooms[i]); 
            CreateNewRooms(rooms, rooms[i]);
        }

        List<InventoryItem> lootDrops = lootManager.GetRandomLootItems(rooms.Count / 4);
        foreach (InventoryItem item in lootDrops) {
            bool lootSet = false;
            do {
                int index = Random.Range(1, rooms.Count);
                if (rooms[index].GetComponent<RoomManager>().GetDropSpawnerItem() == null) {
                    AddLootToRoom(rooms[index], item);
                    lootSet = true;
                }
            } while (!lootSet);
        }
    }

    private void MakeNewDoors(GameObject room) {
        // Immediately return if number of rooms is reached.
        if (rooms.Count == numberOfRooms) { return; }

        var doorsInRoom = room.GetComponentsInChildren<RoomMovement>();
        List<Direction> directionsOfDoors = new List<Direction>();

        foreach (RoomMovement door in doorsInRoom) {
            directionsOfDoors.Add(door.GetDirection());
        } 

        var numberOfNewDoors = (Random.Range(1,3));


        if (numberOfNewDoors > (numberOfRooms - rooms.Count)) {
            numberOfNewDoors = numberOfRooms - rooms.Count;
        }

        while (numberOfNewDoors != 0) {
            int newDoorDirectionValue;
            Direction newDoorDirection;
            do {
                newDoorDirectionValue = Random.Range(0, 4);
                newDoorDirection = (Direction)newDoorDirectionValue;
            } while (directionsOfDoors.Contains(newDoorDirection));

            var door = Instantiate(doorArray[newDoorDirectionValue], room.transform);
            directionsOfDoors.Add(newDoorDirection);
            numberOfNewDoors--;
        }
        
    }

    private void CreateNewRooms(List<GameObject> rooms, GameObject currentRoom) {
        if (rooms.Count == numberOfRooms) { return; }

        var doorsInRoom = currentRoom.GetComponentsInChildren<RoomMovement>();
        var currentRoomPosition = currentRoom.transform.position;

        for (int i = 0; i < doorsInRoom.Length; i++) {
            if (doorsInRoom[i].GetNewRoomManager() != null) {
                continue;
            }

            var newRoomPosition = currentRoomPosition;
            int doorArrayIndex = 0;

            if (doorsInRoom[i].GetDirection() == Direction.Left) {
                newRoomPosition -= (Vector3.right * xDeltaBetweenRooms);
                doorArrayIndex = 2;
            } else if (doorsInRoom[i].GetDirection() == Direction.Up) {
                newRoomPosition += (Vector3.up * yDeltaBetweenRooms);
                doorArrayIndex = 1;
            } else if (doorsInRoom[i].GetDirection() == Direction.Right) {
                newRoomPosition += (Vector3.right * xDeltaBetweenRooms);
                doorArrayIndex = 3;
            } else if (doorsInRoom[i].GetDirection() == Direction.Down) {
                newRoomPosition -= (Vector3.up * yDeltaBetweenRooms);
                doorArrayIndex = 0;
            }

            var existingRoom = CheckForExistingRoom(newRoomPosition);

            if (existingRoom != null) {
                var door = Instantiate(doorArray[doorArrayIndex], existingRoom.transform);
                door.GetComponent<RoomMovement>().SetNewRoomManager(currentRoom.GetComponent<RoomManager>());
                doorsInRoom[i].SetNewRoomManager(existingRoom.GetComponent<RoomManager>());
            } else if (numberOfRooms - 1 == rooms.Count) {
                var newRoom = Instantiate(endRoom, newRoomPosition, Quaternion.identity, roomsParent.transform);
                var door = Instantiate(doorArray[doorArrayIndex], newRoom.transform);
                door.GetComponent<RoomMovement>().SetNewRoomManager(currentRoom.GetComponent<RoomManager>());
                doorsInRoom[i].SetNewRoomManager(newRoom.GetComponent<RoomManager>());
                rooms.Add(newRoom);
            } else if (rooms.Count == shopRoomIndex) { 
                var newRoom = Instantiate(shopRoom, newRoomPosition, Quaternion.identity, roomsParent.transform);
                var door = Instantiate(doorArray[doorArrayIndex], newRoom.transform);
                door.GetComponent<RoomMovement>().SetNewRoomManager(currentRoom.GetComponent<RoomManager>());
                doorsInRoom[i].SetNewRoomManager(newRoom.GetComponent<RoomManager>());
                newRoom.GetComponent<ShopRoomManager>().SetShopTables();
                rooms.Add(newRoom);
            } else {
                var newRoom = Instantiate(roomArray[Random.Range(0,roomArray.Length)], newRoomPosition, Quaternion.identity, roomsParent.transform);
                var door = Instantiate(doorArray[doorArrayIndex], newRoom.transform);
                door.GetComponent<RoomMovement>().SetNewRoomManager(currentRoom.GetComponent<RoomManager>());
                doorsInRoom[i].SetNewRoomManager(newRoom.GetComponent<RoomManager>());
                SetEnemyLevels(newRoom);
                rooms.Add(newRoom);
            }
        }
    }

    private GameObject CheckForExistingRoom(Vector3 newRoomPosition) {
        var roomTransforms = roomsParent.GetComponentsInChildren<Transform>();

        foreach(Transform transform in roomTransforms) {
            if (transform.position == newRoomPosition) {
                return transform.gameObject;
            }
        }

        return null;
    }

    private void AddLootToRoom(GameObject room, InventoryItem item) {
        room.GetComponent<RoomManager>().ConfigureDropSpawner(item, 1);
    }

    private void SetEnemyLevels(GameObject room) {
        var enemySpawners = room.transform.Find("Spawners").GetComponentsInChildren<EnemySpawner>();
        foreach(var spawner in enemySpawners) {
            spawner.SetEnemyLevel(levelOfEnemies);
        }
    }

    public void PauseGame() {
        Time.timeScale = 0;
        pauseMenu.ToggleBodyActive();
    }

    public void UnPauseGame() {
        Time.timeScale = 1;
        pauseMenu.ToggleBodyActive();
    }
}
