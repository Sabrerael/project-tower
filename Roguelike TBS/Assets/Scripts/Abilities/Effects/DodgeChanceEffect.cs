using UnityEngine;

[CreateAssetMenu(fileName = "Dodge Chance Effect", menuName = "Abilities/Effects/Dodge Chance")]
public class DodgeChanceEffect : EffectStrategy {
    [SerializeField] int numerator = 1;
    [SerializeField] int denominator = 20;

    public override void StartEffect(AbilityData data, System.Action finished) {
        Fighter fighter = data.GetUser().GetComponent<Fighter>();

        if (fighter) {
            fighter.onInitialHit += WillDodge;
        }
        
        finished();
    }

    public bool WillDodge() {
        var chance = Random.Range(1, denominator);

        return chance <= numerator;
    }
}
