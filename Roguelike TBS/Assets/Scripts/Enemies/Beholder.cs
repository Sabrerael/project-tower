using System.Collections;
using RPG.Stats;
using UnityEngine;

public class Beholder : Boss {
    enum BeholderPhase {
        Starting,
        Idle,
        Bite,
        MainEyeLazer,
        LittleEyeLazers,
        SwingStalks,
        Attacking,
        AttackingWithBite
    }

    [SerializeField] float lazerSpeed = 5f;
    [SerializeField] float biteMovementSpeed = 10f;
    [SerializeField] float xMin, xMax, yMin, yMax = 0f;
    [SerializeField] GameObject smallEyeLazerBolt = null;
    [SerializeField] GameObject mainEyeLazer = null;
    [SerializeField] Transform[] smallEyeLocations = null;

    private BeholderPhase phase = BeholderPhase.Starting;
    [SerializeField] Vector3 targetForMovement = new Vector3();
    private float moveDistance;

    private void Start() {
        SetUpBossHealthBar();
        SelectMovementTarget();
    }

    private void Update() {
        if (gameObject.GetComponent<Health>().IsDead()) { return; }

        if (phase != BeholderPhase.AttackingWithBite) {
            transform.position = Vector3.MoveTowards(transform.position, targetForMovement, moveDistance);
            if (Vector3.Distance(transform.position, targetForMovement) < 0.001) {
                SelectMovementTarget();
                StartCoroutine(IdleTimer(0.3f));
            }
        } else {
            transform.position = Vector3.MoveTowards(transform.position, targetForMovement, moveDistance);
            if (Vector3.Distance(transform.position, targetForMovement) < 0.001) {
                BiteAttack();
                SelectMovementTarget();
                StartCoroutine(IdleTimer(0.3f));
            }
        }

        if (phase == BeholderPhase.Idle || phase == BeholderPhase.Attacking ) { return; }

        if (phase == BeholderPhase.LittleEyeLazers) {
            StartCoroutine(LittleEyeLazerPhase());
        } else if (phase == BeholderPhase.Bite) {
            Bite();
        } else if (phase == BeholderPhase.MainEyeLazer) {
            StartCoroutine(MainEyeLazerPhase());
        } else if (phase == BeholderPhase.SwingStalks) {
            StartCoroutine(SwingStalks());
        } else if (phase == BeholderPhase.Starting) {
            StartCoroutine(IdleTimer(1));
        }
    }

    private void ChangeBossPhase() {
        switch (phase) {
            case BeholderPhase.LittleEyeLazers:
                phase = BeholderPhase.SwingStalks;
                break;
            case BeholderPhase.SwingStalks:
                phase = BeholderPhase.MainEyeLazer;
                break;
            case BeholderPhase.MainEyeLazer:
                phase = BeholderPhase.Bite;
                break;
            case BeholderPhase.Bite:
                phase = BeholderPhase.LittleEyeLazers;
                break;
            case BeholderPhase.Starting:
                phase = BeholderPhase.LittleEyeLazers;
                break;
        }
    }

    private void Bite() {
        //Start a move to the player, stand still and bite
        targetForMovement = player.transform.position;
        moveDistance = biteMovementSpeed * Time.fixedDeltaTime;
        phase = BeholderPhase.AttackingWithBite;
    }

    private void BiteAttack(){
        GetComponent<Animator>().SetTrigger("Bite");
        phase = BeholderPhase.Bite;
        StartCoroutine(IdleTimer(1f));
    }

    private IEnumerator MainEyeLazerPhase(){
        phase = BeholderPhase.Attacking;
        yield return new WaitForSeconds(0.3f);

        GetComponent<Animator>().SetTrigger("BigEyeLazer");
        yield return new WaitForSeconds(7f);

        phase = BeholderPhase.MainEyeLazer;
        StartCoroutine(IdleTimer(1f));
    }

    private IEnumerator FireMainEyeLazer() {
        for (int i = 0; i < 24; i++) {
            GameObject spawnedItem = Instantiate(mainEyeLazer, transform.position, Quaternion.identity);
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

            spawnedItem.GetComponent<EnemyProjectile>().SetMovementValues(xRatio * lazerSpeed, yRatio * lazerSpeed);

            yield return new WaitForSeconds(0.25f);
        }
    }

    private IEnumerator LittleEyeLazerPhase() {
        phase = BeholderPhase.Attacking;
        yield return new WaitForSeconds(0.3f);

        var array = ShuffleArray();

        for (int i = 0; i < 6; i++) {
            GetComponent<Animator>().SetTrigger("LittleLazer" + array[i]);
            yield return new WaitForSeconds(.8f);
        }
        phase = BeholderPhase.LittleEyeLazers;
        StartCoroutine(IdleTimer(1f));
    }

    private void FireLittleEyeLazer(int eyeNumber) {
        GameObject spawnedItem = Instantiate(smallEyeLazerBolt, smallEyeLocations[eyeNumber].position, Quaternion.identity);
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

        spawnedItem.GetComponent<EnemyProjectile>().SetMovementValues(xRatio * lazerSpeed, yRatio * lazerSpeed);
    }

    private void SelectMovementTarget() {
        targetForMovement = new Vector2(Random.Range(xMin, xMax), Random.Range(yMin, yMax));
        moveDistance = movementSpeed * Time.fixedDeltaTime;
    }

    private int[] ShuffleArray() {
        var array = new int[] {1, 2, 3, 4, 5, 6}; 
        int tempIndex;

        for (int i = 0; i < array.Length - 1; i++) {
              int rnd = Random.Range(i, array.Length);
              tempIndex = array[rnd];
              array[rnd] = array[i];
              array[i] = tempIndex;
        }
        return array;
    }

    private IEnumerator SwingStalks() {
        phase = BeholderPhase.Attacking;
        yield return new WaitForSeconds(0.3f);
        
        GetComponent<Animator>().SetTrigger("SwingLeft");
        yield return new WaitForSeconds(0.8f);
        GetComponent<Animator>().SetTrigger("SwingRight");
        yield return new WaitForSeconds(0.8f);

        phase = BeholderPhase.SwingStalks;
        StartCoroutine(IdleTimer(1f));
    }

    private IEnumerator IdleTimer(float time) {
        var heldPhase = phase;
        phase = BeholderPhase.Idle;
        yield return new WaitForSeconds(time);
        phase = heldPhase;
        ChangeBossPhase();
    }
}
