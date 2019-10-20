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
    public GameObject[] floorTiles;
    public GameObject[] enemyTiles;

    private Transform boardHolder;
    private List<Vector3> gridPositions = new List<Vector3>();
 

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
        int north, south, west, east;
        north = south = west = east = 0;
        Vector3 lastRoom = new Vector3(0f, 0f, 0f);

        for (int x = 0; x < rooms; x++)
        {
            int place = Random.Range(1, 5);
            if (x != 0)
            {
                if (place == 1) { east++; }
                else if (place == 2) { north++; }
                else if (place == 3) { west++; }
                else if (place == 4) { south++; }
            }
            for (int y = 0; y < columns; y++)
            {
                for (int z = 0; z < rows; z++)
                {
                    if (x == 0)
                    {
                        GameObject instance = Instantiate(floorTiles[0], new Vector3(y, z, 0f), Quaternion.identity) as GameObject;
                        instance.transform.SetParent(boardHolder);
                        if (y == 0 && z == 0)
                        {
                            lastRoom = instance.transform.position;
                        }
                    }
                    else {
                        if (place == 1)
                        {
                            GameObject instance = Instantiate(floorTiles[0], lastRoom + new Vector3(7*east + y, z, 0f), Quaternion.identity) as GameObject;
                            instance.transform.SetParent(boardHolder);
                            if (y==0 && z==0) {
                                lastRoom = instance.transform.position;
                            }
                        }
                        else if (place == 2)
                        {
                            GameObject instance = Instantiate(floorTiles[0], lastRoom + new Vector3(y, z+7*north, 0f), Quaternion.identity) as GameObject;
                            instance.transform.SetParent(boardHolder);
                            if (y == 0 && z == 0)
                            {
                                lastRoom = instance.transform.position;
                            }
                        }

                        else if (place == 3)
                        {
                            GameObject instance = Instantiate(floorTiles[0], lastRoom + new Vector3(y-7*west, z, 0f), Quaternion.identity) as GameObject;
                            instance.transform.SetParent(boardHolder);
                            if (y == 0 && z == 0)
                            {
                                lastRoom = instance.transform.position;
                            }
                        }
                        else if (place == 4)
                        {
                            GameObject instance = Instantiate(floorTiles[0], lastRoom + new Vector3(y, z-7*south, 0f), Quaternion.identity) as GameObject;
                            instance.transform.SetParent(boardHolder);
                            if (y == 0 && z == 0)
                            {
                                lastRoom = instance.transform.position;
                            }
                        }
                    }
                }
            }
        }
 //       for (int x = 0; x < columns; x++) {
   //         for (int y = 0; y < rows; y++) {
     //               GameObject instance = Instantiate(floorTiles[0], new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
       //             instance.transform.SetParent(boardHolder)
         //   }
       // }
    }

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
