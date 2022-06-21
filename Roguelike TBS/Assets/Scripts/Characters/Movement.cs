using RPG.Stats;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour {
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

    public void StartDodgeRolling() {
        isDodging = true;
        dodgeStartingPosition = transform.localPosition;
        var mouse = Mouse.current.position.ReadValue();
        var screenPoint = Camera.main.WorldToScreenPoint(dodgeStartingPosition);
        var offset = new Vector2(mouse.x - screenPoint.x, mouse.y - screenPoint.y);
        var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;

        var xRatio = Mathf.Cos(angle * Mathf.Deg2Rad);
        var yRatio = Mathf.Sin(angle * Mathf.Deg2Rad);
        deltaX = xRatio * dodgeSpeed * Time.fixedDeltaTime;
        deltaY = yRatio * dodgeSpeed * Time.fixedDeltaTime;

        GetComponent<Animator>().SetTrigger("DodgeRoll");
    }

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

    private void DodgeRoll() {
        float newXPos = Mathf.Clamp(transform.localPosition.x + deltaX, xMin, xMax);
        float newyPos = Mathf.Clamp(transform.localPosition.y + deltaY, yMin, yMax);

        transform.localPosition = new Vector2(newXPos, newyPos);

        if ((dodgeStartingPosition - transform.localPosition).magnitude >= dodgeDistance) {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
            isDodging = false;
        }
    }

    public void NormalMovement() {
        animator.SetBool("IsWalking", true);
        var movementSpeed = GetComponent<BaseStats>().GetStat(Stat.MovementSpeed);

        Debug.Log(movementValues);

        float deltaX = movementValues.x * Time.fixedDeltaTime * movementSpeed;
        float deltaY = movementValues.y * Time.fixedDeltaTime * movementSpeed;

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

        float newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        float newyPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);

        transform.position = new Vector2(newXPos, newyPos);
    }
}
