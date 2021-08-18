using System;
using UnityEngine;

/// <summary>
/// A class ability that is selected at level up and used at various points.
/// </summary>
/// <remarks>
/// This class should be used as a base. Subclasses must implement the `Use`
/// method.
/// </remarks>
[System.Serializable]
public abstract class ClassAbility : ScriptableObject {
    [SerializeField] ActivatationPoint activatationPoint = ActivatationPoint.OnActivate;
    [SerializeField] string abilityName = "";
    [SerializeField] string abilityDescription = "";

    // PUBLIC

    /// <summary>
    /// Trigger the use of this ability. Override to provide functionality.
    /// </summary>
    /// <param name="user">The character that is using this action.</param>
    public virtual void Use(GameObject user) {}

    public ActivatationPoint GetActivatationPoint() { return activatationPoint; }
    public string GetAbilityName() { return abilityName; }
    public string GetAbilityDescription() { return abilityDescription; }
}