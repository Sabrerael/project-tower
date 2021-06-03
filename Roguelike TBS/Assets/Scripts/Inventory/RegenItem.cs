using UnityEngine;
using RPG.Stats;
using System.Collections;

[CreateAssetMenu(menuName = ("Inventory/Regen Item"))]
public class RegenItem: PassiveItem {
    [SerializeField] int regenAmount = 1;
    [SerializeField] float regenTimer = 45;

    public IEnumerator StartRegen() {
        while (true) {
            yield return new WaitForSeconds(regenTimer);
            GameObject.FindGameObjectWithTag("Player").GetComponent<Health>().Heal(regenAmount);
        }
    }
}
