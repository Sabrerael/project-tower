using UnityEngine;
using RPG.Stats;

[CreateAssetMenu(menuName = ("Inventory/Health Potion"))]
public class HealthPotion: ActionItem {
    [SerializeField] int healingAmount = 10;
    [SerializeField] GameObject onUseParticleEffect = null;
    [SerializeField] AudioClip sfx = null;

    // PUBLIC

    /// <summary>
    /// Trigger the use of this item.
    /// </summary>
    /// <param name="user">The character that is using this action.</param>
    public override void Use(GameObject user) {
        user.GetComponent<Health>().Heal(healingAmount);
        AudioSource.PlayClipAtPoint(sfx, user.transform.position);
        OnUseEffect(user);
    }

    private void OnUseEffect(GameObject user) {
        if (onUseParticleEffect == null) { return; }

        Instantiate(onUseParticleEffect, user.transform);
    }
}
