using System.Collections.Generic;
using RPG.Stats;
using UnityEngine;

public abstract class Character : MonoBehaviour, IModifierProvider {
    public static Character instance = null;

    protected List<LevelUpBonus> bonuses = new List<LevelUpBonus>();
    protected BaseStats baseStats;
    protected int levelOfBonuses = 0;
    protected float cooldownTimer = 0;
    protected RoomManager currentRoom = null;
    protected GameObject characterAbilityIcon;

    private void Awake() {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        baseStats = GetComponent<BaseStats>();
        if (baseStats != null) {
            baseStats.onLevelUp += ChooseLevelUpModifier;
        }
        characterAbilityIcon = GameObject.Find("Character Ability Icon");
    }

    public virtual void ActiveAbility() {
        // Overrided in the specific character classes
    }

    public virtual void ChooseLevelUpModifier() {
        // Overrided in the specific character classes
    }

    public void AddLevelUpModifier(LevelUpBonus bonus) {
        Debug.Log("AddLevelUpModifier called");
        bonuses.Add(bonus);
    }

    public IEnumerable<int> GetAdditiveModifiers(Stat stat) {
        int bonusValue = 0;

        if (stat == Stat.Attack) {
            foreach(LevelUpBonus bonus in bonuses) {
                if (bonus.type == "Additive" && bonus.stat == Stat.Attack) {
                    bonusValue += bonus.bonus;
                }
            }

            yield return bonusValue;
        } else if (stat == Stat.Defense) {
            foreach(LevelUpBonus bonus in bonuses) {
                if (bonus.type == "Additive" && bonus.stat == Stat.Defense) {
                    bonusValue += bonus.bonus;
                }
            }

            yield return bonusValue;
        } else if (stat == Stat.Health) {
            foreach(LevelUpBonus bonus in bonuses) {
                if (bonus.type == "Additive" && bonus.stat == Stat.Health) {
                    bonusValue += bonus.bonus;
                }
            }

            yield return bonusValue;
        }
    }

    public virtual IEnumerable<int> GetMultiplicativeModifiers(Stat stat) {
        // Overrided in character class
        foreach(LevelUpBonus bonus in bonuses) {
            if (bonus.type == "Multiplicative" && bonus.stat == Stat.Attack && stat == Stat.Attack) {
                yield return bonus.bonus;
            } else if (bonus.type == "Multiplicative" && bonus.stat == Stat.Defense && stat == Stat.Defense) {
                yield return bonus.bonus;
            } else if (bonus.type == "Multiplicative" && bonus.stat == Stat.Health && stat == Stat.Health) {
                yield return bonus.bonus;
            }
        }
    }

    public void SetCurrentRoom(RoomManager roomManager) { currentRoom = roomManager; }
    public RoomManager GetCurrentRoom() { return currentRoom; }
}
