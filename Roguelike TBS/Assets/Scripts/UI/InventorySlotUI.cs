using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlotUI : MonoBehaviour, IItemHolder {
    // CONFIG DATA
    [SerializeField] InventoryItemIcon icon = null;
    [SerializeField] int index = 0;

    // STATE
    InventoryItem item;
    ActionItemInventory inventory;

    // PUBLIC

    public void Setup(ActionItemInventory inventory, int index) {
        this.inventory = inventory;
        this.index = index;
        icon.SetItem(inventory.GetItemInSlot(index), inventory.GetNumberInActionItemSlot(index));
    }

    public int MaxAcceptable(InventoryItem item) {
        // Currently will be true. Only 5 items for 5 inventory slots
        //if (inventory.HasSpaceFor(item)) {
            return int.MaxValue;
        //}
        //return 0;
    }

    public void AddItems(InventoryItem item, int number) {
        inventory.AddItemToSlot(index, item as ActionItem, number);
    }
        
    public InventoryItem GetItem() {
        return inventory.GetItemInSlot(index);
    }

    public int GetNumber() {
        return inventory.GetNumberInActionItemSlot(index);
    }

    public void RemoveItems(int number) {
        inventory.RemoveFromSlot(index, number);
    }

    private void UpdateIcon() {
        icon.SetItem(GetItem(), GetNumber());
    }
}
