using UnityEngine;

public class ThrownItem : MonoBehaviour {
    [SerializeField] int contactDamage = 1;
    
    private Fighter wielder = null;

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player")) {
            Destroy(gameObject);
        }
    }

    public int GetDamage() { return contactDamage; }
    public Fighter GetWielder() { return wielder; }

    public void SetWielder(Fighter fighter) { wielder = fighter; }
}
