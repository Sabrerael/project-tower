using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    private Fighter fighter;

    private void Start() {
        fighter = GetComponent<Fighter>();
    }

    private void OnAttack(InputValue value) {
        if (value.isPressed) {
            fighter.CheckIfSwinging();
        }
    }

    private void OnMove(InputValue value) {
        if (value.Get<Vector2>().magnitude <= Mathf.Epsilon) {
            GetComponent<Animator>().SetBool("IsWalking", false);
        }
        
        GetComponent<Movement>().SetMovementValues(value.Get<Vector2>());
    }

    private void OnRoll(InputValue value) {
        if (value.isPressed) {
            fighter.DodgeRoll();
        }
    }

    private void OnCharacterAbility(InputValue value) {
        if (value.isPressed) {
            GetComponent<Character>().ActiveAbility();
        }
    }

    private void OnWeaponAbility(InputValue value) {
        if (value.isPressed) {
            fighter.GetWeapon().UseActiveAbility();
        }
    }

    private void OnPause(InputValue value) {
        if (value.isPressed) {
            GameObject.Find("Floor Manager").GetComponent<FloorManager>().TogglePause();
        }
    }

    private void OnActiveItem(InputValue value) {
        if (value.isPressed) {
            GetComponent<Inventory>().UseItemInSlot(0);
        }
    }

    private void OnSwitchItem(InputValue value) {
        if (value.isPressed) {
            //GetComponent<Inventory>().UseItemInSlot(1);
        }
    }

    private void OnChangeWeapon(InputValue value) {
        if (value.isPressed) {
            GetComponent<Inventory>().ChangeWeapon();
        }
    }

    private void OnDeleteWeapon(InputValue value) {
        if (value.isPressed) {
            GetComponent<Inventory>().DeleteEquipWeapon();
        }
    }
}
