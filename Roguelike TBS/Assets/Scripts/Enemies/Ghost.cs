using RPG.Stats;
using UnityEngine;

public class Ghost : Enemy {
    [SerializeField] float alphaChangeTime = 1f;
    [SerializeField] float disappearenceTime = 5f;
    [SerializeField] float visableTime = 15f;
    [SerializeField] float xMin = -0.5f;
    [SerializeField] float xMax = 12.5f;
    [SerializeField] float yMin = -2.5f;
    [SerializeField] float yMax = -10.5f;

    private float timer = -1;
    private bool isDisappearring = true;
    private bool isInvisible = false;
    private bool isReappearing = false;
    private bool isVisible = false;

    private void Update() {
        if (gameObject.GetComponent<Health>().IsDead()) { return; }

        timer += Time.deltaTime;

        if (timer < 0) { return; }

        if (isDisappearring) {
            gameObject.GetComponent<Collider2D>().enabled = false;
            var currentColor = gameObject.GetComponent<SpriteRenderer>().color;
            var alphaChange = Time.deltaTime / alphaChangeTime;
            currentColor.a = Mathf.Max(currentColor.a - alphaChange, 0);
            gameObject.GetComponent<SpriteRenderer>().color = currentColor;

            if (currentColor.a == 0) {
                isDisappearring = false;
                isInvisible = true;
                timer = 0;
            }
        } else if (isInvisible) {
            if (timer >= disappearenceTime) {
                gameObject.transform.localPosition = new Vector3(
                    Random.Range(xMin, xMax),
                    Random.Range(yMin, yMax),
                    0
                );

                isInvisible = false;
                isReappearing = true;
            }
        } else if (isReappearing) { 
            var currentColor = gameObject.GetComponent<SpriteRenderer>().color;
            var alphaChange = Time.deltaTime / alphaChangeTime;
            currentColor.a = Mathf.Min(currentColor.a + alphaChange, 1);
            gameObject.GetComponent<SpriteRenderer>().color = currentColor;

            if (currentColor.a == 1) {
                isReappearing = false;
                isVisible = true;
                timer = 0;
                GetComponent<Animator>().SetBool("Moving", true);
                GetComponent<Collider2D>().enabled = true;
            }
        } else if (isVisible) {
            float playerX = player.transform.position.x;
            float playerY = player.transform.position.y;

            float translateX = (playerX - transform.position.x) * movementSpeed;
            float translateY = (playerY - transform.position.y) * movementSpeed;

            transform.Translate(translateX  * Time.deltaTime, translateY  * Time.deltaTime, 0f);

            if (timer > visableTime) {
                isVisible = false;
                isDisappearring = true;
                GetComponent<Animator>().SetBool("Moving", false);
            }
        }
    }
}
