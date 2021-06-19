using RPG.Stats;
using UnityEngine;

public class Movement : MonoBehaviour {
    [SerializeField] float padding = 0.5f;
    [SerializeField] float speedUpRate = 0.4f;

    float xMin, xMax, yMin, yMax;

    private void Start() {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0,0,0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1,0,0)).x - padding;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0,0,0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0,1,0)).y - padding;
    }

    private void FixedUpdate() {
        // TODO this line gets called every frame. Make this an action? When Passive inventory happens? 
        var movementSpeed = GetComponent<BaseStats>().GetStat(Stat.MovementSpeed);

        //float deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * movementSpeed;
        //float deltaY = Input.GetAxis("Vertical") * Time.deltaTime * movementSpeed;

        //float newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        //float newyPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);

        //transform.position = new Vector2(newXPos, newyPos);

        if (Mathf.Approximately(Input.GetAxis("Horizontal"), 0) && Mathf.Approximately(Input.GetAxis("Vertical"), 0)) {
            GetComponent<Animator>().SetBool("IsWalking", false);
        } else {
            GetComponent<Animator>().SetBool("IsWalking", true);
        }
        GetComponent<Rigidbody2D>().AddForce(new Vector2(Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime,
                                       Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime));
        AddBrakeForce();
    }

    public void UpdateMinMaxValues() {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0,0,0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1,0,0)).x - padding;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0,0,0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0,1,0)).y - padding;
    }

    // This doesn't appear to be doing anything
    private void AddBrakeForce() {
        float speed = Vector3.Magnitude(GetComponent<Rigidbody2D>().velocity);  // test current object speed
      
        if (speed > 10) {
            float brakeSpeed = speed - 10;  // calculate the speed decrease
        
            Vector2 normalisedVelocity = GetComponent<Rigidbody2D>().velocity.normalized;
            Vector2 brakeVelocity = normalisedVelocity * brakeSpeed;  // make the brake Vector3 value
        
            GetComponent<Rigidbody2D>().AddForce(-brakeVelocity);  // apply opposing brake force
        }
    }
}
