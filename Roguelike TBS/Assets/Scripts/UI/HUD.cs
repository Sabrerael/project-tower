using UnityEngine;

public class HUD : MonoBehaviour {
    public static HUD instance = null;

    private void Awake() {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    } 
}
