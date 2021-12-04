using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    private void OnAttack(InputValue value) {
        if (value.isPressed) {
            GetComponent<Fighter>().CheckIfSwinging();
        }
    }

    private void OnMove(InputValue value) {
        if (value.Get<Vector2>().magnitude <= Mathf.Epsilon) {
            GetComponent<Animator>().SetBool("IsWalking", false);
        } else {
            
        }
        GetComponent<Movement>().SetMovementValues(value.Get<Vector2>());
    }

    private void OnRoll(InputValue value) {
        if (value.isPressed) {
            GetComponent<Fighter>().DodgeRoll();
        }
    }

    private void OnCharacterAbility(InputValue value) {
        if (value.isPressed) {
            GetComponent<Character>().ActiveAbility();
        }
    }

    private void OnWeaponAbility(InputValue value) {
        if (value.isPressed) {
            GetComponent<Weapon>().UseActiveAbility();
        }
    }

    private void OnPause(InputValue value) {
        if (value.isPressed) {
            GameObject.Find("Floor Manager").GetComponent<FloorManager>().TogglePause();
        }
    }

    private void OnItem1(InputValue value) {
        if (value.isPressed) {
            GetComponent<Inventory>().UseItemInSlot(0);
        }
    }

    private void OnItem2(InputValue value) {
        if (value.isPressed) {
            GetComponent<Inventory>().UseItemInSlot(1);
        }
    }

    private void OnItem3(InputValue value) {
        if (value.isPressed) {
            GetComponent<Inventory>().UseItemInSlot(2);
        }
    }

    private void OnItem4(InputValue value) {
        if (value.isPressed) {
            GetComponent<Inventory>().UseItemInSlot(3);
        }
    }

    private void OnItem5(InputValue value) {
        if (value.isPressed) {
            GetComponent<Inventory>().UseItemInSlot(4);
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
