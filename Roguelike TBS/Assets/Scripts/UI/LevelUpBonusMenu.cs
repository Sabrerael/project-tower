using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpBonusMenu : MonoBehaviour {
    public static LevelUpBonusMenu instance = null;

    [SerializeField] GameObject menuBody = null;

    private void Awake() {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    } 

    public void ToggleBodyActive() {
        menuBody.SetActive(!menuBody.activeInHierarchy);
    }
}
