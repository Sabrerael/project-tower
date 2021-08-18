using System.Collections;
using RPG.Stats;
using UnityEngine;

[CreateAssetMenu(fileName = "Regen", menuName = "Class Ability/Regen")]
public class Regen : ClassAbility {
    [SerializeField] int regenAmount = 1;
    [SerializeField] float regenFrameCount = 60;

    // PUBLIC

    /// <summary>
    /// Trigger the use of this ability. Override to provide functionality.
    /// </summary>
    /// <param name="user">The character that is using this action.</param>
    public override void Use(GameObject user) {
        int i = 0;
        while (true) {
            i++;

            if (i >= regenFrameCount) {
                GameObject.FindGameObjectWithTag("Player").GetComponent<Health>().Heal(regenAmount);
                i = 0;
            }
        }
    }   

}
