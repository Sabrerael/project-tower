using RPG.Stats;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionItemInventory : MonoBehaviour, IModifierProvider {
    public static ActionItemInventory instance = null;

    [Serializable]
    public struct ActionItemInventorySlot {
        public ActionItem item;
        public int number;
    }

    [SerializeField] ActionItemInventorySlot[] actionItemInventory = new ActionItemInventorySlot[2];

    private int activeActionItemIndex = 0;
    private bool[] activeItemsInCooldown = {false, false};
    protected Dictionary<Stat, int> statModifyAdditions = new Dictionary<Stat, int>();
    protected Dictionary<Stat, int> statModifyPercentages = new Dictionary<Stat, int>();

    /// <summary>
    /// Broadcasts when the items in the slots are added/removed.
    /// </summary>
    public event Action actionItemInventoryUpdated;

    private void Awake() {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        foreach(Stat stat in Enum.GetValues(typeof(Stat))) {
            statModifyPercentages[stat] = 0;
            statModifyAdditions[stat] = 0;
        }
    }

    /// <summary>
    /// How many slots are in the inventory?
    /// </summary>
    public int GetActionItemInventorySize() {
        return actionItemInventory.Length;
    }

    public ActionItemInventorySlot[] GetActionItemInventory() { return actionItemInventory; }
    public ActionItemInventorySlot GetActionItemInventorySlot(int index) { return actionItemInventory[index]; }

    /// <summary>
    /// Attempt to add the items to the first available slot.
    /// </summary>
    /// <param name="item">The item to add.</param>
    /// <param name="number">The number to add.</param>
    /// <returns>Whether or not the item could be added.</returns>
    public bool AddToFirstEmptyActionItemSlot(ActionItem item, int number) {
        int i = FindActionItemSlot(item);

        if (i < 0) {
            return false;
        }

        actionItemInventory[i].item = item;
        actionItemInventory[i].number += number;

        if (actionItemInventoryUpdated != null) {
            actionItemInventoryUpdated();
        }
        return true;
    }

    /// <summary>
    /// Is there an instance of the item in the inventory?
    /// </summary>
    public bool HasItem(InventoryItem item) {
        for (int i = 0; i < actionItemInventory.Length; i++) {
            if (object.ReferenceEquals(actionItemInventory[i].item, item)) {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Return the item type in the given slot.
    /// </summary>
    public ActionItem GetItemInSlot(int slot) {
        return actionItemInventory[slot].item;
    }

    /// <summary>
    /// Get the number of items in the given slot.
    /// </summary>
    public int GetNumberInActionItemSlot(int slot) {
        return actionItemInventory[slot].number;
    }

    /// <summary>
    /// Remove a number of items from the given slot. Will never remove more
    /// that there are.
    /// </summary>
    public void RemoveFromSlot(int slot, int number) {
        return;
        
        /*if (!actionItemInventory[slot].item.isConsumable()) { return; }
        
        actionItemInventory[slot].number -= number;

        if (actionItemInventory[slot].number <= 0) {
            actionItemInventory[slot].number = 0;
            actionItemInventory[slot].item = null;
        }

        if (actionItemInventoryUpdated != null) {
            actionItemInventoryUpdated();
        }*/
    }

    /// <summary>
    /// Will add an item to the given slot if possible. If there is already
    /// a stack of this type, it will add to the existing stack. Otherwise,
    /// it will be added to the first empty slot.
    /// </summary>
    /// <param name="slot">The slot to attempt to add to.</param>
    /// <param name="item">The item type to add.</param>
    /// <param name="number">The number of items to add.</param>
    /// <returns>True if the item was added anywhere in the inventory.</returns>
    public bool AddItemToSlot(int slot, ActionItem item, int number) {
        if (actionItemInventory[slot].item != null) {
            return AddToFirstEmptyActionItemSlot(item, number); ;
        }

        var i = FindActionItemStack(item);
        if (i >= 0) {
            slot = i;
        }

        actionItemInventory[slot].item = item;
        actionItemInventory[slot].number += number;
        if (actionItemInventoryUpdated != null) {
            actionItemInventoryUpdated();
        }
        return true;
    }

    /// <summary>
    /// Find an empty slot.
    /// </summary>
    /// <returns>-1 if all slots are full.</returns>
    private int FindEmptyActionItemSlot() {
        for (int i = 0; i < actionItemInventory.Length; i++) {
            if (actionItemInventory[i].item == null) {
                return i;
            }
        }
        return -1;
    }

    /// <summary>
    /// Find a slot that can accomodate the given item.
    /// </summary>
    /// <returns>-1 if no slot is found.</returns>
    private int FindActionItemSlot(InventoryItem item) {
        int i = FindActionItemStack(item);
        if (i < 0) {
            i = FindEmptyActionItemSlot();
        }
        return i;
    }

    /// <summary>
    /// Find an existing stack of this item type.
    /// </summary>
    /// <returns>-1 if no stack exists or if the item is not stackable.</returns>
    private int FindActionItemStack(InventoryItem item) {
        /*if (!item.IsStackable()) {
            return -1;
        }

        for (int i = 0; i < actionItemInventory.Length; i++) {
            if (object.ReferenceEquals(actionItemInventory[i].item, item)) {
                return i;
            }
        }*/
        return -1;
    }

    /// <summary>
    /// Could this Action Item fit anywhere in the inventory?
    /// </summary>
    public bool HasSpaceForActionItem(ActionItem item) {
        return FindActionItemSlot(item) >= 0;
    }

    private IEnumerator CooldownTimer(float time, int index) {
        Debug.Log("Cooldown started");
        activeItemsInCooldown[index] = true;
        yield return new WaitForSeconds(time);
        Debug.Log("Cooldown ended");
        activeItemsInCooldown[index] = false;
    }

    public void UseItemInSlot(int index) {
        if (GetItemInSlot(index) != null && !activeItemsInCooldown[index]) {
            actionItemInventory[index].item.Use(gameObject);
            StartCoroutine(CooldownTimer(actionItemInventory[index].item.GetCooldownTimer(), index));
        }
    }

    public void UseActiveItem() {
        if (GetItemInSlot(activeActionItemIndex) != null && !activeItemsInCooldown[activeActionItemIndex]) {
            actionItemInventory[activeActionItemIndex].item.Use(gameObject);
            StartCoroutine(CooldownTimer(actionItemInventory[activeActionItemIndex].item.GetCooldownTimer(), activeActionItemIndex));
        }
    }

    public void SwitchActiveItem() {
        int indexToSwitchTo;
        if (activeActionItemIndex == 0) {
            indexToSwitchTo = 1;
        } else {
            indexToSwitchTo = 0;
        }

        if (GetItemInSlot(activeActionItemIndex) != null) {
            activeActionItemIndex = indexToSwitchTo;
            // TODO UI Stuff
        }
    }

    public Sprite GetActiveActionItemSprite() {
        return actionItemInventory[activeActionItemIndex].item.GetIcon();
    }

    public void ModifyPassiveBonusAddition(Stat stat, int increase) {
        statModifyAdditions[stat] += increase;
    }

    public void ModifyPassiveBonusPercentage(Stat stat, int percent) {
        statModifyPercentages[stat] += percent;
    }

    IEnumerable<int> IModifierProvider.GetAdditiveModifiers(Stat stat) {
        yield return statModifyAdditions[stat];
    }

    IEnumerable<int> IModifierProvider.GetMultiplicativeModifiers(Stat stat) {
        yield return statModifyAdditions[stat];
    }
}
