using RPG.Stats;
using UnityEngine;

public class MovementAdjustmentAOE : AOE {
    [SerializeField] float movementAdjustment = 0f;
    [SerializeField] bool locksAfterTrigger = false;

    private bool triggered = false;

    protected override void OnTriggerStay2D(Collider2D other) {
        return;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.GetComponent<Health>() == null) { return; }
        
        other.gameObject.GetComponent<Health>().TakeDamage(instigator, damage);
        other.gameObject.GetComponent<Animator>().SetTrigger("Hit");
        other.GetComponent<Enemy>().ModifyMovementSpeed(movementAdjustment);

        if (locksAfterTrigger) {
            Destroy(gameObject);
        }
    }
}
