using UnityEngine;

public class IceGolem : AttackingEnemy {
    [SerializeField] GameObject iceShard = null;

    public void DeathExplosion() {
        // TODO Finish writing this out and decide if this spawns an actual explosion in addition to the shards of ice
        Instantiate(iceShard, transform.position, Quaternion.identity);

    }
}
