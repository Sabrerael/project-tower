using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Invincibility Effect", menuName = "Abilities/Effects/Invincibility")]
public class InvincibilityEffect : EffectStrategy {
    [SerializeField] float invulnerabilityTimer;

    public override void StartEffect(AbilityData data, Action finished) {
        Fighter fighter = data.GetUser().GetComponent<Fighter>();
        fighter.StartIFrameTimer(invulnerabilityTimer);
    }
}
