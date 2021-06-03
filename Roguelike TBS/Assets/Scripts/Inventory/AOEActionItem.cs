using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Inventory/AOE Action Item"))]
public class AOEActionItem: ActionItem {
    [SerializeField] GameObject aoePrefab = null;
    [SerializeField] AudioClip sfx = null;

    // PUBLIC

    /// <summary>
    /// Trigger the use of this item.
    /// </summary>
    /// <param name="user">The character that is using this action.</param>
    public override void Use(GameObject user) {
        GameObject spawnedItem = Instantiate(aoePrefab, user.transform.position, Quaternion.identity);
        if (spawnedItem.GetComponent<AOE>()) {
            spawnedItem.GetComponent<AOE>().SetInstigator(user);
        } else if (spawnedItem.GetComponent<DrainingAOE>()) {
            spawnedItem.GetComponent<DrainingAOE>().SetInstigator(user);
        }
    }
}
