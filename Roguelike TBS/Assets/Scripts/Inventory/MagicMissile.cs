using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO Rename to Tracking Magic
public class MagicMissile : MonoBehaviour {
    [SerializeField] float movementSpeed = 7.5f;
    [SerializeField] float xMin, xMax, yMin, yMax = 0;
    [SerializeField] int damage = 10;

    // PRIVATE VARIABLES
    private GameObject target = null;
    private GameObject caster = null;
    private Vector3 startingPoint = new Vector3();
    private float deltaX = 0;
    private float deltaY = 0;

    private void Update() {
        if (!caster) { return; }
        target = caster.GetComponent<Character>().GetCurrentRoom().GetEnemiesParent().transform.GetChild(0).gameObject;
        startingPoint = transform.position;

        var offset = new Vector2(
            target.transform.position.x - gameObject.transform.position.x,
            target.transform.position.y - gameObject.transform.position.y
        );
        var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;

        var xRatio = Mathf.Cos(angle * Mathf.Deg2Rad);
        var yRatio = Mathf.Sin(angle * Mathf.Deg2Rad);

        deltaX = xRatio * movementSpeed * Time.deltaTime;
        deltaY = yRatio * movementSpeed * Time.deltaTime;

        float newXPos = transform.localPosition.x + deltaX;
        float newyPos = transform.localPosition.y + deltaY;

        transform.localPosition = new Vector2(newXPos, newyPos);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        Destroy(gameObject);
    }

    public int GetDamage() { return damage; }
    public GameObject GetCaster() { return caster; }

    public void SetCaster(GameObject value) { caster = value; }
}
