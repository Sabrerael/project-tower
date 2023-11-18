using System.Collections;
using UnityEngine;

public class SlimeMovement : Enemy {

    public void RunContactKnockback(Collider2D other) {
        StartCoroutine(ContactKnockback(other));
    }

    private  IEnumerator ContactKnockback(Collider2D other) {
        aiPath.enabled = false;
        enemyRigidbody2D.AddForce((transform.position - other.transform.position).normalized * knockbackForce);
        yield return new WaitForSeconds(knockbackTime);
        enemyRigidbody2D.velocity = Vector3.zero;
        yield return new WaitForSeconds(knockbackTime);
        aiPath.enabled = true;
    }

}
