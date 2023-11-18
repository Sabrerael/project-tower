using System;
using System.Collections;
using UnityEngine;

namespace RPG.Stats {
    public class Health : MonoBehaviour {
        [SerializeField] int healthPoints = 1;
        [SerializeField] Sprite deadSprite = null;
        
        private bool isDead = false;
        private LevelLoader levelLoader = null;
        Animator animator;
        BaseStats baseStats;
        SpriteRenderer spriteRenderer;

        public event Func<int, int> onDamageTaken;
        public event Action onDeath;

        private void Start() {
            healthPoints = GetComponent<BaseStats>().GetStat(Stat.Health);
            levelLoader = GameObject.Find("Level Loader").GetComponent<LevelLoader>();
            animator = GetComponent<Animator>();
            baseStats = GetComponent<BaseStats>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public bool IsDead() { return isDead; }

        public void TakeDamage(GameObject instigator, int damage) {
            if (isDead) { return; }

            if (Mathf.Sign(damage) == -1 || damage == 0) {
                damage = 1;
            }

            if (onDamageTaken != null) {
                damage = onDamageTaken(damage);
            }
            
            healthPoints = Mathf.Max(healthPoints - damage, 0);

            if (healthPoints == 0) {
                Die();
                AwardExperience(instigator);
            } else {
                StartCoroutine(DamageEffect());
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
            if (gameObject.CompareTag("Player")) {
                StartCoroutine(PlayerDeath());
                return;
            }

            if (isDead) { return; }

            isDead = true;
            animator.SetBool("Dead", true);
            foreach (var collider in gameObject.GetComponents<Collider2D>()) {
                collider.enabled = false;
            }
            spriteRenderer.sortingOrder = -1;
            transform.SetParent(GameObject.Find("Dead Enemies").transform);
            
            if (onDeath != null) {
                onDeath();
            }
        }

        private void AwardExperience(GameObject instigator) {
            if (instigator == gameObject) { return; }

            Experience experience = instigator.GetComponent<Experience>();
            if (experience == null) { return; }
            
            experience.GainExperience(baseStats.GetStat(Stat.ExperienceReward));
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

        private IEnumerator DamageEffect() {
            //TODO Make the color transition smoother -- Maybe this just goes in Update?
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(.25f);
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(.25f);
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(.25f);
            spriteRenderer.color = Color.white;
        }

        private IEnumerator PlayerDeath() {
            isDead = true;
            animator.SetBool("IsDead", true);
            yield return new WaitForSeconds(2.5f);
            levelLoader.LoadGameOver();
        }
    }
}