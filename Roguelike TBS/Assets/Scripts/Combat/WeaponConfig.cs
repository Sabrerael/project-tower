using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Weapon", menuName = "New Weapon", order = 0)]
public class WeaponConfig : InventoryItem {
    [SerializeField] Weapon equippedPrefab = null;
    [SerializeField] int weaponDamage = 5;
    [SerializeField] Sprite abilityIcon = null;
    [SerializeField] WeaponType weaponType = WeaponType.OneHanded;

    const string weaponName = "Weapon";
    Weapon weapon = null;

    public void Spawn(Transform hand, Animator animator, Fighter fighter) {
        DestroyOldWeapon(hand);

        if (equippedPrefab != null) {
            weapon = Instantiate(equippedPrefab, hand);
            weapon.gameObject.GetComponent<Collider2D>().enabled = false;
            weapon.gameObject.name = weaponName;
            weapon.SetWielder(fighter);
            if (abilityIcon) {
                var weaponAbilityIcon = GameObject.Find("Weapon Ability Icon");
                weaponAbilityIcon.GetComponent<Image>().sprite = abilityIcon;
                weaponAbilityIcon.GetComponent<Image>().color = new Color(1,1,1,0.75f);
            } else {
                var weaponAbilityIcon = GameObject.Find("Weapon Ability Icon");
                weaponAbilityIcon.GetComponent<Image>().sprite = null;
                weaponAbilityIcon.GetComponent<Image>().color = Color.clear;
            }
        }
        HandlePassive(fighter.gameObject);
    }

    private void DestroyOldWeapon(Transform hand) {
        Transform oldWeapon = hand.Find(weaponName);

        if (oldWeapon == null) return;

        oldWeapon.name = "DESTROYING";
        Destroy(oldWeapon.gameObject);
    }

    public int GetWeaponDamage() { return weaponDamage; }

    public virtual void HandlePassive(GameObject user) {
        // Overrided in specific files for now
    }
}
