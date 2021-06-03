using System.Collections;
using System.Collections.Generic;
using RPG.Stats;
using UnityEngine;

public class Buffs : MonoBehaviour, IModifierProvider {
    Dictionary<Stat, int> additiveBuffs = new Dictionary<Stat, int>();
    Dictionary<Stat, int> multiplicativeBuffs = new Dictionary<Stat, int>();

    private void Awake() {
        additiveBuffs.Add(Stat.Attack, 0);
        additiveBuffs.Add(Stat.AttackSpeed, 0);
        additiveBuffs.Add(Stat.Cooldown, 0);
        additiveBuffs.Add(Stat.Defense, 0);
        additiveBuffs.Add(Stat.Health, 0);
        additiveBuffs.Add(Stat.MovementSpeed, 0);

        multiplicativeBuffs.Add(Stat.Attack, 0);
        multiplicativeBuffs.Add(Stat.AttackSpeed, 0);
        multiplicativeBuffs.Add(Stat.Cooldown, 0);
        multiplicativeBuffs.Add(Stat.Defense, 0);
        multiplicativeBuffs.Add(Stat.Health, 0);
        multiplicativeBuffs.Add(Stat.MovementSpeed, 0);
    }

    private void AddAdditiveBuff(Stat stat, int percent) {
        additiveBuffs[stat] += percent;
    }

    private void RemoveAdditiveBuff(Stat stat, int percent) {
        additiveBuffs[stat] -= percent;
    }

    private void AddMultiplicativeBuff(Stat stat, int percent) {
        multiplicativeBuffs[stat] += percent;
    }

    private void RemoveMultiplicativeBuff(Stat stat, int percent) {
        multiplicativeBuffs[stat] -= percent;
    }

    public void StartAdditivetiveBuffTimer(Stat stat, int time, int percent) {
        AddAdditiveBuff(stat, percent);
        StartCoroutine(AdditiveBuffTimer(stat, time, percent));
    }

    private IEnumerator AdditiveBuffTimer(Stat stat, int time, int percent) {
        yield return new WaitForSeconds(time);

        RemoveAdditiveBuff(stat, percent);
    }

    public void StartMultiplicativeBuffTimer(Stat stat, int time, int percent) {
        AddMultiplicativeBuff(stat, percent);
        StartCoroutine(MultiplicativeBuffTimer(stat, time, percent));
    }

    private IEnumerator MultiplicativeBuffTimer(Stat stat, int time, int percent) {
        yield return new WaitForSeconds(time);

        RemoveMultiplicativeBuff(stat, percent);
    }

    public IEnumerable<int> GetAdditiveModifiers(Stat stat) {
        yield return additiveBuffs[stat];
    }

    public IEnumerable<int> GetMultiplicativeModifiers(Stat stat) {
        yield return multiplicativeBuffs[stat];
    }
}
