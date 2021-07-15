using UnityEngine;

public class MusicPlayer : MonoBehaviour {
    public static MusicPlayer instance = null;

    private void Awake() {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
}