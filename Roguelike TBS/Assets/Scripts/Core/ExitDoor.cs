using System.Collections;
using UnityEngine;

public class ExitDoor : MonoBehaviour {
    [SerializeField] Sprite openDoorSprite = null;
    [SerializeField] int floor = 1;
    [SerializeField] SpriteRenderer doorToOpen = null;

    private LevelLoader levelLoader = null;

    private void Awake() {
        levelLoader = GameObject.Find("Level Loader").GetComponent<LevelLoader>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            StartCoroutine(FloorTransition(other.gameObject));
        }
    }

    private IEnumerator FloorTransition(GameObject player) {
        yield return new WaitForSeconds(1.5f);
        
        if (floor == 1) {
            levelLoader.LoadLevelTwo();
        } else if (floor == 2) {
            //levelLoader.LoadLevelThree();
            levelLoader.LoadWinScreen();
        } else if (floor == 3) {
            levelLoader.LoadWinScreen();
        } 
    }

    public void Unlock() {
        gameObject.GetComponent<Collider2D>().enabled = true;
        doorToOpen.sprite = openDoorSprite;
        doorToOpen.gameObject.GetComponent<SpriteMask>().enabled = true;
    }
}
