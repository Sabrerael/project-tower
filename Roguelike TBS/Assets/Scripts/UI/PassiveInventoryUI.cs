﻿using UnityEngine;

public class PassiveInventoryUI : MonoBehaviour {
       // CONFIG DATA
    [SerializeField] PassiveInventorySlotUI InventoryItemPrefab = null;

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

        for (int i = 0; i < playerInventory.GetPassiveItemInventorySize(); i++) {
            var itemUI = Instantiate(InventoryItemPrefab, transform);
            itemUI.Setup(playerInventory, i);
        }
    }   
}