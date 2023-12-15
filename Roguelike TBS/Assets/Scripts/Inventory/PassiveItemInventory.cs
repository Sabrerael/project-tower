using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Stats;
using UnityEngine;

public class PassiveItemInventory : MonoBehaviour, IModifierProvider {
    [SerializeField] List<PassiveItem> passiveItemInventory = new List<PassiveItem>();

    protected Dictionary<Stat, int> statModifyAdditions = new Dictionary<Stat, int>();
    protected Dictionary<Stat, int> statModifyPercentages = new Dictionary<Stat, int>();

    /// <summary>
    /// Broadcasts when the passive items in the slots are added/removed.
    /// </summary>
    public event Action passiveItemInventoryUpdated;

    private void Awake() {
        foreach(Stat stat in Enum.GetValues(typeof(Stat))) {
            statModifyPercentages[stat] = 0;
            statModifyAdditions[stat] = 0;
        }
    }

    /// <summary>
    /// How many slots are in the inventory?
    /// </summary>
    public int GetPassiveItemInventorySize() {
        return passiveItemInventory.Count;
    }

    public List<PassiveItem> GetPassiveItemInventory() { return passiveItemInventory; }
    public PassiveItem GetPassiveItemInSlot(int index) { return passiveItemInventory[index]; }

        /// <summary>
    /// Attempt to add the items to the first available slot.
    /// </summary>
    /// <param name="item">The item to add.</param>
    /// <param name="number">The number to add.</param>
    /// <returns>Whether or not the item could be added.</returns>
    public bool AddToFirstEmptyPassiveItemSlot(PassiveItem item) {
        passiveItemInventory.Add(item);

        item.TriggerPassiveEffect(gameObject);

        if (passiveItemInventoryUpdated != null) {
            passiveItemInventoryUpdated();
        }
        return true;
    }

    /// <summary>
    /// Remove a number of items from the given slot. Will never remove more
    /// that there are.
    /// </summary>
    public void RemovePassiveItemFromSlot(int slot) {
        passiveItemInventory.RemoveAt(slot);
        if (passiveItemInventoryUpdated != null) {
            passiveItemInventoryUpdated();
        }
    }

    /// <summary>
    /// Will add a passiveitem to the given slot if possible. If there is already
    /// a stack of this type, it will add to the existing stack. Otherwise,
    /// it will be added to the first empty slot.
    /// </summary>
    /// <param name="item">The item type to add.</param>
    /// <returns>True if the item was added anywhere in the inventory.</returns>
    public bool AddPassiveItemToSlot(PassiveItem item) {
        passiveItemInventory.Add(item);
        
        item.TriggerPassiveEffect(gameObject);

        if (passiveItemInventoryUpdated != null) {
            passiveItemInventoryUpdated();
        }
        return true;
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
