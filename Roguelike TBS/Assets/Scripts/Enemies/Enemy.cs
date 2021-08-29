using UnityEngine;
using RPG.Stats;

public class Enemy : MonoBehaviour {
    [SerializeField] protected AudioClip sfx = null;
    [SerializeField] protected float movementSpeed = 0f;
    [SerializeField] int moneyDropped = 1;

    /// CACHE
    protected GameObject player;

    // Start is called before the first frame update
    private void Awake() {
        player = GameObject.FindGameObjectWithTag("Player");

        GetComponent<Health>().onDeath += AddMoney;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Weapon") {
            gameObject.GetComponent<Animator>().SetTrigger("Hit");
            GameObject wielder = other.gameObject.GetComponent<Weapon>().GetWielder().gameObject;
            var damageTaken = wielder.GetComponent<BaseStats>().GetStat(Stat.Attack) - gameObject.GetComponent<BaseStats>().GetStat(Stat.Defense);
            Debug.Log("Damage being dealt is " + damageTaken);
            GetComponent<Health>().TakeDamage(wielder, damageTaken);

            if (sfx) {
                AudioSource.PlayClipAtPoint(sfx, transform.position);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Item") {
            gameObject.GetComponent<Animator>().SetTrigger("Hit");
            var damageTaken = other.gameObject.GetComponent<ThrownItem>().GetDamage() - gameObject.GetComponent<BaseStats>().GetStat(Stat.Defense);

            GetComponent<Health>().TakeDamage(
                other.gameObject.GetComponent<ThrownItem>().GetWielder().gameObject,
                damageTaken
            );
            if (sfx) {
                AudioSource.PlayClipAtPoint(sfx, transform.position);
            }
        } else if (other.gameObject.tag == "Magic") {
            gameObject.GetComponent<Animator>().SetTrigger("Hit");
            var damageTaken = other.gameObject.GetComponent<MagicMissile>().GetDamage() - gameObject.GetComponent<BaseStats>().GetStat(Stat.Defense);

            GetComponent<Health>().TakeDamage(
                other.gameObject.GetComponent<MagicMissile>().GetCaster().gameObject,
                damageTaken
            );
            if (sfx) {
                AudioSource.PlayClipAtPoint(sfx, transform.position);
            }
        }
    }

    public void ModifyMovementSpeed(float speedChange) {
        Debug.Log("In ModifyMovementSpeed");
        if (Mathf.Approximately(speedChange, 0)) {
            movementSpeed = 0;
        } else {
            movementSpeed = Mathf.Max(movementSpeed - speedChange, 0);
        }
    }

    private void AddMoney() {
        player.GetComponent<Purse>().UpdateBalance(moneyDropped);
    }
}
