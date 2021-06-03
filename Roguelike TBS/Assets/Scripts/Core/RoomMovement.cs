using UnityEngine;

public enum Direction {
    Up,
    Down,
    Right,
    Left
}

public class RoomMovement : MonoBehaviour {
    [SerializeField] RoomManager newRoomManager = null;  
    [SerializeField] Direction directionToMove;
    [SerializeField] Sprite openDoor = null;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            MoveToRoom(other.gameObject);
        }
    }

    private void MoveToRoom(GameObject player) {
        Vector3 newPosition = new Vector3();
        
        if (directionToMove == Direction.Up) {
            Camera.main.transform.position = Camera.main.transform.position + new Vector3(0,14,0);
            newPosition = player.transform.position + new Vector3(0,5.5f,0);
        } else if (directionToMove == Direction.Down) {
            Camera.main.transform.position = Camera.main.transform.position + new Vector3(0,-14,0);
            newPosition = player.transform.position + new Vector3(0,-5.5f,0);
        } else if (directionToMove == Direction.Right) {
            Camera.main.transform.position = Camera.main.transform.position + new Vector3(18,0,0);
            newPosition = player.transform.position + new Vector3(5f,0,0);
        } else if (directionToMove == Direction.Left) {
            Camera.main.transform.position = Camera.main.transform.position + new Vector3(-18,0,0);
            newPosition = player.transform.position + new Vector3(-5f,0,0);
        }

        player.GetComponent<Movement>().UpdateMinMaxValues();
        player.transform.position = newPosition;

        
        ActivateNewRoom(player);
    }

    private void ActivateNewRoom(GameObject player) {
        if (newRoomManager == null) {
            Debug.LogError("Door is not configured with a Room to move to. Check newRoomManager value");
        }

        newRoomManager.SetRoomActive();
        player.GetComponent<Character>().SetCurrentRoom(newRoomManager);
    }

    public Direction GetDirection() { return directionToMove; }
    public RoomManager GetNewRoomManager() { return newRoomManager;}
    public void SetNewRoomManager(RoomManager roomManager) { newRoomManager = roomManager; }

    public void OpenDoor() {
        gameObject.GetComponent<Collider2D>().enabled = true;
        gameObject.GetComponentInChildren<SpriteRenderer>().sprite = openDoor;
    }
}
