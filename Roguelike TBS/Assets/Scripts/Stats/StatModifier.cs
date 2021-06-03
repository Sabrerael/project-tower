using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Stats;

public class StatModifier : MonoBehaviour, IModifierProvider {
    private int multiplicativeModifier = 0;

    public void StartStatTimer(int time, int percent) {
        StartCoroutine(PotionTimer(time, percent));
    } 

    private void AddModifier(int percent) {
        multiplicativeModifier += percent;
    }

    private void RemoveModifier(int percent) {
        multiplicativeModifier -= percent;
    }

    private IEnumerator PotionTimer(int time, int percent) {
        AddModifier(percent);
        yield return null;
    }

    public IEnumerable<int> GetAdditiveModifiers(Stat stat)
    {
        // Not used in StatModifier
        yield return 0;
    }

    public IEnumerable<int> GetMultiplicativeModifiers(Stat stat)
    {
        if (stat == Stat.Attack) {
            yield return multiplicativeModifier;
        }
    }
}
