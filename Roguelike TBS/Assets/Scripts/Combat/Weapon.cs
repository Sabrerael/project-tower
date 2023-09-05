using UnityEngine;

public class Weapon : MonoBehaviour {
    [SerializeField] float baseAttackSpeed = 10;
    [SerializeField] protected WeaponConfig weaponConfig = null;
    [SerializeField] TargetingStrategy targetingStrategy;
    [SerializeField] EffectStrategy[] effectStrategies;

    protected Fighter wielder = null;
    private float timeCount = 0f;
    private float attackSpeedModifier = 1;
    protected bool waitingForNextAttack = false;
    protected WeaponState weaponState = WeaponState.Ready;

    public float GetBaseAttackSpeed() { return baseAttackSpeed; }
    public virtual int GetWeaponDamage() { return weaponConfig.GetWeaponDamage(); }
    public Fighter GetWielder() { return wielder; }

    public void SetWielder(Fighter fighter) {
        wielder = fighter;
    }
    
    public virtual void UseActiveAbility() {
        if (targetingStrategy == null) { return; }
        
        AbilityData data = new AbilityData(wielder.gameObject);
        targetingStrategy.StartTargeting(data,
            () => {
                TargetAcquired(data);
            });

    }

    private void TargetAcquired(AbilityData data) {
        foreach (var effect in effectStrategies) {
            effect.StartEffect(data, EffectFinished);
        }
    }

    private void EffectFinished() {}
}
