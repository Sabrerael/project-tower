using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// To be placed on the root of the inventory UI. Handles spawning all the
/// inventory slot prefabs.
/// </summary>
public class InventoryUI : MonoBehaviour {

    public static InventoryUI instance = null;

    // CONFIG DATA
    [SerializeField] InventorySlotUI InventoryItemPrefab = null;

    // CACHE
    Inventory playerInventory;

    // LIFECYCLE METHODS

    private void Awake() {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        
        playerInventory = Inventory.GetPlayerInventory();
        playerInventory.actionItemInventoryUpdated += Redraw;
    }

    private void Start() {
        Redraw();
    }

    // PRIVATE

    private void Redraw() {
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }

        // Magic Number situation. Fix to get player inventory
        //for (int i = 0; i < playerInventory.GetSize(); i++) {
        for (int i = 0; i < 5; i++) {
            var itemUI = Instantiate(InventoryItemPrefab, transform);
            itemUI.Setup(playerInventory, i);
        }
    }    
}

