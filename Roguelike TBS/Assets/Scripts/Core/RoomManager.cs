using UnityEngine;

public class RoomManager : MonoBehaviour {
    [System.Serializable]
    public struct EnemySpawns {
        public Enemy[] enemies;
    }

    [SerializeField] DropSpawner dropSpawner = null;
    [SerializeField] GameObject enemiesParent = null;
    [SerializeField] EnemySpawns[] enemySpawns = null;
    [SerializeField] EnemySpawner[] enemySpawners = null;

    private bool itemSpawned = false;
    private bool roomIsActive = false;
    private bool enemiesSpawned = false;
    private bool doorsOpen = false;

    private void Start() {
        SetEnemySpawners();
    }

    private void Update() {
        CheckDropSpawner();
        CheckDoors();
    }

    private void CheckDropSpawner() {
        if (itemSpawned || dropSpawner == null) { return; } 

        if (!roomIsActive) { return; }

        if (!enemiesSpawned) { return; }

        if (dropSpawner.GetItemToDrop() != null && enemiesParent.transform.childCount == 0)
        {
            dropSpawner.SpawnPickup();
            itemSpawned = true;
        }
    }

    private void TriggerEnemySpawners() {
        if (enemySpawners == null) { 
            enemiesSpawned = true;
            return;
        }

        for (int i = 0; i < enemySpawners.Length; i++)
        {
            enemySpawners[i].SpawnEnemy(enemiesParent);
        }

        enemiesSpawned = true;
    }

    private void CheckDoors() {
        if (!roomIsActive) { return; }

        if (!enemiesSpawned) { return; }

        if (doorsOpen) { return; }

        if (enemiesParent.transform.childCount == 0) {
            OpenAllDoors();
            doorsOpen = true;
            if (gameObject.name.Equals("End Room(Clone)") ||
                gameObject.name.Equals("Floor 2 End Room (Clone)") ||
                gameObject.name.Equals("Floor Three End Room(Clone)")) {
                OpenExitDoor();
            }
            var player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<Character>().TriggerOnRoomClear();
        }
    }

    private void SetEnemySpawners() {
        if (enemySpawns == null || enemySpawns.Length == 0) {
            enemiesSpawned = true;
            roomIsActive = true; 
            CheckDoors();
            return;
        }

        for (int i = 0; i < enemySpawners.Length; i++) {
            int maximumNum = enemySpawns[i].enemies.Length;
            int index = UnityEngine.Random.Range(0, enemySpawns[i].enemies.Length);
            enemySpawners[i].SetEnemy(enemySpawns[i].enemies[index]);
        }
    }

    public GameObject GetEnemiesParent() { return enemiesParent; }

    public void ConfigureDropSpawner(InventoryItem item, int number) { 
        dropSpawner.SetItemToDrop(item);
        dropSpawner.SetNumberOfItem(number);    
    }

    public void SetRoomActive() {
        if (roomIsActive) { return; }
        
        TriggerEnemySpawners();
        roomIsActive = true;
    }

    protected void OpenAllDoors() {
        var rooms = gameObject.GetComponentsInChildren<RoomMovement>();

        foreach(var room in rooms) {
            room.OpenDoor();
        }
    }

    private void OpenExitDoor() {
        gameObject.GetComponentsInChildren<ExitDoor>()[0].Unlock();
    }
}
