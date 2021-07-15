using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Inventory/Magic Spawner"))]
public class MagicSpawner : ActionItem {
    [SerializeField] AudioClip sfx = null;
    [SerializeField] GameObject magicToSpawn = null;

    /// <summary>
    /// Trigger the use of this item.
    /// </summary>
    /// <param name="user">The character that is using this action.</param>
    // TODO this needs a cooldown
    public override void Use(GameObject user) {
        if (user.GetComponent<Character>().GetCurrentRoom().GetEnemiesParent().transform.childCount == 0) {return;}
        var magic = Instantiate(magicToSpawn, user.transform.position, Quaternion.identity);

        magic.GetComponent<MagicMissile>().SetCaster(user);
    }
}
