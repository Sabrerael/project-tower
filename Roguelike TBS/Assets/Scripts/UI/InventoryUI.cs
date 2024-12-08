﻿using UnityEngine;

/// <summary>
/// To be placed on the root of the inventory UI. Handles spawning all the
/// inventory slot prefabs.
/// </summary>
public class InventoryUI : MonoBehaviour {

    public static InventoryUI instance = null;

    // CONFIG DATA
    [SerializeField] InventorySlotUI InventoryItemPrefab = null;

    // CACHE
    [SerializeField] Inventory playerInventory;
    [SerializeField] ActionItemInventory actionItemInventory;

    // LIFECYCLE METHODS

    private void Start() {
        //if (instance == null)
            //instance = this;
        //else if (instance != this)
            //Destroy(gameObject);
        
        if (!playerInventory) {
            playerInventory = GameObject.FindWithTag("Player").GetComponent<Inventory>();
        }

        if (!actionItemInventory) {
            actionItemInventory = GameObject.FindWithTag("Player").GetComponent<ActionItemInventory>();
        }

        actionItemInventory.actionItemInventoryUpdated += Redraw;
        Redraw();
    }

    // PRIVATE

    private void Redraw() {
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }

        // Magic Number situation. Fix to get player inventory
        //for (int i = 0; i < playerInventory.GetSize(); i++) {
        for (int i = 0; i < 2; i++) {
            var itemUI = Instantiate(InventoryItemPrefab, transform);
            itemUI.Setup(actionItemInventory, i);
        }
    }    
}

