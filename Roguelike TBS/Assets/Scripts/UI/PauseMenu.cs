using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {
    public static PauseMenu instance = null;

    [SerializeField] GameObject menuBody = null;

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
