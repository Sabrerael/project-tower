﻿using UnityEngine;

/// <summary>
/// An inventory item that can be placed in the action bar and "Used".
/// </summary>
/// <remarks>
/// This class should be used as a base. Subclasses must implement the `Use`
/// method.
/// </remarks>
[CreateAssetMenu(menuName = ("Inventory/Action Item"))]
public class ActionItem : InventoryItem {
    // CONFIG DATA
    [Tooltip("Does an instance of this item get consumed every time it's used.")]
    [SerializeField] bool consumable = false;
    [Tooltip("Cooldown Timer")]
    [SerializeField] float cooldownTimer = 1f;

    // PUBLIC

    /// <summary>
    /// Trigger the use of this item. Override to provide functionality.
    /// </summary>
    /// <param name="user">The character that is using this action.</param>
    public virtual void Use(GameObject user) {
    }

    public bool isConsumable() {
        return consumable;
    }

    public float GetCooldownTimer() {
        return cooldownTimer;
    }
}
