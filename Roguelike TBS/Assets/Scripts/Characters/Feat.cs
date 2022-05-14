using UnityEngine;

/// <summary>
/// A class ability that is selected at level up and used at various points.
/// </summary>
/// <remarks>
/// This class should be used as a base. Subclasses must implement the `Use`
/// method.
/// </remarks>
[System.Serializable]
[CreateAssetMenu(fileName = "New Feat", menuName = "Feat")]
public class Feat : ScriptableObject {
    [SerializeField] ActivatationPoint activatationPoint = ActivatationPoint.OnActivate;
    [SerializeField] string abilityName = "";
    [SerializeField] string abilityDescription = "";
    [SerializeField] TargetingStrategy targetingStrategy; // Currently this should only be SelfTargeting
    [SerializeField] EffectStrategy[] effectStrategies;

    // PUBLIC

    /// <summary>
    /// Trigger the use of this ability. Override to provide functionality.
    /// </summary>
    /// <param name="user">The character that is using this action.</param>
    public virtual void Use(GameObject user) {
        AbilityData data = new AbilityData(user.gameObject);

        targetingStrategy.StartTargeting(data,
            () => {
                TargetAcquired(data);
            });
    }

    public ActivatationPoint GetActivatationPoint() { return activatationPoint; }
    public string GetAbilityName() { return abilityName; }
    public string GetAbilityDescription() { return abilityDescription; }

    // PRIVATE     
    private void TargetAcquired(AbilityData data) {
        foreach (var effect in effectStrategies) {
            effect.StartEffect(data, EffectFinished);
        }
    }

    private void EffectFinished() {}
}