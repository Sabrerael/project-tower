using System.Collections;
using RPG.Stats;
using UnityEngine;

public class GelatinousCube : Boss {
    [SerializeField] EnemySpawner[] slimeSpawners = null;
    [SerializeField] float xMax, xMin, yMax, yMin = 0f; 
    [SerializeField] float jumpTime = 1f;

    private int numOfAttacks = 0;
    private CubePhase phase = CubePhase.Starting;

    //Attack Pattern: Jump to corner, move to opposite wall, repeat x4 or x8, 
    //                random jumps, jump to center and spawn slimes, back to beginning

    enum CubePhase {
        Starting,
        Jump1,
        Move1,
        Jump2,
        Move2,
        Jump3,
        Move3,
        Jump4,
        Move4,
        RandomJump1,
        RandomJump2,
        RandomJump3,
        RandomJump4,
        JumpToCenter,
        SpawnSlimes,
        Spawning,
        Idle
    }

    struct Corner {
        public float x;
        public float y;
    }

    private Corner[] corners = new Corner[4];
    private bool inMove = false;
    private Vector2 moveLocation;
    private float moveDistance;

    private void Start() {
        Corner corner = new Corner();
        corner.x = xMin;
        corner.y = yMin;
        corners[0] = corner;

        corner.x = xMin;
        corner.y = yMax;
        corners[1] = corner;
        
        corner.x = xMax;
        corner.y = yMin;
        corners[2] = corner;

        corner.x = xMax;
        corner.y = yMax;
        corners[3] = corner;
        SetUpBossHealthBar();
    }

    private void Update() {
        if (gameObject.GetComponent<Health>().IsDead()) { return; }

        if (phase == CubePhase.Idle) { return; }

        if (inMove) {
            transform.position = Vector3.MoveTowards(transform.position, moveLocation, moveDistance);
            if (Vector3.Distance(transform.position, moveLocation) < 0.001) {
                inMove = false;
                StartCoroutine(IdleTimer(0.3f));
            } 
        } else {
            if (phase == CubePhase.Move1 || phase == CubePhase.Move2 ||
                phase == CubePhase.Move3 || phase == CubePhase.Move4) {
                if (transform.position.x - xMin <= 0.002f) {
                    AttackMoveRight();
                } else if (xMax - transform.position.x <= 0.002f) {
                    AttackMoveLeft();
                }
            } else if (phase == CubePhase.Jump1 || phase == CubePhase.Jump2 ||
                       phase == CubePhase.Jump3 || phase == CubePhase.Jump4) {
                JumpToRandomCorner();
            } else if (phase == CubePhase.RandomJump1 || phase == CubePhase.RandomJump2 ||
                       phase == CubePhase.RandomJump3 || phase == CubePhase.RandomJump4) {
                JumpToRandomSpot();
            } else if (phase == CubePhase.JumpToCenter) {
                JumpToCenter();
            } else if (phase == CubePhase.SpawnSlimes) {
                StartCoroutine(SpawnSlimes());
            } else if (phase == CubePhase.Starting) {
                StartCoroutine(IdleTimer(1));
            }
        }
    }

    private void ChangeBossPhase() {
        switch (phase) {
            case CubePhase.Starting:
                phase = CubePhase.Jump1;
                break;
            case CubePhase.Jump1:
                phase = CubePhase.Move1;
                break;
            case CubePhase.Move1:
                phase = CubePhase.Jump2;
                break;
            case CubePhase.Jump2:
                phase = CubePhase.Move2;
                break;
            case CubePhase.Move2:
                phase = CubePhase.Jump3;
                break;
            case CubePhase.Jump3:
                phase = CubePhase.Move3;
                break;
            case CubePhase.Move3:
                phase = CubePhase.Jump4;
                break;
            case CubePhase.Jump4:
                phase = CubePhase.Move4;
                break;
            case CubePhase.Move4:
                phase = CubePhase.RandomJump1;
                break;
            case CubePhase.RandomJump1:
                phase = CubePhase.RandomJump2;
                break;
            case CubePhase.RandomJump2:
                phase = CubePhase.RandomJump3;
                break;
            case CubePhase.RandomJump3:
                phase = CubePhase.RandomJump4;
                break;
            case CubePhase.RandomJump4:
                phase = CubePhase.JumpToCenter;
                break;
            case CubePhase.JumpToCenter:
                phase = CubePhase.SpawnSlimes;
                break;
            case CubePhase.Spawning:
                phase = CubePhase.Jump1;
                break;
      }
    }

