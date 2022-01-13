using UnityEngine;

public class PauseMenu : MonoBehaviour {
    [SerializeField] GameObject menuBody = null;

    public static PauseMenu instance = null;

    private void Awake() {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    } 

    public bool GetMenuBodyActive() { return menuBody.activeInHierarchy; }

    public void ToggleBodyActive() {
        menuBody.SetActive(!menuBody.activeInHierarchy);
    }
}
