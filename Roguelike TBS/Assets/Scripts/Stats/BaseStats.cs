﻿using System;
using UnityEngine;

namespace RPG.Stats{
    public class BaseStats : MonoBehaviour {
        [Range(1, 99)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;
        [SerializeField] GameObject levelUpParticleEffect = null;
        [SerializeField] bool shouldUseModifiers = false;

        public event Action onLevelUp;

        private int currentLevel;

        Experience experience;

        private void Awake() {
            experience = GetComponent<Experience>();
            currentLevel = CalculateLevel();
        }

        private void Start() { }

        private void OnEnable() {
            if (experience != null)
            {
                experience.onExperienceGained += UpdateLevel;
            }
        }

        private void OnDisable() {
            if (experience != null)
            {
                experience.onExperienceGained -= UpdateLevel;
            }
        }

        private void UpdateLevel() {
            int newLevel = CalculateLevel();
            if (newLevel > currentLevel) {
                currentLevel = newLevel;
                LevelUpEffect();
                onLevelUp();
            }
        }

        private void LevelUpEffect() {
            if (levelUpParticleEffect == null) { return; }

            Instantiate(levelUpParticleEffect, transform);
        }

        public int GetStat(Stat stat) {
            return Mathf.CeilToInt((float)GetBaseStat(stat) * (float)(1f + (float)GetPercentageModifier(stat)/100)) + GetAdditiveModifier(stat);
        }

        private int GetBaseStat(Stat stat) {
            return progression.GetStat(stat, characterClass, GetLevel());
        }

        public int GetLevel() {
            return currentLevel;
        }

        // This is for setting the level of enemies
        public void SetLevel(int level) {
            currentLevel = level;
        }

        private int GetAdditiveModifier(Stat stat) {
            if (!shouldUseModifiers) return 0;

            int total = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (int modifier in provider.GetAdditiveModifiers(stat))
                {
                    total += modifier;
                }
            }
            return total;
        }

        private int GetPercentageModifier(Stat stat) {
            if (!shouldUseModifiers) return 0;

            int total = 0;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (int modifier in provider.GetMultiplicativeModifiers(stat))
                {
                    total += modifier;
                }
            }
            return total;
        }

        private int CalculateLevel()
        {
            Experience experience = GetComponent<Experience>();
            if (experience == null) return startingLevel;

            float currentXP = experience.GetPoints();
            int penultimateLevel = progression.GetLevels(Stat.ExperienceToLevelUp, characterClass);
            for (int level = 1; level <= penultimateLevel; level++)
            {
                float XPToLevelUp = progression.GetStat(Stat.ExperienceToLevelUp, characterClass, level);
                if (XPToLevelUp > currentXP)
                {
                    return level;
                }
            }

            return penultimateLevel + 1;
        }
    }
}