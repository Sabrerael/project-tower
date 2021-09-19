using UnityEngine;
using RPG.Stats;
using System.Collections.Generic;

[CreateAssetMenu(menuName = ("Inventory/Stat Modifier Item"))]
public class StatModifierItem: PassiveItem {
    [SerializeField] int modifierAmount = 1;
    [SerializeField] Stat statModified = Stat.Attack;
    [SerializeField] string typeOfModifier = "Additive";

    public void ApplyStatChanges(Inventory inventory) {
        if (typeOfModifier == "Additive") {
            inventory.ModifyPassiveBonusAddition(statModified, modifierAmount);
        } else if (typeOfModifier == "Multiplicative") {
            inventory.ModifyPassiveBonusPercentage(statModified, modifierAmount);
        }
    }
}
