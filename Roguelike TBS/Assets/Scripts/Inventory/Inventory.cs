using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Stats;
using UnityEngine;
// TODO THIS IS SUPER GROSS. I NEED TO PULL THIS SHIT OUT INTO SEPERATE FILES.
public class Inventory : MonoBehaviour, IModifierProvider {
    [SerializeField] WeaponConfig[] weaponInventory = new WeaponConfig[3];
    [SerializeField] AudioClip sfx = null;

    private int activeWeaponIndex = 0;

    protected Dictionary<Stat, int> statModifyAdditions = new Dictionary<Stat, int>();
    protected Dictionary<Stat, int> statModifyPercentages = new Dictionary<Stat, int>();

    private void Awake() {
        foreach(Stat stat in Enum.GetValues(typeof(Stat))) {
            statModifyPercentages[stat] = 0;
            statModifyAdditions[stat] = 0;
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

    /// <summary>
    /// Broadcasts when the weapons in the slots are added/removed.
    /// </summary>
    public event Action weaponInventoryUpdated;

    /// <summary>
    /// Could this Weapon fit anywhere in the inventory?
    /// </summary>
    public bool HasSpaceForWeapon(WeaponConfig weapon) {
        return FindWeaponSlot(weapon) >= 0;
    }

     /// <summary>
    /// Could this Passive Item fit anywhere in the inventory?
    /// </summary>
    public bool HasSpaceForPassiveItem(PassiveItem item) {
        return true;
    }

    /// <summary>
    /// How many slots are in the inventory?
    /// </summary>
    public int GetWeaponInventorySize() {
        return weaponInventory.Length;
    }

    // All these might not be necessary
    public WeaponConfig[] GetWeaponInventory() { return weaponInventory; }
    public WeaponConfig GetWeaponInventorySlot(int index) { return weaponInventory[index]; }

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

    public void ChangeWeapon() {
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

    public void DeleteEquipWeapon() {
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
