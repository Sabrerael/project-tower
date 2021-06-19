using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInventoryUI : MonoBehaviour {
       // CONFIG DATA
    [SerializeField] WeaponInventorySlotUI InventoryItemPrefab = null;

    // CACHE
    Inventory playerInventory;

    // LIFECYCLE METHODS

    private void Awake() {
        playerInventory = Inventory.GetPlayerInventory();
        playerInventory.weaponInventoryUpdated += Redraw;
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
