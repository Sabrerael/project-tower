using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// TODO Take common code and make a Boss class
public class GelatinousCube : Enemy {
    [SerializeField] EnemySpawner[] slimeSpawners = null;
    [SerializeField] float xMax, xMin, yMax, yMin = 0f; 
    [SerializeField] float jumpSpeed = 2f;

    private int numOfAttacks = 0;
    private CubePhase phase = CubePhase.Start;

    //Attack Pattern: Jump to corner, move to opposite wall, repeat x4 or x8, 
    //                random jumps, jump to center and spawn slimes, back to beginning

    enum CubePhase {
        Start,
        Moving,
        RandomJumps,
        SpawnSlimes,
    }

    struct Corner {
        public float x;
        public float y;
    }

    private Corner[] corners = new Corner[4];
    private bool inMove = false;
    private Vector2 moveLocation;

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
    }

    private void Update() {
        if (phase == CubePhase.Start) {
            if (!inMove) {
                var randomCorner = ChooseRandomCorner();
                moveLocation = new Vector2(randomCorner.x, randomCorner.y);
                transform.position = Vector3.MoveTowards(transform.position, moveLocation, movementSpeed*Time.deltaTime);
                inMove = true;
            } else {
                transform.position = Vector3.MoveTowards(transform.position, moveLocation, movementSpeed*Time.deltaTime);
                if (Vector3.Distance(transform.position, moveLocation) < 0.001) {
                    inMove = false;
                    phase = CubePhase.Moving;
                } 
            }
            
        }
    }

    void AttackMoveLeft() {

    }
    
    void AttackMoveRight() {

    }

    void JumpAttack() {

    }

    void SpawnSlimes() {

    }

    private Corner ChooseRandomCorner() {
        return corners[Random.Range(0, 4)];
    }

    public void ToggleCollider() {
        GetComponent<Collider2D>().enabled = !GetComponent<Collider2D>().enabled;
    }

}
