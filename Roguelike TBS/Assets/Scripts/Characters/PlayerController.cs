using UnityEngine;

public class PlayerController : MonoBehaviour {
    private void Update() {
        if (Input.GetKeyDown(KeyCode.X)) {
            GetComponent<Fighter>().DodgeRoll();
        }
    }
}
