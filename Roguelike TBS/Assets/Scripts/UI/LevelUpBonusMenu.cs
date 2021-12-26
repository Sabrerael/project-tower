using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LevelUpBonusMenu : MonoBehaviour {
    public static LevelUpBonusMenu instance = null;

    [SerializeField] GameObject menuBody = null;
    [SerializeField] List<Button> buttons = null;
    [SerializeField] TextMeshProUGUI[] bonusesMenuTextFields = null;

    private Character player;
    private List<int> choiceIndexes = new List<int>();

    private void Awake() {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
    }

    public void SetAbilityDescription(int index, string description) {
        bonusesMenuTextFields[index].text = description;
    }

    public void SetButtonOnClicks() {
        for (int i = 0; i < buttons.Count; i++) {
            buttons[i].onClick.AddListener( delegate {CallChooseBonusButton(i);});
        }
    }

    private void CallChooseBonusButton(int index) {
        player.ChooseBonusButton(index);
    }

    public void ToggleBodyActive() {
        menuBody.SetActive(!menuBody.activeInHierarchy);
    }
}
