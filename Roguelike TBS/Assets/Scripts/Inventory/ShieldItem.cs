using UnityEngine;

[CreateAssetMenu(menuName = ("Inventory/Shield Item"))]
public class ShieldItem : PassiveItem {
    [SerializeField] int numerator = 1;
    [SerializeField] int denominator = 20;

    public bool WillShieldPlayer() {
        var chance = Random.Range(1, denominator);

        return chance <= numerator;
    }

}
