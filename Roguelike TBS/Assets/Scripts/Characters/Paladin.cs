using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public enum AbilityState {
    Ready,
    Active,
    Cooldown
}

public class Paladin : Character {
    [SerializeField] LevelUpBonuses levelUpBonuses = null;
    [SerializeField] TextMeshProUGUI[] bonusesMenuTextFields = null;
    [SerializeField] GameObject levelUpMenu = null;
    [SerializeField] int abilityModifyPercent = 30;
    [SerializeField] float abilityTimer = 30;

    private int[] choiceIndexes = new int[3];
    private int activeAbilityModifier = 0;
    private float timer = 0;
    private AbilityState abilityState = AbilityState.Ready;
    private LevelUpBonus[] randomBonuses = new LevelUpBonus[3];

    private void Update() {
        if (abilityState == AbilityState.Cooldown) {
            timer += Time.deltaTime;
            
            if (timer > cooldownTimer) {
                abilityState = AbilityState.Ready;
                timer = 0;
            }
        } else if (abilityState == AbilityState.Ready) {
            if (Input.GetKeyDown(KeyCode.Q)) {
                ActiveAbility();
            }
        }
    }

    public override void ActiveAbility() {
        abilityState = AbilityState.Active;
        activeAbilityModifier = abilityModifyPercent;
        Debug.Log("Active Ability active");
        StartCoroutine(StatTimer(abilityTimer, abilityModifyPercent));
    }

    public override void ChooseLevelUpModifier() {
        if (baseStats.GetLevel() % 2 == 1) { return; }

        levelOfBonuses = (baseStats.GetLevel() / 2) - 1;

        randomBonuses = levelUpBonuses.GetLevelUpBonusesByLevel(levelOfBonuses);
        Debug.Log(levelUpMenu);
        
        Time.timeScale = 0;

        levelUpMenu.SetActive(true);
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
        levelUpMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void ChooseBonusTwo() {
        AddLevelUpModifier(randomBonuses[choiceIndexes[1]]);
        levelUpMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void ChooseBonusThree() {
        AddLevelUpModifier(randomBonuses[choiceIndexes[2]]);
        levelUpMenu.SetActive(false);
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
        yield return new WaitForSeconds(time);

        RemoveAbilityModifier(percent);
        Debug.Log("Active Ability over");
        abilityState = AbilityState.Cooldown;
        cooldownTimer = baseStats.GetStat(Stat.Cooldown);
    }

    public void RemoveAbilityModifier(int percent) {
        activeAbilityModifier = 0;
    }
}
