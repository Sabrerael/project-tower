using UnityEngine;

public class DropSpawner : MonoBehaviour {
    [SerializeField] Pickup pickup = null;

    public Pickup GetPickup() { return pickup; }
    public void SetPickup(Pickup newPickup) {pickup = newPickup;}

    public void SpawnPickup() {
        Instantiate(pickup, gameObject.transform.position, Quaternion.identity);
    }
}
