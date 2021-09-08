using RPG.Stats;
using UnityEngine;

public class Movement : MonoBehaviour {
    [SerializeField] float padding = 0.5f;
    [SerializeField] float speedUpRate = 0.4f;
    [SerializeField] float dodgeSpeed;
    [SerializeField] float dodgeDistance;

    private float xMin, xMax, yMin, yMax;
    private float deltaX = 0;
    private float deltaY = 0;
    private bool isDodging = false;
    private Vector3 dodgeStartingPosition = new Vector3();

    private void Start() {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0,0,0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1,0,0)).x - padding;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0,0,0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0,1,0)).y - padding;
    }

    private void FixedUpdate() {
        if (isDodging) {
            DodgeRoll();
        } else {
            NormalMovement();
        }
    }

    public void StartDodgeRolling() {
        isDodging = true;
        dodgeStartingPosition = transform.localPosition;
        var mouse = Input.mousePosition;
        var screenPoint = Camera.main.WorldToScreenPoint(dodgeStartingPosition);
        var offset = new Vector2(mouse.x - screenPoint.x, mouse.y - screenPoint.y);
        var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;

        var xRatio = Mathf.Cos(angle * Mathf.Deg2Rad);
        var yRatio = Mathf.Sin(angle * Mathf.Deg2Rad);
        deltaX = xRatio * dodgeSpeed * Time.deltaTime;
        deltaY = yRatio * dodgeSpeed * Time.deltaTime;
    }

    public void UpdateMinMaxValues() {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0,0,0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1,0,0)).x - padding;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0,0,0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0,1,0)).y - padding;
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

    private void NormalMovement() {
        var movementSpeed = GetComponent<BaseStats>().GetStat(Stat.MovementSpeed);

        float deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * movementSpeed;
        float deltaY = Input.GetAxis("Vertical") * Time.deltaTime * movementSpeed;

        float newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        float newyPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);

        transform.position = new Vector2(newXPos, newyPos);
    }
}