    private void AttackMoveLeft() {
        moveLocation = new Vector2(xMin, transform.position.y);
        moveDistance = movementSpeed*Time.fixedDeltaTime;
        transform.position = Vector3.MoveTowards(transform.position, moveLocation, moveDistance);
        inMove = true;
        GetComponent<Animator>().SetTrigger("MovingLeft");
    }
    
    private void AttackMoveRight() {
        moveLocation = new Vector2(xMax, transform.position.y);
        moveDistance = movementSpeed*Time.fixedDeltaTime;
        transform.position = Vector3.MoveTowards(transform.position, moveLocation, moveDistance);
        inMove = true;
        GetComponent<Animator>().SetTrigger("MovingRight");
    }

    private void JumpToRandomCorner() {
        var randomCorner = ChooseRandomCorner();
        moveLocation = new Vector2(randomCorner.x, randomCorner.y);
        moveDistance = Vector3.Distance(transform.position, moveLocation) * (Time.fixedDeltaTime / jumpTime);
        Debug.Log(moveDistance + "; " + Time.fixedDeltaTime);
        transform.position = Vector3.MoveTowards(transform.position, moveLocation, moveDistance);
        inMove = true;
        GetComponent<Animator>().SetTrigger("Jumping");
    }

    private void JumpToRandomSpot() {
        moveLocation = new Vector2(Random.Range(xMin, xMax), Random.Range(yMin, yMax));
        moveDistance = Vector3.Distance(transform.position, moveLocation) * (Time.fixedDeltaTime / jumpTime);
        Debug.Log(moveDistance + "; " + Time.fixedDeltaTime);
        transform.position = Vector3.MoveTowards(transform.position, moveLocation, moveDistance);
        inMove = true;
        GetComponent<Animator>().SetTrigger("Jumping");
    }

    private void JumpToCenter() {
        moveLocation = new Vector2(6, -6);
        moveDistance = Vector3.Distance(transform.position, moveLocation) * (Time.fixedDeltaTime / jumpTime);
        Debug.Log(moveDistance + "; " + Time.fixedDeltaTime);
        transform.position = Vector3.MoveTowards(transform.position, moveLocation, moveDistance);
        inMove = true;
        GetComponent<Animator>().SetTrigger("Jumping");
    }

    private IEnumerator SpawnSlimes() {
        GetComponent<Animator>().SetTrigger("Spawning");
        phase = CubePhase.Spawning;
        yield return new WaitForSeconds(1);
        foreach (var spawner in slimeSpawners) {
            spawner.SpawnEnemy(transform.parent.gameObject);
            yield return new WaitForSeconds(1);
        }
        yield return new WaitForSeconds(1);
        StartCoroutine(IdleTimer(2.5f));
    }

    private Corner ChooseRandomCorner() {
        Corner corner; 
        do {
            corner = corners[Random.Range(0, 4)];
        } while(Mathf.Abs(corner.x - transform.position.x) <= 0.002f && 
                Mathf.Abs(corner.y - transform.position.y) <= 0.002f);
        return corner;
    }

    private IEnumerator IdleTimer(float time) {
        var heldPhase = phase;
        phase = CubePhase.Idle;
        yield return new WaitForSeconds(time);
        phase = heldPhase;
        ChangeBossPhase();
    }
}
