using UnityEngine;

public class Pickup : MonoBehaviour {
    [SerializeField] InventoryItem item = null;
    [SerializeField] int number = 1;
    [SerializeField] SpriteRenderer spriteRenderer = null;
    [SerializeField] AudioClip sfx = null;

    private void Start() {
        if (item.GetIcon()) {
            spriteRenderer.sprite = item.GetIcon();
        }    
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
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
            other.gameObject.GetComponent<ActionItemInventory>().AddToFirstEmptyActionItemSlot(item as ActionItem, number);
            Destroy(gameObject);
        } else if (item as PassiveItem) {
            other.gameObject.GetComponent<PassiveItemInventory>().AddToFirstEmptyPassiveItemSlot(item as PassiveItem);
            Destroy(gameObject);
        }
        if (sfx) { AudioSource.PlayClipAtPoint(sfx, transform.position); }
        GameObject.Find("HUD").GetComponent<HUD>().LaunchItemPopup(item.GetDisplayName() + ": " + item.GetDescription());
    }

    private void SetPickupSprite() {
        if (item) {
            spriteRenderer.sprite = item.GetIcon();
        }
    }
}
