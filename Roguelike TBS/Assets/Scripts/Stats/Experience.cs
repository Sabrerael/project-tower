using System;
using UnityEngine;
//using GameDevTV.Saving;

namespace RPG.Stats {
    public class Experience : MonoBehaviour {
        [SerializeField] float experiencePoints = 0;

        public event Action onExperienceGained;

        public void GainExperience(float experience) {
            experiencePoints += experience;
            onExperienceGained();
            CallOnAbilityKill();
        }

        public float GetPoints() {
            return experiencePoints;
        }

        public object CaptureState() {
            return experiencePoints;
        }

        public void RestoreState(object state) {
            experiencePoints = (float)state;
        }

        private void CallOnAbilityKill() {
            if (GetComponent<Character>()) {
                GetComponent<Character>().CallOnAbilityKill();
            }
        }
    }
}
