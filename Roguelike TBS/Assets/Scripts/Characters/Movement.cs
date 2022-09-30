using RPG.Stats;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour {
    [SerializeField] float movementSpeed = 6;
    [SerializeField] float velocityAdjustment = 50000;
    [SerializeField] float padding = 0.5f;
    [SerializeField] float speedUpRate = 0.4f;
    [SerializeField] float dodgeSpeed;
    [SerializeField] float dodgeDistance;

    private Animator animator;
    private Rigidbody2D playerRigidbody;
    private float xMin, xMax, yMin, yMax;
    private Vector2 movementValues = new Vector2();
    private float deltaX = 0;
    private float deltaY = 0;
    private bool isDodging = false;
    private Vector3 dodgeStartingPosition = new Vector3();
    private Vector3 dodgeEndingPosition = new Vector3();
    private float t;

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
        }
    }

    private void DodgeRoll() {
        transform.localPosition = new Vector3(Mathf.Lerp(dodgeStartingPosition.x, dodgeEndingPosition.x, t),
                                              Mathf.Lerp(dodgeStartingPosition.y, dodgeEndingPosition.y, t));

        t += Time.fixedDeltaTime * dodgeSpeed;
    }

    public void StartDodgeRolling() {
        if (movementValues.magnitude < Mathf.Epsilon) { return; }

        isDodging = true;
        dodgeStartingPosition = transform.localPosition;
        dodgeEndingPosition = transform.localPosition + new Vector3(dodgeDistance*movementValues.x, dodgeDistance*movementValues.y);
        animator.SetTrigger("DodgeRoll");
        t = 0;
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
        } else if (movementValues.x > 0) {
            animator.SetBool("WalkingRight", true);
            animator.SetBool("MovingVertical", false);
        } else if (movementValues.y < Mathf.Epsilon) {
            animator.SetBool("WalkingDown", true);
            animator.SetBool("MovingVertical", true);
        } else if (movementValues.y > Mathf.Epsilon) {
            animator.SetBool("WalkingDown", false);
            animator.SetBool("MovingVertical", true);
        } 

        playerRigidbody.AddForce(new Vector2(movementValues.x * movementSpeed*velocityAdjustment * Time.fixedDeltaTime,
                                       movementValues.y * movementSpeed*velocityAdjustment * Time.fixedDeltaTime));
    }
}
