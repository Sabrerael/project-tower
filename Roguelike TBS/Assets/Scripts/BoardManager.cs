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

    public int columns = 6;
    public int rows = 6;
    public int rooms = 2;
    public Count enemyCount = new Count(1,3);
    //    public GameObject start;
    //    public GameObject exit;
    public GameObject[] roomTiles;
    public GameObject[] enemyTiles;

    private Transform boardHolder;
    private List<Vector3> gridPositions = new List<Vector3>();
    private List<GameObject> roomsOnBoard = new List<GameObject>();

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

                if (roomsOnBoard.Count == rooms) { break; }
                lastRoom = "Right";
                Debug.Log(lastRoom);
            }
            else if (direction == 2) {
                instance = Instantiate(roomTiles[roomTile], instance.transform.position + (2*Vector3.up), Quaternion.identity) as GameObject;
                instance.transform.SetParent(boardHolder);
                roomsOnBoard.Add(instance);
                Debug.Log("Instance added to Board");

                if (roomsOnBoard.Count == rooms) { break; }
                lastRoom = "Up";
                Debug.Log(lastRoom);
            }
            /*else if (direction == 3) {
                instance = Instantiate(roomTiles[roomTile], instance.transform.position + (2*Vector3.left), Quaternion.identity) as GameObject;
                instance.transform.SetParent(boardHolder);
                roomsOnBoard.Add(instance);
                Debug.Log("Instance added to Board");

                if (roomsOnBoard.Count == rooms) { break; }
                lastRoom = "Left";
                Debug.Log(lastRoom);
            }*/
            else if (direction == 3) {
                instance = Instantiate(roomTiles[roomTile], instance.transform.position + (2*Vector3.down), Quaternion.identity) as GameObject;
                instance.transform.SetParent(boardHolder);
                roomsOnBoard.Add(instance);
                Debug.Log("Instance added to Board");

                if (roomsOnBoard.Count == rooms) { break; }
                lastRoom = "Down";
                Debug.Log(lastRoom);
            }
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
}
