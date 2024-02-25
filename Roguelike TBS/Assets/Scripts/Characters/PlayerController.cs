using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    private Fighter fighter;
    private Inventory inventory;
    private ActionItemInventory actionItemInventory;
    private Animator animator;
    private Movement movement;
    private Character character;
    private PotionInventory potionInventory;

    private void Start() {
        fighter = GetComponent<Fighter>();
        inventory = GetComponent<Inventory>();
        animator = GetComponent<Animator>();
        movement = GetComponent<Movement>();
        character = GetComponent<Character>();
        actionItemInventory = GetComponent<ActionItemInventory>();
        potionInventory = GetComponent<PotionInventory>();
    }

    private void OnAttack(InputValue value) {
        if (value.isPressed) {
            fighter.CheckIfSwinging();
        }
    }

    private void OnMove(InputValue value) {
        if (value.Get<Vector2>().magnitude <= Mathf.Epsilon) {
            animator.SetBool("IsWalking", false);
        }
        
        movement.SetMovementValues(value.Get<Vector2>());
    }

    private void OnRoll(InputValue value) {
        if (value.isPressed) {
            fighter.DodgeRoll();
        }
    }

    private void OnCharacterAbility(InputValue value) {
        if (value.isPressed) {
            character.ActiveAbility();
        }
    }

    private void OnWeaponAbility(InputValue value) {
        if (value.isPressed) {
            fighter.GetWeapon().UseActiveAbility();
        }
    }

    private void OnPause(InputValue value) {
        if (value.isPressed) {
            // TODO Probably not the best way to handle this
            GameObject.Find("Floor Manager").GetComponent<FloorManager>().TogglePause();
        }
    }

    private void OnActiveItem(InputValue value) {
        if (value.isPressed) {
            actionItemInventory.UseActiveItem();
        }
    }

    private void OnSwitchItem(InputValue value) {
        if (value.isPressed) {
            actionItemInventory.SwitchActiveItem();
        }
    }

    private void OnChangeWeapon(InputValue value) {
        if (value.isPressed) {
            inventory.ChangeWeapon();
        }
    }

    private void OnDeleteWeapon(InputValue value) {
        if (value.isPressed) {
            inventory.DeleteEquipWeapon();
        }
    }

    private void OnPotion(InputValue value) {
        if (value.isPressed) {
            potionInventory.UsePotion();
        }
    }
}
