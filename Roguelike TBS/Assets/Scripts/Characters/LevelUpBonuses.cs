using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum BonusType {
    Additive,
    Multiplicative
}

[System.Serializable]
public struct LevelUpBonus {
    public string type;
    public int bonus;
    public Stat stat;
}


[CreateAssetMenu(menuName = ("Stats/New Level Up Bonus"))]
public class LevelUpBonuses : ScriptableObject {
    [Tooltip("An array of arrays containing the Level Up Bonuses")]
    [SerializeField] BonusesAtLevel[] bonusesAtLevels = null;

    [System.Serializable]
    public struct BonusesAtLevel {
        public ClassAbility[] bonuses;
    }

    public ClassAbility[] GetLevelUpBonusesByLevel(int level) {
        return bonusesAtLevels[level].bonuses;
    }
}
