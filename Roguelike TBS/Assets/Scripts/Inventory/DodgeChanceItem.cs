using UnityEngine;

[CreateAssetMenu(menuName = ("Inventory/Dodge Chance Item"))]
public class DodgeChanceItem : PassiveItem {
    [SerializeField] int numerator = 1;
    [SerializeField] int denominator = 20;

    public bool WillDodge() {
        var chance = Random.Range(1, denominator);

        return chance <= numerator;
    }
}
