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
    [SerializeField] ActivationPoint activationPoint = ActivationPoint.OnActivate;
    [SerializeField] string abilityName = "";
    [SerializeField] string abilityDescription = "";
    [SerializeField] EffectStrategy[] effectStrategies;

    // PUBLIC

    /// <summary>
    /// Trigger the use of this ability. Override to provide functionality.
    /// </summary>
    /// <param name="user">The character that is using this action.</param>
    public virtual void Use(GameObject user) {}

    public ActivationPoint GetActivationPoint() { return activationPoint; }
    public string GetAbilityName() { return abilityName; }
    public string GetAbilityDescription() { return abilityDescription; }
}