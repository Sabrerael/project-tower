using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] WeaponConfig weaponConfig = null;
    [SerializeField] ActionItem item = null;
    [SerializeField] PassiveItem passiveItem = null;
    [SerializeField] int number = 1;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            DoPickup(other);
        }
    }

    public void SetNumber(int num) { number = num; }

    private void DoPickup(Collider2D other) {
        if (weaponConfig) {
            other.gameObject.GetComponent<Fighter>().EquipWeapon(weaponConfig);
            other.gameObject.GetComponent<Inventory>().AddToFirstEmptyWeaponSlot(weaponConfig);
            Destroy(gameObject);
        } else if (item) {
            other.gameObject.GetComponent<Inventory>().AddToFirstEmptyActionItemSlot(item, number);
            Destroy(gameObject);
        } else if (passiveItem) {
            other.gameObject.GetComponent<Inventory>().AddToFirstEmptyPassiveItemSlot(passiveItem);
            Destroy(gameObject);
        }
    }
}
