using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInventorySlotUI : MonoBehaviour {
    // CONFIG DATA
    [SerializeField] WeaponInventoryItemIcon icon = null;
    [SerializeField] int index = 0;

    // STATE
    InventoryItem item;
    Inventory inventory;

    // PUBLIC

    public void Setup(Inventory inventory, int index) {
        this.inventory = inventory;
        this.index = index;
        icon.SetItem(inventory.GetWeaponInSlot(index), 1);
    }

    public int MaxAcceptable(InventoryItem item) {
        // Currently will be true. Only 5 items for 5 inventory slots
        //if (inventory.HasSpaceFor(item)) {
            return int.MaxValue;
        //}
        //return 0;
    }

    public void AddItems(InventoryItem item, int number) {
        inventory.AddWeaponToSlot(index, item as WeaponConfig, number);
    }
        
    public InventoryItem GetItem() {
        return inventory.GetWeaponInSlot(index);
    }

    public int GetNumber() {
        return 1;
    }

    public void RemoveItems(int number) {
        inventory.RemoveFromSlot(index, number);
    }

    private void UpdateIcon() {
        icon.SetItem(GetItem(), GetNumber());
    }
}
