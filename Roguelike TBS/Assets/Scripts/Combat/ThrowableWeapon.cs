﻿using System.Collections;
using UnityEngine;

public class ThrowableWeapon : Weapon {
    [SerializeField] float cooldownTimer = 10;
    [SerializeField] float throwSpeed = 6;

    private bool thrown = false;
    private bool returning = false;
    private bool onCooldown = false;

    private void OnCollisionEnter2D(Collision2D other) {
        Debug.Log("In OnCollisionEnter2D");
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
    }

    public override void UseActiveAbility() {
        // Throw the weapon. If thrown, you can recall it.
        // TODO Broken. Will attempt to fix later.
        return;
        /*if (!isSwinging && Input.GetKeyDown(KeyCode.R) && !onCooldown) {
            if (!thrown) {
                var mouse = Input.mousePosition;
                var screenPoint = Camera.main.WorldToScreenPoint(wielder.transform.localPosition);
                var offset = new Vector2(mouse.x - screenPoint.x, mouse.y - screenPoint.y);
                var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;

                var xRatio = Mathf.Cos(angle * Mathf.Deg2Rad);
                var yRatio = Mathf.Sin(angle * Mathf.Deg2Rad);

                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(xRatio*throwSpeed, yRatio*throwSpeed);
                gameObject.GetComponent<BoxCollider2D>().enabled = true;
                gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
            } else if (thrown) {
                Debug.Log("In thrown if statement");

                thrown = false;
                returning = true;
            } else if (returning) {

            }
        }*/
    }

    public override void UpdateWeaponAngles(Direction direction) {
        if (thrown || returning) { return; }

        if (direction == Direction.Up) {
            // Sword is over head
            startingAngle = -45f;
            endingAngle = 95f;
        } else if (direction == Direction.Right) {
            // Sword is to the right
            startingAngle = -20f;
            endingAngle = -160f;
        } else if (direction == Direction.Down) {
            // Sword is under
            startingAngle = -100f;
            endingAngle = -240f;
        } else if (direction == Direction.Left){ 
            // Sword is to the left
            startingAngle = 20f;
            endingAngle = 160f;
        }

        if (!isSwinging) {
            gameObject.transform.rotation = Quaternion.Euler(0,0, startingAngle);
        }
    }

    private IEnumerator CooldownTimer() {
        onCooldown = true;

        yield return new WaitForSeconds(cooldownTimer);

        onCooldown = false;
    }
}