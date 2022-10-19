using RPG.Stats;
using UnityEngine;

public class IceGolem : AttackingEnemy {
    [SerializeField] GameObject iceShard = null;
    [SerializeField] float[] angles = null;

    private GameObject[] iceShardArray = new GameObject[6];

    public void DeathExplosion() {
        for (int i = 0; i < iceShardArray.Length; i++) {
            iceShardArray[i] = Instantiate(iceShard, transform.position, Quaternion.identity);
            iceShardArray[i].transform.rotation = Quaternion.Euler(0, 0, angles[i]);
            // TODO fix these GetComponents?
            iceShardArray[i].GetComponent<EnemyProjectile>().SetDamage(gameObject.GetComponent<BaseStats>().GetStat(Stat.Attack));
            iceShardArray[i].GetComponent<EnemyProjectile>().SetWielder(gameObject.GetComponent<Enemy>());

            var xRatio = Mathf.Cos(angles[i] * Mathf.Deg2Rad);
            var yRatio = Mathf.Sin(angles[i] * Mathf.Deg2Rad);
            iceShardArray[i].GetComponent<EnemyProjectile>().SetMovementValues(5 * xRatio, 5 * yRatio);
        }
    }
}
