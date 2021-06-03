using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "New Weapon", order = 0)]
public class WeaponConfig : InventoryItem {
    [SerializeField] Weapon equippedPrefab = null;
    [SerializeField] int weaponDamage = 5;

    const string weaponName = "Weapon";
    Weapon weapon = null;

    public void Spawn(Transform hand, Animator animator, Fighter fighter) {
        DestroyOldWeapon(hand);

        if (equippedPrefab != null) {
            weapon = Instantiate(equippedPrefab, hand);
            weapon.transform.rotation = Quaternion.Euler(0, 0, weapon.gameObject.GetComponent<Weapon>().GetStartingAngle());
            weapon.gameObject.GetComponent<Collider2D>().enabled = false;
            weapon.gameObject.name = weaponName;
            weapon.SetWielder(fighter);
        }

        /*var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
        if (animatorOverride != null) {
            animator.runtimeAnimatorController = animatorOverride; 
        }
        else if (overrideController != null) {
            animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
        }*/

        //return weapon;
    }

    private void DestroyOldWeapon(Transform hand) {
        Transform oldWeapon = hand.Find(weaponName);

        if (oldWeapon == null) return;

        oldWeapon.name = "DESTROYING";
        Destroy(oldWeapon.gameObject);
    }

    public int GetWeaponDamage() { return weaponDamage; }
}
