using System;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] float abilityTimer = 30;

    private event Action<GameObject> onAbilityActivate;
    private event Action<GameObject> onAbilityKill;

    private void Start() {
        foreach(Stat stat in Enum.GetValues(typeof(Stat))) {
            activeAbilityModifyPercentages[stat] = 0;
        }

        activeAbilityModifyPercentages[Stat.Defense] = abilityModifyPercent;
    }

    private void Update() {
        if (abilityState == AbilityState.Ready) {
            // TODO Pull out Input code into a PlayerController class
            if (Input.GetKeyDown(KeyCode.Q)) {
                ActiveAbility();
            }
        }
    }

    public override void ActiveAbility() {
        abilityState = AbilityState.Active;
        if (abilityParticles != null) { 
            Instantiate(abilityParticles, transform);
        }
        if (onAbilityActivate != null) {
            onAbilityActivate(gameObject);
        }
        StartCoroutine(StatTimer(abilityTimer, abilityModifyPercent));
    }

    public override void CallOnAbilityKill() {
        if (abilityState == AbilityState.Active && onAbilityKill != null) {
            onAbilityKill(gameObject);
        }
    }
    
    public override IEnumerable<int> GetMultiplicativeModifiers(Stat stat) {
        if (abilityState == AbilityState.Active) {
            yield return activeAbilityModifyPercentages[stat];
        }
        yield return 0;
    }

    protected override void HandleSelectedClassAbility(ClassAbility ability) {
        selectedAbilities.Add(ability);
        if (ability.GetActivatationPoint() == ActivatationPoint.Passive) {
            ability.Use(gameObject);
        } else if (ability.GetActivatationPoint() == ActivatationPoint.OnActivate) {
            onAbilityActivate += ability.Use;
        } else if (ability.GetActivatationPoint() == ActivatationPoint.OnKill) {
            onAbilityKill += ability.Use;
        }
    }

    private IEnumerator StatTimer(float time, int percent) {
        characterAbilityIcon.GetComponent<Image>().color = new Color(0.5f,0.5f,0.5f,0.25f);

        yield return new WaitForSeconds(time);

        abilityState = AbilityState.Cooldown;
        cooldownTimer = baseStats.GetStat(Stat.Cooldown);

        yield return new WaitForSeconds(cooldownTimer);

        characterAbilityIcon.GetComponent<Image>().color = new Color(1,1,1,0.75f);
        abilityState = AbilityState.Ready;
    }
}
