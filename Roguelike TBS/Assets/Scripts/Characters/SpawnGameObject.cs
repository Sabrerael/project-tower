using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnGameObject", menuName = "Class Ability/Spawn Game Object")]
public class SpawnGameObject : ClassAbility {
    [SerializeField] GameObject toInstantiate = null;

    // PUBLIC

    /// <summary>
    /// Trigger the use of this ability. Override to provide functionality.
    /// </summary>
    /// <param name="user">The character that is using this action.</param>
    public override void Use(GameObject user) {
        Instantiate(toInstantiate, user.transform.position, Quaternion.identity);
        // TODO Add Specific code for gameObjects to set Instigatior and such
    }
}
