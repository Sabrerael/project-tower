using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Barbarian : Character {
    [Header("Barbarian Active Ability Modifiers")]
    [SerializeField] int abilityModifyPercent = 30;
    [SerializeField] float abilityTime = 30;

    private float extendedAbilityTime = 0;

    private event Action<GameObject> onAbilityActivate;
    public event Action<GameObject> onAbilityKill;
    public override event Action onFeatAdded;

    private void Start() {
        if (activeAbilityIcon) {
            var weaponAbilityIcon = GameObject.Find("Character Ability Icon");
            weaponAbilityIcon.GetComponent<Image>().sprite = activeAbilityIcon;
            weaponAbilityIcon.GetComponent<Image>().color = new Color(1,1,1,0.75f);
        }

        levelUpMenu = GameObject.Find("Level Up Bonuses Menu").GetComponent<LevelUpBonusMenu>();

        foreach(Stat stat in Enum.GetValues(typeof(Stat))) {
            activeAbilityModifyPercentages[stat] = 0;
        }

        activeAbilityModifyPercentages[Stat.Attack] = abilityModifyPercent;
    }

    public override void ActiveAbility() {
        if (abilityState != AbilityState.Ready) { return; }

        GetComponent<Animator>().SetTrigger("Rage");
        abilityState = AbilityState.Active;
        if (abilityParticles != null) { 
            Instantiate(abilityParticles, transform);
        }
        if (onAbilityActivate != null) {
            onAbilityActivate(gameObject);
        }
        abilityCoroutine = AbilityTimer(abilityTime);
        StartCoroutine(abilityCoroutine);
    }

    public override void CallOnAbilityKill() {
        if (abilityState == AbilityState.Active && onAbilityKill != null) {
            onAbilityKill(gameObject);
        }
    }

    public override void AddTimeToAbility(float time) {
        extendedAbilityTime += time;
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
        } else if (ability.GetActivationPoint() == ActivationPoint.OnHealthCondition) {
            // TODO keeping for a possible use later    
        }

        if (onFeatAdded != null) {
            onFeatAdded();
        }
    }

    private IEnumerator AbilityTimer(float time) {
        characterAbilityIcon.GetComponent<Image>().color = new Color(0.5f,0.5f,0.5f,0.25f);

        yield return new WaitForSeconds(time);

        // TODO This only counts any accumulated time during the initial time. Need to modify to make sure it can be extended as long as the ability is active
        if (extendedAbilityTime != 0) {
            yield return new WaitForSeconds(extendedAbilityTime);
        }

        abilityState = AbilityState.Cooldown;
        cooldownTimer = baseStats.GetStat(Stat.Cooldown);
        extendedAbilityTime = 0;

        yield return new WaitForSeconds(cooldownTimer);

        characterAbilityIcon.GetComponent<Image>().color = new Color(1,1,1,0.75f);
        abilityState = AbilityState.Ready;
    }
}
