using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Usable Item", menuName = "Usable Item", order = 0)]
public class ItemConfig : ScriptableObject// : EquipableItem, IModifierProvider
{
    [SerializeField] GameObject itemPrefab = null;
    [SerializeField] bool stackable = false;

    void UseItem() {
        //Using item code goes in here? Maybe?
    }
}
