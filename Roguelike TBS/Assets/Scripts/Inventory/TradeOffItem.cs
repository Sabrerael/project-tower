using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Inventory/TradeOffPassiveItem"))]
public class TradeOffItem : PassiveItem {
    [SerializeField] Stat statToIncrease = Stat.Health;
    [SerializeField] int statIncrease = 1;
    [SerializeField] string increaseModifier = "Additive";
    [SerializeField] Stat statToDecrease = Stat.Attack;
    [SerializeField] int statDecrease = 1;
    [SerializeField] string decreaseModifier = "Additive";

    public void ApplyStatChanges(Inventory inventory) {
        if (increaseModifier == "Additive") {
            inventory.ModifyPassiveBonusAddition(statToIncrease, statIncrease);
        } else if (increaseModifier == "Multiplicative") {
            inventory.ModifyPassiveBonusPercentage(statToIncrease, statIncrease);
        }

        if (decreaseModifier == "Additive") {
            inventory.ModifyPassiveBonusAddition(statToDecrease, statDecrease);
        } else if (decreaseModifier == "Multiplicative") {
            inventory.ModifyPassiveBonusPercentage(statToDecrease, statDecrease);
        }
    }
}
