using System;
using UnityEngine;

namespace RPG.Stats {
    public class Health : MonoBehaviour {
        [SerializeField] int healthPoints = 1;
        [SerializeField] Sprite deadSprite = null;
        
        private bool isDead = false;
        private LevelLoader levelLoader = null;

        private void Start() {
            healthPoints = GetComponent<BaseStats>().GetStat(Stat.Health);
            levelLoader = GameObject.Find("Level Loader").GetComponent<LevelLoader>();
        }

        public bool IsDead() { return isDead; }

        public void TakeDamage(GameObject instigator, int damage) {
            if (isDead) { return; }
            
            healthPoints = Mathf.Max(healthPoints - damage, 0);

            if (healthPoints == 0) {
                Die();
                AwardExperience(instigator);
                return;
            }
        }

        public void Heal(int healthToRestore) {
            healthPoints = Mathf.Min(healthPoints + healthToRestore, GetMaxHealthPoints());
        }

        public int GetHealthPoints() { return healthPoints; }

        public int GetMaxHealthPoints() {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public float GetFraction() {
            return (float)healthPoints / (float)GetMaxHealthPoints();
        }

        public bool IsAtMaxHealth() {
            return GetHealthPoints() == GetMaxHealthPoints();
        }

        private void Die() {
            if (gameObject.tag == "Player") {
                // Go to Game Over screen
                levelLoader.LoadGameOver();
            }

            if (isDead) { return; }

            isDead = true;
            gameObject.GetComponent<Animator>().SetBool("Dead", true);
            gameObject.GetComponent<Collider2D>().enabled = false;
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = -1;
            transform.SetParent(GameObject.Find("Dead Enemies").transform);
        }

        private void AwardExperience(GameObject instigator) {
            if (instigator == gameObject) { return; }

            Experience experience = instigator.GetComponent<Experience>();
            if (experience == null) { return; }

            
            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }

        private void RegenerateHealth() {
            int regenHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public object CaptureState() {
            return healthPoints;
        }

        public void RestoreState(object state) {
            healthPoints = (int) state;

            if (healthPoints <= 0) {
                Die();
            }
        }
    }
}