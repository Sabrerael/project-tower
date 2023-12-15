using UnityEngine;

public class PassiveInventorySlotUI : MonoBehaviour, IItemHolder {
    // CONFIG DATA
    [SerializeField] PassiveInventoryItemIcon icon = null;
    [SerializeField] int index = 0;

    // STATE
    InventoryItem item;
    PassiveItemInventory inventory;

    // PUBLIC

    public void Setup(PassiveItemInventory inventory, int index) {
        this.inventory = inventory;
        this.index = index;
        icon.SetItem(inventory.GetPassiveItemInSlot(index), 1);
    }

    public int MaxAcceptable(InventoryItem item) {
        // Currently will be true. Only 5 items for 5 inventory slots
        //if (inventory.HasSpaceFor(item)) {
            return int.MaxValue;
        //}
        //return 0;
    }

    public void AddItems(InventoryItem item, int number) {
        inventory.AddPassiveItemToSlot(item as PassiveItem);
    }
        
    public InventoryItem GetItem() {
        return inventory.GetPassiveItemInSlot(index);
    }

    public int GetNumber() {
        return 1;
    }

    public void RemoveItems(int number) {
        //inventory.RemoveFromSlot(index, number);
    }

    private void UpdateIcon() {
        icon.SetItem(GetItem(), GetNumber());
    }
}
