using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Inventory/Attack Feedback Item"))]
public class AttackFeedbackItem : PassiveItem {
    [SerializeField] int numerator = 1;
    [SerializeField] int denominator = 20;

    // Will need to expand this in the future to allow for other effects
    public void FeedbackEffect(Enemy enemy) {
        var chance = Random.Range(1, denominator);

        if (chance <= numerator) {
           enemy.ModifyMovementSpeed(0);
           enemy.GetComponent<SpriteRenderer>().color = Color.cyan;
        }
    }
}
