using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFlipper : MonoBehaviour {
    private Camera mainCam;

    private void Start() {
        mainCam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    private void Update() {
        if (mainCam == null) {
            mainCam = GameObject.Find("Main Camera").GetComponent<Camera>();
        }

        if (transform.position.x >= mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue()).x) {
            transform.localScale = new Vector2(-1f, 1f);
        } else {
            transform.localScale = new Vector2(1f, 1f);
        }
    }
}
