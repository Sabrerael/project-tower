using Pathfinding;
using RPG.Stats;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour {
    [SerializeField] protected AudioClip sfx = null;
    [SerializeField] protected float movementSpeed = 0f;
    [SerializeField] int moneyDropped = 1;
    [SerializeField] protected HealthBar healthBar;
    [SerializeField] protected float knockbackForce = 1000000;
    [SerializeField] protected float knockbackTime = 0.35f;

    /// CACHE
    protected AIDestinationSetter aiDestinationSetter;
    protected AIPath aiPath;    
    protected Animator animator;
    protected BaseStats baseStats;
    protected GameObject player;
    protected Health health;
    protected Rigidbody2D enemyRigidbody2D;

    // Start is called before the first frame update
    protected void Awake() {
        player = GameObject.FindGameObjectWithTag("Player");
        aiDestinationSetter = GetComponent<AIDestinationSetter>();
        aiDestinationSetter.target = player.transform;
        aiPath = GetComponent<AIPath>();
        health = GetComponent<Health>();
        animator = GetComponent<Animator>();
        baseStats = GetComponent<BaseStats>();
        enemyRigidbody2D = GetComponent<Rigidbody2D>();

        health.onDeath += AddMoney;
        health.onDeath += SetRigidbodyTypeToStatic;
    }

    protected virtual void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Weapon")) {
            animator.SetTrigger("Hit");
            GameObject wielder = other.gameObject.GetComponent<Weapon>().GetWielder().gameObject;
            var damageTaken = wielder.GetComponent<BaseStats>().GetStat(Stat.Attack) - baseStats.GetStat(Stat.Defense);
            Debug.Log("Damage being dealt is " + damageTaken);
            health.TakeDamage(wielder, damageTaken);

            if (sfx) {
                AudioSource.PlayClipAtPoint(sfx, transform.position);
            }

            StartCoroutine(Knockback(other));
        }
    }

    protected void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Item")) {
            animator.SetTrigger("Hit");
            var damageTaken = other.gameObject.GetComponent<ThrownItem>().GetDamage() - baseStats.GetStat(Stat.Defense);

            health.TakeDamage(
                other.gameObject.GetComponent<ThrownItem>().GetWielder().gameObject,
                damageTaken
            );
            if (sfx) {
                AudioSource.PlayClipAtPoint(sfx, transform.position);
            }
        } else if (other.gameObject.CompareTag("Magic")) {
            animator.SetTrigger("Hit");
            var damageTaken = other.gameObject.GetComponent<MagicMissile>().GetDamage() - baseStats.GetStat(Stat.Defense);

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
        aiDestinationSetter.enabled = false;
        if (aiPath) {
            aiPath.enabled = false;
        }
    }

    private void AddMoney() {
        player.GetComponent<Purse>().UpdateBalance(moneyDropped);
    }

    protected virtual IEnumerator Knockback(Collider2D other) {
        aiPath.enabled = false;
        enemyRigidbody2D.AddForce((transform.position - other.transform.position).normalized * knockbackForce);
        yield return new WaitForSeconds(knockbackTime);
        enemyRigidbody2D.velocity = Vector3.zero;
        yield return new WaitForSeconds(knockbackTime);
        aiPath.enabled = true;
    }
}
