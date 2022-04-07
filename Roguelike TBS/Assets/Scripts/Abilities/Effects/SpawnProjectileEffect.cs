using System;
using RPG.Stats;
using UnityEngine;

[CreateAssetMenu(fileName = "Spawn Projectile Effect", menuName = "Abilities/Effects/Spawn Projectile")]
public class SpawnProjectileEffect : EffectStrategy {
    [SerializeField] Projectile projectileToSpawn;
    [SerializeField] float throwSpeed; //Not currently used, speed is set on the prefab
    [SerializeField] int damage;

    public override void StartEffect(AbilityData data, Action finished) {
        Projectile projectile = Instantiate(projectileToSpawn, data.GetUser().transform.position, Quaternion.identity);
        if (data.GetTargets() == null) {
            projectile.SetTarget(data.GetTargetedPoint(), data.GetUser(), damage);
        } else {
            foreach(var target in data.GetTargets()) {
                Health health = target.GetComponent<Health>();
                if (health) {
                    projectile.SetTarget(health, data.GetUser(), damage);
                } else {
                    projectile.SetTarget(target.transform.position, data.GetUser(), damage);
                }
            }
        }

        finished(); 
    }
}