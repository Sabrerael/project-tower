using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour {

    [Serializable]
    public class Count {
        public int minimum;
        public int maximum;

        public Count (int min, int max) {
            minimum = min;
            maximum = max;
        }
    }

    private class Room
    {
        private bool north;
        private bool south;
        private bool east;
        private bool west;
        private int[] position;
        private List<Enemy> enemies = new List<Enemy>();
        private List<Weapon> weapons = new List<Weapon>();

        public bool getNorth() { return north; }
        public bool getSouth() { return south; }
        public bool getEast() { return east; }
        public bool getWest() { return west; }
        public int[] getPosition() { return position; }
        public List<Enemy> getEnemies() { return enemies; }
        public List<Weapon> getWeapons() { return weapons; }

        public void setNorth(bool val) { north = val; }
        public void setSouth(bool val) { south = val; }
        public void setEast(bool val) { east = val; }
        public void setWest(bool val) { west = val; }
        public void setPosition(int[] val) { position = val; }
        public void setEnemies(List<Enemy> val) { enemies = val; }
        public void setWeapons(List<Weapon> val) { weapons = val; }
        public void addWeapon(Weapon val) { weapons.Add(val); }
    }

    public int columns = 6;
    public int rows = 6;
    public int rooms = 2;
    public Count enemyCount = new Count(1,3);
    public GameObject[] roomTiles;
    public GameObject[] enemyTiles;

    private Transform boardHolder;
    private List<Vector3> gridPositions = new List<Vector3>();
    private List<GameObject> roomsOnBoard = new List<GameObject>();
    private List<Room> testRoomsOnBoard = new List<Room>();

    void InitializeList() {
        gridPositions.Clear();

        for (int x=0; x < rooms; x++)
        {
            for (int y = 1; y < columns; y++)
            {
                for (int z = 1; z < rows; z++)
                {
                    gridPositions.Add(new Vector3(x*7 + y, z, 0f));
                }
            }
        }
        
    }

    void BoardSetup () {
        boardHolder = new GameObject ("Board").transform;
        string lastRoom = "";
        int roomTile;

        GameObject instance = Instantiate(roomTiles[0], new Vector3(0f, 0f, 0f), Quaternion.identity) as GameObject;
        roomsOnBoard.Add(instance);
        instance.transform.SetParent(boardHolder);

        Room newRoom = new Room();
        int[] roomPosition = {0, 0};

        newRoom.setPosition(roomPosition);

        for (int i = 0; i < rooms; i++) {
            int direction;
            if (lastRoom == "Up") {
                direction = Random.Range(1, 3);
            }
            else if (lastRoom == "Down") {
                direction = Random.Range(1,3);
                if (direction == 2) { direction = 3; }
            }
            else {
                direction = Random.Range(1, 4);
            }

            roomTile = Random.Range(0, 3);

            if (direction == 1) {
                instance = Instantiate(roomTiles[roomTile], instance.transform.position + (2*Vector3.right), Quaternion.identity) as GameObject;
                instance.transform.SetParent(boardHolder);
                roomsOnBoard.Add(instance);
                Debug.Log("Instance added to Board");

                newRoom.setEast(true);
                testRoomsOnBoard.Add(newRoom);
                Debug.Log("New room added to List, Going east");
                newRoom = new Room();
                roomPosition[0] = (int)instance.transform.position.x;
                roomPosition[1] = (int)instance.transform.position.y;
                newRoom.setPosition(roomPosition);
                newRoom.setWest(true);

                if (roomsOnBoard.Count == rooms) {
                    testRoomsOnBoard.Add(newRoom);
                    Debug.Log("New room added to List, last room");
                    break; 
                }
                lastRoom = "Right";
            }
            else if (direction == 2) {
                instance = Instantiate(roomTiles[roomTile], instance.transform.position + (2*Vector3.up), Quaternion.identity) as GameObject;
                instance.transform.SetParent(boardHolder);
                roomsOnBoard.Add(instance);
                Debug.Log("Instance added to Board");

                newRoom.setNorth(true);
                testRoomsOnBoard.Add(newRoom);
                Debug.Log("New room added to List, Going north");
                newRoom = new Room();
                roomPosition[0] = (int)instance.transform.position.x;
                roomPosition[1] = (int)instance.transform.position.y;
                newRoom.setPosition(roomPosition);
                newRoom.setSouth(true);

                if (roomsOnBoard.Count == rooms) {
                    testRoomsOnBoard.Add(newRoom);
                    Debug.Log("New room added to List, last room");
                    break; 
                }
                lastRoom = "Up";
            } else if (direction == 3) {
                instance = Instantiate(roomTiles[roomTile], instance.transform.position + (2*Vector3.down), Quaternion.identity) as GameObject;
                instance.transform.SetParent(boardHolder);
                roomsOnBoard.Add(instance);
                Debug.Log("Instance added to Board");

                newRoom.setSouth(true);
                testRoomsOnBoard.Add(newRoom);
                Debug.Log("New room added to List, Going south");
                newRoom = new Room();
                roomPosition[0] = (int)instance.transform.position.x;
                roomPosition[1] = (int)instance.transform.position.y;
                newRoom.setPosition(roomPosition);
                newRoom.setNorth(true);

                if (roomsOnBoard.Count == rooms)
                {
                    testRoomsOnBoard.Add(newRoom);
                    Debug.Log("New room added to List, last room");
                    break;
                }
                lastRoom = "Down";
            }

            //newRoom = createLootForRoom(newRoom);
        }
    }

    //Here for possible room mapping 
    /*void RandomFillMap() {
        string seed = Time.time.ToString();

        System.Random pseudoRandom = new System.Random(seed.GetHashCode());

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1) {
                    map[x, y] = 1;
                } else {
                    map[x, y] = (pseudoRandom.Next(0, 100) < randomFillPercent) ? 1 : 0;
                }
            }
        }
    }*/

    Vector3 RandomPosition (int room) {
        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector3 randomPosition = gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex);
        return randomPosition;
    }

    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum) {
        for (int x=0; x<rooms; x++)
        {
            int objectCount = Random.Range(minimum, maximum);
            for (int i = 0; i < objectCount; i++)
            {
                Vector3 randomPosition = RandomPosition(x);
                GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
                Instantiate(tileChoice, randomPosition, Quaternion.identity);
                Debug.Log("Enemy instantiated at " + randomPosition);
            }
        }
    }

    public void SetupScene(int level) {
        BoardSetup();
        InitializeList();
        //LayoutObjectAtRandom(enemyTiles, enemyCount.minimum, enemyCount.maximum);
        //LayoutObjectAtRandom(foodTiles, foodCount.minimum, wallCount.maximum);
        //int enemyCount = (int)Mathf.Log(level, 2f);
        //LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);
        //Instantiate(exit, new Vector3(columns - 1, rows - 1, 0f), Quaternion.identity);
    }

    /*private Room createLootForRoom(Room room) {
        int itemId;
        Item loot;
        Weapon weapon;

        int weaponOrItem = Random.Range(1, 3);
        
        if (weaponOrItem == 1) {
            itemId = 1000 + Random.Range(1, 6);
            loot = createItemById(itemId);

            Debug.Log("Adding " + loot + " to the room loot");
            room.addItem(loot);
        } else if (weaponOrItem == 2) {
            itemId = Random.Range(1, 6);
            weapon = createWeaponById(itemId);

            Debug.Log("Adding " + weapon + " to the room loot");
            room.addWeapon(weapon);
        }

        return room;
    }

    public Item createItemById(int id)
    {
        Item newItem = new Item();
        if (id == 1001) {
            newItem = new HealthPotion();
        } else if (id == 1002) {
            newItem = new StrengthPotion();
        } else if (id == 1003) {
            newItem = new Rock();
        } else if (id == 1004) {
            newItem = new ThrowingKnife();
        } else if (id == 1005) {
            newItem = new LiquidFire();
        } else {
            return null;
        }
        return newItem;
    }

    public Weapon createWeaponById(int id)
    {
        Weapon newWeapon = new Weapon();
        if (id == 1) {
            newWeapon = new LongSword();
        } else if (id == 2) {
            newWeapon = new Hammer();
        } else if (id == 3) {
            newWeapon = new Hatchet();
        } else if (id == 4) {
            newWeapon = new Halberd();
        } else if (id == 5) {
            newWeapon = new Katana();
        } else {
            return null;
        }
        return newWeapon;
    }*/
}
