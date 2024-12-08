﻿using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Stats;
using UnityEngine;
using UnityEngine.UI;

public enum AbilityState {
    Ready,
    Active,
    Cooldown
}

public class Paladin : Character {
    [Header("Paladin Active Ability Modifiers")]
    [SerializeField] int abilityModifyPercent = 30;
    //[SerializeField] float abilityTimer = 30; // Should be necessary when the temporary health points go away
    [SerializeField] int temporaryHealthPoints = 15;

    private Health health;

    private event Action<GameObject> onAbilityActivate;
    private event Action<GameObject> onAbilityKill;
    public override event Action onFeatAdded;

    private void Start() {
        levelUpMenu = GameObject.Find("Level Up Bonuses Menu").GetComponent<LevelUpBonusMenu>();

        foreach(Stat stat in Enum.GetValues(typeof(Stat))) {
            activeAbilityModifyPercentages[stat] = 0;
        }

        health = GetComponent<Health>();
    }

    public override void ActiveAbility() {
        if (abilityState != AbilityState.Ready) { return; }

        GetComponent<Animator>().SetTrigger("DivineProtection");
        abilityState = AbilityState.Active;
        if (abilityParticles != null) { 
            Instantiate(abilityParticles, transform);
        }
        if (onAbilityActivate != null) {
            onAbilityActivate(gameObject);
        }

        health.SetTemporaryHealth(temporaryHealthPoints);
        health.onTemporaryHealthGone += StartAbilityCooldown;
    }

    public override void CallOnAbilityKill() {
        if (abilityState == AbilityState.Active && onAbilityKill != null) {
            onAbilityKill(gameObject);
        }
    }
    
    public override IEnumerable<int> GetMultiplicativeModifiers(Stat stat) {
        if (abilityState == AbilityState.Active) {
            yield return activeAbilityModifyPercentages[stat] + passiveModifyPercentages[stat];
        }
        yield return passiveModifyPercentages[stat];
    }

    protected override void HandleSelectedClassAbility(Feat ability) {
        selectedAbilities.Add(ability);
        if (ability.GetActivationPoint() == ActivationPoint.Passive) {
            ability.Use(gameObject);
        } else if (ability.GetActivationPoint() == ActivationPoint.OnActivate) {
            onAbilityActivate += ability.Use;
        } else if (ability.GetActivationPoint() == ActivationPoint.OnKill) {
            onAbilityKill += ability.Use;
        }
        if (onFeatAdded != null) {
            onFeatAdded();
        }
    }

    private void StartAbilityCooldown() {
        StartCoroutine(AbilityCooldown());
    }

    private IEnumerator AbilityCooldown() {
        abilityState = AbilityState.Cooldown;
        cooldownTimer = baseStats.GetStat(Stat.Cooldown);

        yield return new WaitForSeconds(cooldownTimer);

        characterAbilityIcon.GetComponent<Image>().color = new Color(1,1,1,0.75f);
        abilityState = AbilityState.Ready;
    }

    public void ModifyTemporaryHealthAmount(int temporaryHealthIncrease) {
        temporaryHealthPoints += temporaryHealthIncrease;
    }
}
