using System.Collections;
using RPG.Stats;
using UnityEngine;

public class Balor : Boss {
    enum BalorPhase {
        Starting,
        Idle,
        LongswordAttack,
        WhipAttack,
        FireAttack,
        Teleport
    }

    [SerializeField] float xMin, xMax, yMin, yMax = 0f;
    [SerializeField] float fireSpeed = 5;
    [SerializeField] GameObject fireAttack = null;
    [SerializeField] Collider2D longswordCollider = null;
    [SerializeField] Collider2D whipCollider = null;

    private BalorPhase phase = BalorPhase.Starting;

    private void Start() {
        SetUpBossHealthBar();
    }

    private void Update() {
        if (gameObject.GetComponent<Health>().IsDead()) { return; }

        if (phase == BalorPhase.Idle) { return; }

        if (phase == BalorPhase.FireAttack) {
            GetComponent<Animator>().SetTrigger("FireAttack");
            StartCoroutine(IdleTimer(2));
        } else if (phase == BalorPhase.Teleport) {
            GetComponent<Animator>().SetTrigger("Teleport");
            StartCoroutine(IdleTimer(2));
        } else if (phase == BalorPhase.WhipAttack) {
            GetComponent<Animator>().SetTrigger("Whip");
            StartCoroutine(IdleTimer(2));
        } else if (phase == BalorPhase.LongswordAttack) {
            GetComponent<Animator>().SetTrigger("Longsword");
            StartCoroutine(IdleTimer(2));
        } else if (phase == BalorPhase.Starting) {
            StartCoroutine(IdleTimer(1));
        }
        
    }

    private void ChangeBossPhase() {
        switch (phase) {
            case BalorPhase.LongswordAttack:
                phase = BalorPhase.Teleport;
                break;
            case BalorPhase.Teleport:
                phase = BalorPhase.WhipAttack;
                break;
            case BalorPhase.WhipAttack:
                phase = BalorPhase.FireAttack;
                break;
            case BalorPhase.FireAttack:
                phase = BalorPhase.LongswordAttack;
                break;
            case BalorPhase.Starting:
                phase = BalorPhase.LongswordAttack;
                break;
        }
    }

    // TODO This is a super long function
    public IEnumerator LaunchFireAttack() {
        GameObject spawnedItem = Instantiate(fireAttack, transform.position + new Vector3(0, 1.2f, 0), Quaternion.identity);
        spawnedItem.GetComponent<EnemyProjectile>().SetDamage(gameObject.GetComponent<BaseStats>().GetStat(Stat.Attack));
        spawnedItem.GetComponent<EnemyProjectile>().SetWielder(gameObject.GetComponent<Enemy>());

        var offset = new Vector2(
            player.transform.position.x - spawnedItem.transform.position.x,
            player.transform.position.y - spawnedItem.transform.position.y
        );

        var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;

        var xRatio = Mathf.Cos(angle * Mathf.Deg2Rad);
        var yRatio = Mathf.Sin(angle * Mathf.Deg2Rad);

        spawnedItem.transform.rotation = Quaternion.Euler(0, 0, angle);

        spawnedItem.GetComponent<EnemyProjectile>().SetMovementValues(xRatio * fireSpeed, yRatio * fireSpeed);

        yield return new WaitForSeconds(.5f);

        spawnedItem = Instantiate(fireAttack, transform.position + new Vector3(0, 1.2f, 0), Quaternion.identity);
        spawnedItem.GetComponent<EnemyProjectile>().SetDamage(gameObject.GetComponent<BaseStats>().GetStat(Stat.Attack));
        spawnedItem.GetComponent<EnemyProjectile>().SetWielder(gameObject.GetComponent<Enemy>());

        offset = new Vector2(
            player.transform.position.x - spawnedItem.transform.position.x,
            player.transform.position.y - spawnedItem.transform.position.y
        );

        angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;

        xRatio = Mathf.Cos(angle * Mathf.Deg2Rad);
        yRatio = Mathf.Sin(angle * Mathf.Deg2Rad);

        spawnedItem.transform.rotation = Quaternion.Euler(0, 0, angle);

        spawnedItem.GetComponent<EnemyProjectile>().SetMovementValues(xRatio * fireSpeed, yRatio * fireSpeed);

        yield return new WaitForSeconds(.5f);

        spawnedItem = Instantiate(fireAttack, transform.position + new Vector3(0, 1.2f, 0), Quaternion.identity);
        spawnedItem.GetComponent<EnemyProjectile>().SetDamage(gameObject.GetComponent<BaseStats>().GetStat(Stat.Attack));
        spawnedItem.GetComponent<EnemyProjectile>().SetWielder(gameObject.GetComponent<Enemy>());

        offset = new Vector2(
            player.transform.position.x - spawnedItem.transform.position.x,
            player.transform.position.y - spawnedItem.transform.position.y
        );

        angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;

        xRatio = Mathf.Cos(angle * Mathf.Deg2Rad);
        yRatio = Mathf.Sin(angle * Mathf.Deg2Rad);

        spawnedItem.transform.rotation = Quaternion.Euler(0, 0, angle);

        spawnedItem.GetComponent<EnemyProjectile>().SetMovementValues(xRatio * fireSpeed, yRatio * fireSpeed);
    }

    public void TeleportMove() {
        transform.position = new Vector2(Random.Range(xMin, xMax), Random.Range(yMin, yMax));
    }

    public void ToggleLongswordCollider() {
        longswordCollider.enabled = !longswordCollider.enabled;
    }

    public void ToggleWhipCollider() {
        whipCollider.enabled = !whipCollider.enabled;
    }

    private IEnumerator IdleTimer(float time) {
        var heldPhase = phase;
        phase = BalorPhase.Idle;
        yield return new WaitForSeconds(time);
        phase = heldPhase;
        ChangeBossPhase();
    }
}
