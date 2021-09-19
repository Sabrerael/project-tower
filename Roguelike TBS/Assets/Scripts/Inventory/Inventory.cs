using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Stats;
using UnityEngine;

public class Inventory : MonoBehaviour, IModifierProvider {
    [Serializable]
    public struct ActionItemInventorySlot {
        public ActionItem item;
        public int number;
    }

    [SerializeField] WeaponConfig[] weaponInventory = new WeaponConfig[5];
    [SerializeField] ActionItemInventorySlot[] actionItemInventory = new ActionItemInventorySlot[5];
    [SerializeField] List<PassiveItem> passiveItemInventory = new List<PassiveItem>();
    [SerializeField] AudioClip sfx = null;

    private int activeWeaponIndex = 0;
    private bool[] activeItemsInCooldown = {false, false, false, false, false};

    protected Dictionary<Stat, int> statModifyAdditions = new Dictionary<Stat, int>();
    protected Dictionary<Stat, int> statModifyPercentages = new Dictionary<Stat, int>();

    private void Awake() {
        foreach(Stat stat in Enum.GetValues(typeof(Stat))) {
            statModifyPercentages[stat] = 0;
            statModifyAdditions[stat] = 0;
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            if (GetItemInSlot(0) != null && CanUseHealthPotion(0) && !activeItemsInCooldown[0]) { //TODO this might be a issue
                actionItemInventory[0].item.Use(gameObject);
                RemoveFromSlot(0, 1);
            }
        } else if (Input.GetKeyDown(KeyCode.Alpha2) && !activeItemsInCooldown[1]) {
            if (GetItemInSlot(1) != null) {
                actionItemInventory[1].item.Use(gameObject);
                RemoveFromSlot(1, 1);
            }
        } else if (Input.GetKeyDown(KeyCode.Alpha3) && !activeItemsInCooldown[2]) {
            if (GetItemInSlot(2) != null) {
                actionItemInventory[2].item.Use(gameObject);
                RemoveFromSlot(2, 1);
            }
        } else if (Input.GetKeyDown(KeyCode.Alpha4) && !activeItemsInCooldown[3]) {
            if (GetItemInSlot(3) != null) {
                actionItemInventory[3].item.Use(gameObject);
                RemoveFromSlot(3, 1);
            }
        } else if (Input.GetKeyDown(KeyCode.Alpha5) && !activeItemsInCooldown[4]) {
            if (GetItemInSlot(4) != null) {
                actionItemInventory[4].item.Use(gameObject);
                RemoveFromSlot(4, 1);
            }
        } else if (Input.GetKeyDown(KeyCode.E)) {
            ChangeWeapon();
        } else if (Input.GetKeyDown(KeyCode.Backspace)) {
            DeleteEquipWeapon();
        }
    }

    // PUBLIC

    /// <summary>
    /// Broadcasts when the items in the slots are added/removed.
    /// </summary>
    public event Action inventoryUpdated;

    /// <summary>
    /// Convenience for getting the player's inventory.
    /// </summary>
    public static Inventory GetPlayerInventory() {
        var player = GameObject.FindWithTag("Player");
        return player.GetComponent<Inventory>();
    }

