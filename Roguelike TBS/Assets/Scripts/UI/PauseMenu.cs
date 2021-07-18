using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {

    [SerializeField] GameObject menuBody = null;

    public bool GetMenuBodyActive() { return menuBody.activeInHierarchy; }

    public void ToggleBodyActive() {
        menuBody.SetActive(!menuBody.activeInHierarchy);
    }
}
