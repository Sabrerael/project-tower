using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Spawn Target Prefab Effect", menuName = "Abilities/Effects/Spawn Target Prefab")]
public class SpawnTargetPrefabEffect : EffectStrategy {
    [SerializeField] Transform prefabToSpawn = null;
    [SerializeField] float destroyDelay = -1;

    public override void StartEffect(AbilityData data, Action finished) {
        data.StartCoroutine(Effect(data, finished));
    }

    private IEnumerator Effect(AbilityData data, Action finished) {
        Transform instance = Instantiate(prefabToSpawn);
        instance.position = data.GetTargetedPoint();

        if (destroyDelay > 0) {
            yield return new WaitForSeconds(destroyDelay);
            Destroy(instance.gameObject);
        }
        finished();
    }
}
