using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityData {
    GameObject user;
    Vector3 targetedPoint;
    IEnumerable<GameObject> targets;

    public AbilityData(GameObject user) {
        this.user = user;
    }

    public GameObject GetUser() { return user; }
    public Vector3 GetTargetedPoint() { return targetedPoint; }
    public IEnumerable<GameObject> GetTargets() { return targets; }

    public void SetTargetedPoint(Vector3 targetedPoint) { this.targetedPoint = targetedPoint; }
    public void SetTargets(IEnumerable<GameObject> targets) { this.targets = targets; }

    public void StartCoroutine(IEnumerator coroutine) {
        user.GetComponent<MonoBehaviour>().StartCoroutine(coroutine);
    }
}