    private bool CanUseHealthPotion(int slot) {
        if (actionItemInventory[slot].item is HealthPotion) {
            if (gameObject.GetComponent<Health>().IsAtMaxHealth()) {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Broadcasts when the weapons in the slots are added/removed.
    /// </summary>
    public event Action weaponInventoryUpdated;

    /// <summary>
    /// Broadcasts when the items in the slots are added/removed.
    /// </summary>
    public event Action actionItemInventoryUpdated;

    /// <summary>
    /// Broadcasts when the passive items in the slots are added/removed.
    /// </summary>
    public event Action passiveItemInventoryUpdated;

    /// <summary>
    /// Could this Weapon fit anywhere in the inventory?
    /// </summary>
    public bool HasSpaceForWeapon(WeaponConfig weapon) {
        return FindWeaponSlot(weapon) >= 0;
    }

    /// <summary>
    /// Could this Action Item fit anywhere in the inventory?
    /// </summary>
    public bool HasSpaceForActionItem(ActionItem item) {
        return FindActionItemSlot(item) >= 0;
    }

     /// <summary>
    /// Could this Passive Item fit anywhere in the inventory?
    /// </summary>
    public bool HasSpaceForPassiveItem(PassiveItem item) {
        return FindActionItemSlot(item) >= 0;
    }

    /// <summary>
    /// How many slots are in the inventory?
    /// </summary>
    public int GetWeaponInventorySize() {
        return weaponInventory.Length;
    }

    /// <summary>
    /// How many slots are in the inventory?
    /// </summary>
    public int GetActionItemInventorySize() {
        return actionItemInventory.Length;
    }

    /// <summary>
    /// How many slots are in the inventory?
    /// </summary>
    public int GetPassiveItemInventorySize() {
        return passiveItemInventory.Count;
    }

    // All these might not be necessary
    public WeaponConfig[] GetWeaponInventory() { return weaponInventory; }
    public WeaponConfig GetWeaponInventorySlot(int index) { return weaponInventory[index]; }
    public ActionItemInventorySlot[] GetActionItemInventory() { return actionItemInventory; }
    public ActionItemInventorySlot GetActionItemInventorySlot(int index) { return actionItemInventory[index]; }
    public List<PassiveItem> GetPassiveItemInventory() { return passiveItemInventory; }
    public PassiveItem GetPassiveItemInSlot(int index) { return passiveItemInventory[index]; }

    /// <summary>
    /// Attempt to add the weapon to the first available slot.
    /// </summary>
    /// <param name="weapon">The weapon to add.</param>
    /// <returns>Whether or not the item could be added.</returns>
    public bool AddToFirstEmptyWeaponSlot(WeaponConfig weapon) {
        int i = FindWeaponSlot(weapon);

        if (i < 0) {
            return false;
        }

        weaponInventory[i] = weapon;
        if (weaponInventoryUpdated != null) {
            weaponInventoryUpdated();
        }
        return true;
    }

    /// <summary>
    /// Is there an instance of the Weapon in the inventory?
    /// </summary>
    public bool HasWeapon(WeaponConfig weapon) {
        for (int i = 0; i < weaponInventory.Length; i++) {
            if (object.ReferenceEquals(weaponInventory[i], weapon)) {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Return the weapon in the given slot.
    /// </summary>
    public WeaponConfig GetWeaponInSlot(int slot) {
        return weaponInventory[slot];
    }

    /// <summary>
    /// Remove a number of items from the given slot. Will never remove more
    /// that there are.
    /// </summary>
    public void RemoveFromWeaponSlot(int slot) {
        weaponInventory[slot] = null;
        if (weaponInventoryUpdated != null) {
            weaponInventoryUpdated();
        }
    }

    /// <summary>
    /// Will add a weapon to the given slot if possible. If there is already
    /// a stack of this type, it will add to the existing stack. Otherwise,
    /// it will be added to the first empty slot.
    /// </summary>
    /// <param name="slot">The slot to attempt to add to.</param>
    /// <param name="weapon">The item type to add.</param>
    /// <param name="number">The number of items to add.</param>
    /// <returns>True if the item was added anywhere in the inventory.</returns>
    public bool AddWeaponToSlot(int slot, WeaponConfig weapon, int number) {
        if (weaponInventory[slot] != null) {
            return AddToFirstEmptyWeaponSlot(weapon);
        }

        if (weaponInventoryUpdated != null) {
                weaponInventoryUpdated();
        }
        return true;
    }

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
        if (!actionItemInventory[slot].item.isConsumable()) { return; }
        
        actionItemInventory[slot].number -= number;

        if (actionItemInventory[slot].number <= 0) {
            actionItemInventory[slot].number = 0;
            actionItemInventory[slot].item = null;
        }

        if (actionItemInventoryUpdated != null) {
            actionItemInventoryUpdated();
        }
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
    /// Attempt to add the items to the first available slot.
    /// </summary>
    /// <param name="item">The item to add.</param>
    /// <param name="number">The number to add.</param>
    /// <returns>Whether or not the item could be added.</returns>
    public bool AddToFirstEmptyPassiveItemSlot(PassiveItem item) {
        passiveItemInventory.Add(item);

        if (item is RegenItem) {
            StartCoroutine((item as RegenItem).StartRegen());
        } else if (item is PurseModifier) {
            (item as PurseModifier).AddPurseMultiplier(GetComponent<Purse>());
        } else if (item as StatModifierItem) {
            (item as StatModifierItem).ApplyStatChanges(this);
        } else if (item as TradeOffItem) {
            (item as TradeOffItem).ApplyStatChanges(this);
        } else if (item as ShieldItem) {
            GetComponent<Fighter>().onInitialHit += (item as ShieldItem).WillShieldPlayer;
        } else if (item as DodgeChanceItem) {
            GetComponent<Fighter>().onInitialHit += (item as DodgeChanceItem).WillDodge;
        } else if (item as AttackFeedbackItem) {
            GetComponent<Fighter>().onActualHit += (item as AttackFeedbackItem).FeedbackEffect;
        }

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
        
        if (item is RegenItem) {
            StartCoroutine((item as RegenItem).StartRegen());
        } else if (item is PurseModifier) {
            (item as PurseModifier).AddPurseMultiplier(GetComponent<Purse>());
        } else if (item as StatModifierItem) {
            (item as StatModifierItem).ApplyStatChanges(this);
        } else if (item as TradeOffItem) {
            (item as TradeOffItem).ApplyStatChanges(this);
        } else if (item as ShieldItem) {
            GetComponent<Fighter>().onInitialHit += (item as ShieldItem).WillShieldPlayer;
        } else if (item as DodgeChanceItem) {
            GetComponent<Fighter>().onInitialHit += (item as DodgeChanceItem).WillDodge;
        } else if (item as AttackFeedbackItem) {
            GetComponent<Fighter>().onActualHit += (item as AttackFeedbackItem).FeedbackEffect;
        }

        if (passiveItemInventoryUpdated != null) {
            passiveItemInventoryUpdated();
        }
        return true;
    }

    /// <summary>
    /// Find a Weapon slot that can accomodate the given weapon.
    /// </summary>
    /// <returns>-1 if no slot is found.</returns>
    private int FindWeaponSlot(WeaponConfig weapon) {
        return FindEmptyWeaponSlot();
    }

    /// <summary>
    /// Find an empty Weapon slot.
    /// </summary>
    /// <returns>-1 if all slots are full.</returns>
    private int FindEmptyWeaponSlot() {
        for (int i = 0; i < weaponInventory.Length; i++) {
            if (weaponInventory[i] == null) {
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
    /// Find an existing stack of this item type.
    /// </summary>
    /// <returns>-1 if no stack exists or if the item is not stackable.</returns>
    private int FindActionItemStack(InventoryItem item) {
        if (!item.IsStackable()) {
            return -1;
        }

        for (int i = 0; i < actionItemInventory.Length; i++) {
            if (object.ReferenceEquals(actionItemInventory[i].item, item)) {
                return i;
            }
        }
        return -1;
    }

    private IEnumerator CooldownTimer(float time, int index) {
        activeItemsInCooldown[index] = true;
        yield return new WaitForSeconds(time);
        activeItemsInCooldown[index] = false;
    }

    private void ChangeWeapon() {
        int attemptedWeaponIndex = activeWeaponIndex;
        for (int i = 0; i < weaponInventory.Length - 1; i++) {
            attemptedWeaponIndex += 1;
            if (attemptedWeaponIndex >= 5) {
                attemptedWeaponIndex -= 5;
            }
            
            if (GetWeaponInSlot(attemptedWeaponIndex) != null) {
                activeWeaponIndex = attemptedWeaponIndex;
                gameObject.GetComponent<Fighter>().EquipWeapon(GetWeaponInSlot(activeWeaponIndex));
                AudioSource.PlayClipAtPoint(sfx, transform.position);
            }
        }
    }

    private void DeleteEquipWeapon() {
        //Check for another weapon (Player should not have zero weapons)
        int attemptedWeaponIndex = activeWeaponIndex;
        for (int i = 0; i < weaponInventory.Length - 1; i++) {
            attemptedWeaponIndex += 1;
            if (attemptedWeaponIndex >= 5) {
                attemptedWeaponIndex -= 5;
            }

            // If another weapon exists, switch to the new weapon and delete the previously equip weapon
            if (GetWeaponInSlot(attemptedWeaponIndex)) {
                int weaponIndexToDelete = activeWeaponIndex;
                ChangeWeapon();
                RemoveFromWeaponSlot(weaponIndexToDelete);
                return;
            }
        }
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
