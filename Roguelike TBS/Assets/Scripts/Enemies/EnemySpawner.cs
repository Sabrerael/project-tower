using RPG.Stats;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    [SerializeField] Enemy enemyToSpawn = null;
    [SerializeField] int levelOfEnemy = 1;

    public void SpawnEnemy(GameObject enemiesParent) {
        var newEnemy = Instantiate(enemyToSpawn, gameObject.transform.position, Quaternion.identity);
        newEnemy.GetComponent<BaseStats>().SetLevel(levelOfEnemy);

        newEnemy.transform.SetParent(enemiesParent.transform);
    }

    public void SetEnemyLevel(int level) { levelOfEnemy = level; }
    public void SetEnemy(Enemy enemy) { enemyToSpawn = enemy; }
}
