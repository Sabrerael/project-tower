using UnityEngine;

[CreateAssetMenu(menuName = ("Inventory/Speed Up Gem"))]
public class SpeedUpGem: ActionItem {
    [SerializeField] int movementIncrease = 5;
    [SerializeField] int attackSpeedIncrease = 5;
    [SerializeField] int time = 0;
    [SerializeField] GameObject onUseParticleEffect = null;
    [SerializeField] AudioClip sfx = null;

    private float timer = 0f;
    private Buffs buffs = null;

    // PUBLIC

    /// <summary>
    /// Trigger the use of this item.
    /// </summary>
    /// <param name="user">The character that is using this action.</param>
    public override void Use(GameObject user) {
        buffs = user.GetComponent<Buffs>();

        buffs.StartMultiplicativeBuffTimer(Stat.MovementSpeed, time, movementIncrease);
        buffs.StartMultiplicativeBuffTimer(Stat.AttackSpeed, time, attackSpeedIncrease);

        //AudioSource.PlayClipAtPoint(sfx, user.transform.position);
        OnUseEffect(user);
    }

    private void OnUseEffect(GameObject user) {
        if (onUseParticleEffect == null) { return; }

        Instantiate(onUseParticleEffect, user.transform);
    }
}
