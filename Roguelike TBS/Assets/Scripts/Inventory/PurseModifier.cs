using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Inventory/Purse Modifier"))]
public class PurseModifier : PassiveItem {
    [SerializeField] float multiplier = 1.5f;

    public void AddPurseMultiplier(Purse purse) {
        purse.SetMoneyMultiplier(multiplier);
    }
}
