using UnityEngine;
using RPG.Stats;
using System.Collections.Generic;

[CreateAssetMenu(menuName = ("Inventory/Stat Modifier Item"))]
public class StatModifierItem: PassiveItem {
    [SerializeField] int modifierAmount = 1;
    [SerializeField] Stat statModified = Stat.Attack;
    [SerializeField] string typeOfModifier = "Additive";

    public int GetModifierAmount() { return modifierAmount; }
    public Stat GetStatModified() { return statModified; }
    public string GetTypeOfModifier() { return typeOfModifier; }
}
