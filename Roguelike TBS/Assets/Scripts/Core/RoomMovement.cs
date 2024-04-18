using Cinemachine;
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
    [SerializeField] GameObject doorGameObject = null;

    private ICinemachineCamera virtualCamera;

    private void Start() {
        virtualCamera = Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera;
    } 

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            MoveToRoom(other.gameObject);
        }
    }

    // TODO Update to do a smooth panover and then activate enemies.
    private void MoveToRoom(GameObject player) {
        if (virtualCamera == null) {
            virtualCamera = Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera;
        }

        Vector3 newPosition = new Vector3();
        
        if (directionToMove == Direction.Up) {
            virtualCamera.VirtualCameraGameObject.transform.position = virtualCamera.VirtualCameraGameObject.transform.position + new Vector3(0,16,0);
            newPosition = player.transform.position + new Vector3(0,3.5f,0);
        } else if (directionToMove == Direction.Down) {
            virtualCamera.VirtualCameraGameObject.transform.position = virtualCamera.VirtualCameraGameObject.transform.position + new Vector3(0,-16,0);
            newPosition = player.transform.position + new Vector3(0,-3.5f,0);
        } else if (directionToMove == Direction.Right) {
            virtualCamera.VirtualCameraGameObject.transform.position = virtualCamera.VirtualCameraGameObject.transform.position + new Vector3(26,0,0);
            newPosition = player.transform.position + new Vector3(7f,0,0);
        } else if (directionToMove == Direction.Left) {
            virtualCamera.VirtualCameraGameObject.transform.position = virtualCamera.VirtualCameraGameObject.transform.position + new Vector3(-26,0,0);
            newPosition = player.transform.position + new Vector3(-7f,0,0);
        }

        //player.GetComponent<Movement>().UpdateMinMaxValues();
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
        doorGameObject.GetComponent<SpriteRenderer>().sprite = openDoor;
        doorGameObject.GetComponent<SpriteMask>().enabled = true;
    }
}
