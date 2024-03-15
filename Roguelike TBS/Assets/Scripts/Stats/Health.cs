using System;
using System.Collections;
using UnityEngine;

namespace RPG.Stats {
    public class Health : MonoBehaviour {
        [SerializeField] int healthPoints = 1;
        [SerializeField] Material whiteFlashMat;
        [SerializeField] float restoreDefaultMaterialTime = 0.2f;
        [SerializeField] GameObject deadObject;
        
        private bool isDead = false;
        private bool shieldUp = false;
        private int temporaryHealthPoints = 0;
        private Transform deadEnemyParent;
        private LevelLoader levelLoader = null;
        private Material defaultMaterial;
        Animator animator;
        BaseStats baseStats;
        SpriteRenderer spriteRenderer;

        public event Func<int, int> onDamageTaken;
        public event Action onTemporaryHealthGone;
        public event Action onDeath;

        private void Start() {
            healthPoints = GetComponent<BaseStats>().GetStat(Stat.Health);
            levelLoader = GameObject.Find("Level Loader").GetComponent<LevelLoader>();
            animator = GetComponent<Animator>();
            baseStats = GetComponent<BaseStats>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            deadEnemyParent = GameObject.Find("Dead Enemies").transform;
            defaultMaterial = spriteRenderer.material;
        }

        public bool IsDead() { return isDead; }

        public void TakeDamage(GameObject instigator, int damage) {
            if (isDead) { return; }

            if (shieldUp) {
                shieldUp = false;
                return;
            }

            if (Mathf.Sign(damage) == -1 || damage == 0) {
                damage = 1;
            }

            if (onDamageTaken != null) {
                damage = onDamageTaken(damage);
            }
            
            if (temporaryHealthPoints >= 0) {
                int temporaryHealthDamage = Mathf.Min(damage, temporaryHealthPoints);
                temporaryHealthPoints = temporaryHealthPoints - temporaryHealthDamage;
                damage -= temporaryHealthDamage;

                if (temporaryHealthPoints == 0 && onTemporaryHealthGone != null) {
                    onTemporaryHealthGone();
                }
            }

            if (damage != 0) {
                healthPoints = Mathf.Max(healthPoints - damage, 0);
            }

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

        public int GetHealthPoints() { return healthPoints + temporaryHealthPoints; }

        public int GetMaxHealthPoints() {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public float GetFraction() {
            return (float)(healthPoints + temporaryHealthPoints) / (float)GetMaxHealthPoints();
        }

        public bool IsAtMaxHealth() {
            return GetHealthPoints() >= GetMaxHealthPoints();
        }

        public void SetShieldUp() {
            shieldUp = true;
        }

        public void SetTemporaryHealth(int value) {
            temporaryHealthPoints = value;
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
            Instantiate(deadObject, transform.position, Quaternion.identity, deadEnemyParent);
            
            if (onDeath != null) {
                onDeath();
            }
            Destroy(gameObject);
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
            spriteRenderer.material = whiteFlashMat;
            yield return new WaitForSeconds(restoreDefaultMaterialTime);
            spriteRenderer.material = defaultMaterial;
        }

        private IEnumerator PlayerDeath() {
            isDead = true;
            animator.SetBool("IsDead", true);
            yield return new WaitForSeconds(2.5f);
            levelLoader.LoadGameOver();
        }
    }
}