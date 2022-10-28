using RPG.Stats;
using UnityEngine;

public class Enemy : MonoBehaviour {
    [SerializeField] protected AudioClip sfx = null;
    [SerializeField] protected float movementSpeed = 0f;
    [SerializeField] int moneyDropped = 1;
    [SerializeField] protected HealthBar healthBar;

    /// CACHE
    protected GameObject player;
    protected Health health;
    protected Animator animator;
    protected Rigidbody2D enemyRigidbody2D;

    // Start is called before the first frame update
    protected void Awake() {
        player = GameObject.FindGameObjectWithTag("Player");
        health = GetComponent<Health>();
        animator = GetComponent<Animator>();
        enemyRigidbody2D = GetComponent<Rigidbody2D>();

        health.onDeath += AddMoney;
        health.onDeath += SetRigidbodyTypeToStatic;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Weapon") {
            animator.SetTrigger("Hit");
            GameObject wielder = other.gameObject.GetComponent<Weapon>().GetWielder().gameObject;
            var damageTaken = wielder.GetComponent<BaseStats>().GetStat(Stat.Attack) - gameObject.GetComponent<BaseStats>().GetStat(Stat.Defense);
            Debug.Log("Damage being dealt is " + damageTaken);
            health.TakeDamage(wielder, damageTaken);

            if (sfx) {
                AudioSource.PlayClipAtPoint(sfx, transform.position);
            }
        }
    }

    protected void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Item") {
            animator.SetTrigger("Hit");
            var damageTaken = other.gameObject.GetComponent<ThrownItem>().GetDamage() - gameObject.GetComponent<BaseStats>().GetStat(Stat.Defense);

            health.TakeDamage(
                other.gameObject.GetComponent<ThrownItem>().GetWielder().gameObject,
                damageTaken
            );
            if (sfx) {
                AudioSource.PlayClipAtPoint(sfx, transform.position);
            }
        } else if (other.gameObject.tag == "Magic") {
            animator.SetTrigger("Hit");
            var damageTaken = other.gameObject.GetComponent<MagicMissile>().GetDamage() - gameObject.GetComponent<BaseStats>().GetStat(Stat.Defense);

            health.TakeDamage(
                other.gameObject.GetComponent<MagicMissile>().GetCaster().gameObject,
                damageTaken
            );
            if (sfx) {
                AudioSource.PlayClipAtPoint(sfx, transform.position);
            }
        }
    }

    public void ModifyMovementSpeed(float speedChange) {
        if (Mathf.Approximately(speedChange, 0)) {
            movementSpeed = 0;
        } else {
            movementSpeed = Mathf.Max(movementSpeed - speedChange, 0);
        }
    }

    public void SetRigidbodyTypeToStatic() {
        enemyRigidbody2D.bodyType = RigidbodyType2D.Static;
    }

    private void AddMoney() {
        player.GetComponent<Purse>().UpdateBalance(moneyDropped);
    }
}
