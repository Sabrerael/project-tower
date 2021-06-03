using UnityEngine;

public class ExitDoor : MonoBehaviour {
    [SerializeField] Sprite openDoor = null;

    private LevelLoader levelLoader = null;

    private void Awake() {
        levelLoader = GameObject.Find("Level Loader").GetComponent<LevelLoader>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            levelLoader.LoadWinScreen();
        }
    }

    public void Unlock() {
        gameObject.GetComponent<Collider2D>().enabled = true;
        gameObject.GetComponentInChildren<SpriteRenderer>().sprite = openDoor;
    }
}
