using UnityEngine;

public class ExitDoor : MonoBehaviour {
    [SerializeField] Sprite openDoor = null;
    [SerializeField] int floor = 1;

    private LevelLoader levelLoader = null;

    private void Awake() {
        levelLoader = GameObject.Find("Level Loader").GetComponent<LevelLoader>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            if (floor == 1) {
                levelLoader.LoadLevelTwo();
            } else if (floor == 2) {
                levelLoader.LoadLevelThree();
            } else if (floor == 3) {
                levelLoader.LoadWinScreen();
            }
        }
    }

    public void Unlock() {
        gameObject.GetComponent<Collider2D>().enabled = true;
        gameObject.GetComponentInChildren<SpriteRenderer>().sprite = openDoor;
    }
}
