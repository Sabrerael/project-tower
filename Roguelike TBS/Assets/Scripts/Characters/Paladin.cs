using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public enum AbilityState {
    Ready,
    Active,
    Cooldown
}

public class Paladin : Character {
    [SerializeField] LevelUpBonuses levelUpBonuses = null;
    [SerializeField] TextMeshProUGUI[] bonusesMenuTextFields = null;
    [SerializeField] LevelUpBonusMenu levelUpMenu = null;
    [SerializeField] int abilityModifyPercent = 30;
    [SerializeField] float abilityTimer = 30;
    [SerializeField] GameObject abilityParticles = null;

    private int[] choiceIndexes = new int[3];
    private int activeAbilityModifier = 0;
    private AbilityState abilityState = AbilityState.Ready;
    private LevelUpBonus[] randomBonuses = new LevelUpBonus[3];

    private void Update() {
        if (abilityState == AbilityState.Ready) {
            if (Input.GetKeyDown(KeyCode.Q)) {
                ActiveAbility();
            }
        }
    }

    public override void ActiveAbility() {
        abilityState = AbilityState.Active;
        activeAbilityModifier = abilityModifyPercent;
        if (abilityParticles != null) { 
            Instantiate(abilityParticles, transform);
        }
        StartCoroutine(StatTimer(abilityTimer, abilityModifyPercent));
    }

    public override void ChooseLevelUpModifier() {
        if (baseStats.GetLevel() % 2 == 1) { return; }

        levelOfBonuses = (baseStats.GetLevel() / 2) - 1;

        randomBonuses = levelUpBonuses.GetLevelUpBonusesByLevel(levelOfBonuses);
        
        Time.timeScale = 0;

        levelUpMenu.ToggleBodyActive();
        for(int i = 0; i < 3; i++) {
            var index = UnityEngine.Random.Range(0,5);
            choiceIndexes[i] = index;

            if (randomBonuses[index].type == "Multiplicative") {
                bonusesMenuTextFields[i].text = "Increases your " + randomBonuses[index].stat + " by " + randomBonuses[index].bonus + "%";
            } else {
                bonusesMenuTextFields[i].text = "Increases your " + randomBonuses[index].stat + " by " + randomBonuses[index].bonus + " points";
            }
        }
    }

    public void ChooseBonusOne() {
        AddLevelUpModifier(randomBonuses[choiceIndexes[0]]);
        levelUpMenu.ToggleBodyActive();
        Time.timeScale = 1;
    }

    public void ChooseBonusTwo() {
        AddLevelUpModifier(randomBonuses[choiceIndexes[1]]);
        levelUpMenu.ToggleBodyActive();
        Time.timeScale = 1;
    }

    public void ChooseBonusThree() {
        AddLevelUpModifier(randomBonuses[choiceIndexes[2]]);
        levelUpMenu.ToggleBodyActive();
        Time.timeScale = 1;
    }
    
    public override IEnumerable<int> GetMultiplicativeModifiers(Stat stat) {
        // TODO add the rest of the stats
        var bonusValue = 0;

        if (stat == Stat.Attack) {
            foreach(LevelUpBonus bonus in bonuses) {
                if (bonus.type == "Multiplicative" && bonus.stat == Stat.Attack) {
                    bonusValue += bonus.bonus;
                }
            }

            yield return bonusValue;
        } else if (stat == Stat.Defense) {
            foreach(LevelUpBonus bonus in bonuses) {
                if (bonus.type == "Multiplicative" && bonus.stat == Stat.Defense) {
                    bonusValue += bonus.bonus;
                }
            }

            bonusValue += activeAbilityModifier;
            yield return bonusValue;
        } else if (stat == Stat.Health) {
            foreach(LevelUpBonus bonus in bonuses) {
                if (bonus.type == "Multiplicative" && bonus.stat == Stat.Health) {
                    bonusValue += bonus.bonus;
                }
            }

            yield return bonusValue;
        }

        yield return 0;
    }

    private IEnumerator StatTimer(float time, int percent) {
        characterAbilityIcon.GetComponent<Image>().color = new Color(0.5f,0.5f,0.5f,0.25f);
        // Trigger particle effect
        Debug.Log("Ability Triggered");
        yield return new WaitForSeconds(time);

        Debug.Log("Ability Over");
        RemoveAbilityModifier(percent);

        abilityState = AbilityState.Cooldown;
        cooldownTimer = baseStats.GetStat(Stat.Cooldown);

        yield return new WaitForSeconds(cooldownTimer);

        Debug.Log("Cooldown Over");
        characterAbilityIcon.GetComponent<Image>().color = new Color(1,1,1,0.75f);
        abilityState = AbilityState.Ready;
    }

    public void RemoveAbilityModifier(int percent) {
        activeAbilityModifier = 0;
    }
}
