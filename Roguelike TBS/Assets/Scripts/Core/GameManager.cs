using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    private List<InventoryItem> knockoutList = new List<InventoryItem>();
    private CharacterClass playerCharacter = CharacterClass.Paladin;

    public static GameManager instance = null;

    private void Awake() {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public CharacterClass GetPlayerCharacter() { return playerCharacter; }
    public void SetPlayerCharacter(CharacterClass character) { playerCharacter = character; }
    public void SetPlayerCharacter(int value) { playerCharacter = (CharacterClass)value; }

    public void AddItemToKnockoutList(InventoryItem item) {
        knockoutList.Add(item);
    }

    public bool ItemIsInKnockoutList(InventoryItem item) {
        if (knockoutList.Contains(item)) {
            return true;
        } else {
            return false;
        }
    }

    public void ResetKnockoutList() {
        knockoutList = new List<InventoryItem>();
    }
}
