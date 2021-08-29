using UnityEngine;
using UnityEngine.UI;

public class Pickup : MonoBehaviour {
    [SerializeField] InventoryItem item = null;
    [SerializeField] int number = 1;
    [SerializeField] Image itemSprite = null;
    [SerializeField] AudioClip sfx = null;

    private void Start() {
        if (item) {
            SetPickupSprite();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            DoPickup(other);
        }
    }

    public void SetItem(InventoryItem itemToSet, int quantity) { 
        item = itemToSet;
        number = quantity;
        SetPickupSprite();
    } 

    public void SetNumber(int num) { number = num; }

    private void DoPickup(Collider2D other) {
        if (item as WeaponConfig) {
            other.gameObject.GetComponent<Fighter>().EquipWeapon(item as WeaponConfig);
            other.gameObject.GetComponent<Inventory>().AddToFirstEmptyWeaponSlot(item as WeaponConfig);
            Destroy(gameObject);
        } else if (item as ActionItem) {
            other.gameObject.GetComponent<Inventory>().AddToFirstEmptyActionItemSlot(item as ActionItem, number);
            Destroy(gameObject);
        } else if (item as PassiveItem) {
            other.gameObject.GetComponent<Inventory>().AddToFirstEmptyPassiveItemSlot(item as PassiveItem);
            Destroy(gameObject);
        }
        if (sfx) { AudioSource.PlayClipAtPoint(sfx, transform.position); }
    }

    private void SetPickupSprite() {
        if (itemSprite) {
            itemSprite.sprite = item.GetIcon();
        }
    }
}
