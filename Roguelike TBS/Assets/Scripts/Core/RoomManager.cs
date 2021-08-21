using UnityEngine;

public class RoomManager : MonoBehaviour {
    [SerializeField] DropSpawner dropSpawner = null;
    [SerializeField] GameObject enemiesParent = null;
    [SerializeField] EnemySpawner[] enemySpawners = null;

    private bool itemSpawned = false;
    private bool roomIsActive = false;
    private bool enemiesSpawned = false;
    private bool doorsOpen = false;

    private void Update() {
        CheckDropSpawner();
        CheckDoors();
    }

    private void CheckDropSpawner() {
        if (itemSpawned || dropSpawner == null) { return; } 

        if (!roomIsActive) { return; }

        if (!enemiesSpawned) { return; }

        if (dropSpawner.GetPickup() != null && enemiesParent.transform.childCount == 0)
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
        }
    }

    public GameObject GetEnemiesParent() { return enemiesParent; }

    public void SetDropSpawner(Pickup pickup) { dropSpawner.SetPickup(pickup); }

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
