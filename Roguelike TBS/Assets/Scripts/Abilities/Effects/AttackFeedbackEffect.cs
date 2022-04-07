using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack Feedback Effect", menuName = "Abilities/Effects/Attack Feedback")]
public class AttackFeedbackEffect : EffectStrategy {
    [SerializeField] int denominator;
    [SerializeField] int numerator;

    public override void StartEffect(AbilityData data, Action finished) {
        Fighter fighter = data.GetUser().GetComponent<Fighter>();

        if (fighter) {
            fighter.onActualHit += FeedbackEffect;
        }
        
        finished();
    }

    public void FeedbackEffect(Enemy enemy) {
        var chance = UnityEngine.Random.Range(1, denominator);

        if (chance <= numerator) {
           enemy.ModifyMovementSpeed(0);
           enemy.GetComponent<SpriteRenderer>().color = Color.cyan;
        }
    }
}
