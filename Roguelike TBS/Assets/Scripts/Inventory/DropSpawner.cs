using UnityEngine;

public class DropSpawner : MonoBehaviour {
    [SerializeField] Pickup pickup = null;

    [SerializeField] InventoryItem itemToDrop;
    [SerializeField] int numberOfItem = 1;
    [SerializeField] AudioClip spawnSound = null;

    public Pickup GetPickup() { return pickup; }
    public InventoryItem GetItemToDrop() { return itemToDrop; }

    public void SetItemToDrop(InventoryItem item) { itemToDrop = item; }
    public void SetNumberOfItem(int number) { numberOfItem = number; }

    public void SpawnPickup() {
        var pickupObject = Instantiate(pickup, gameObject.transform.position, Quaternion.identity);
        pickupObject.GetComponent<Pickup>().SetItem(itemToDrop, numberOfItem);
        if (spawnSound) { AudioSource.PlayClipAtPoint(spawnSound, transform.position); }
    }
}
