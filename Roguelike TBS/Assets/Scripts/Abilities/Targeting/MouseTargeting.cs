using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "Mouse Targeting", menuName = "Abilities/Targeting/Mouse")]
public class MouseTargeting : TargetingStrategy {
    public override void StartTargeting(AbilityData data, Action finished) {
        data.SetTargetedPoint(Mouse.current.position.ReadValue());
        finished();
    }
}