using UnityEngine;

public class DropSpawner : MonoBehaviour {
    [SerializeField] Pickup pickup = null;

    [SerializeField] InventoryItem itemToDrop;
    [SerializeField] int numberOfItem = 1;

    public Pickup GetPickup() { return pickup; }

    public void SetItemToDrop(InventoryItem item) { itemToDrop = item; }
    public void SetNumberOfItem(int number) { numberOfItem = number; }

    public void SpawnPickup() {
        var pickupObject = Instantiate(pickup, gameObject.transform.position, Quaternion.identity);
        pickupObject.GetComponent<Pickup>().SetItem(itemToDrop, numberOfItem);
    }
}
