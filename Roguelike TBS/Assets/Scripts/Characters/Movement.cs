using RPG.Stats;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour {
    [SerializeField] float movementSpeed = 6;
    [SerializeField] float padding = 0.5f;
    [SerializeField] float dodgeSpeed;
    [SerializeField] ParticleSystem dust = null;

    private Animator animator;
    private Rigidbody2D playerRigidbody;
    private float xMin, xMax, yMin, yMax;
    private Vector2 movementValues = new Vector2();
    private bool isDodging = false;
    private bool movingRight = false;
    private bool movingDown = true;
    private bool movingVertical = true;
    private Vector3 dodgeDirection = new Vector3();
    private float t;
    private float dustTimer = 0;

    private void Start() {
        playerRigidbody = GetComponent<Rigidbody2D>();
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0,0,0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1,0,0)).x - padding;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0,0,0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0,1,0)).y - padding;
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate() {
        if (isDodging) {
            DodgeRoll();
        } else if (movementValues.magnitude > Mathf.Epsilon) {
            NormalMovement();
        } else {
            dustTimer = 5;
        }
    }

    private void DodgeRoll() {
        playerRigidbody.AddForce(new Vector2(dodgeDirection.x * dodgeSpeed,
                                             dodgeDirection.y * dodgeSpeed));
        t += Time.fixedDeltaTime;
    }

    public void StartDodgeRolling() {
        if (movementValues.magnitude < Mathf.Epsilon) {
            if (!movingVertical && !movingRight) {
                dodgeDirection = Vector3.left;
            } else if (!movingVertical && movingRight) {
                dodgeDirection = Vector3.right;
            } else if (movingVertical && movingDown) {
                dodgeDirection = Vector3.down;
            } else if (movingVertical && !movingDown) {
                dodgeDirection = Vector3.up;
            } 
        } else {
            dodgeDirection = movementValues;
        }

        isDodging = true;
        animator.SetTrigger("DodgeRoll");
        t = 0;
        dust.Play();
    }

    // Keeping this in case I need it. I should be good without it but I'm not sure
    public void UpdateMinMaxValues() {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0,0,0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1,0,0)).x - padding;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0,0,0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0,1,0)).y - padding;
    }

    public void SetMovementValues(Vector2 values) {
        movementValues = values;
    }

    private void EndDodgeRoll() {
        isDodging = false;
    }

    public void NormalMovement() {
        animator.SetBool("IsWalking", true);

        if (movementValues.x < 0) {
            animator.SetBool("WalkingRight", false);
            animator.SetBool("MovingVertical", false);
            movingRight = false;
            movingVertical = false;
        } else if (movementValues.x > 0) {
            animator.SetBool("WalkingRight", true);
            animator.SetBool("MovingVertical", false);
            movingRight = true;
            movingVertical = false;
        } else if (movementValues.y < Mathf.Epsilon) {
            animator.SetBool("WalkingDown", true);
            animator.SetBool("MovingVertical", true);
            movingDown = true;
            movingVertical = true;
        } else if (movementValues.y > Mathf.Epsilon) {
            animator.SetBool("WalkingDown", false);
            animator.SetBool("MovingVertical", true);
            movingDown = false;
            movingVertical = true;
        } 

        playerRigidbody.AddForce(new Vector2(movementValues.x * movementSpeed,
                                             movementValues.y * movementSpeed));
        
        if (dustTimer > 1.5) {
            dust.Play();
            dustTimer = 0;
        } else {
            dustTimer += Time.fixedDeltaTime;
        }
    }
}
