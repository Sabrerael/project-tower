using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Inventory/Drop Library"))]
public class DropLibrary : ScriptableObject {
    [SerializeField] DropConfig[] potentialDrops;
    [SerializeField] float dropChancePercentage; // Future work: turn this into an array to represent different rarities of drops
    //[SerializeField] List<InventoryItem> droppedWeapons = new List<InventoryItem>(); 

    [System.Serializable]
    class DropConfig {
        public InventoryItem item;
        public Pickup itemDropped;
        public float relativeChance; // Future work as above
        public int minNumber; // Future work as above
        public int maxNumber; // Future work as above

        public int GetRandomNumber() {
            if (!item.IsStackable()) {
                return 1;
            }

            return UnityEngine.Random.Range(minNumber, maxNumber+1);
        }
    }

    public struct Dropped {
        public InventoryItem item;
        public Pickup itemDropped;
        public int number;
    }

    public IEnumerable<Dropped> GetRandomDrops() {
        if (!ShouldRandomDrop()) {
            yield break;
        }

        yield return GetRandomDrop();
    }

    private bool ShouldRandomDrop() {
        return UnityEngine.Random.Range(0,100) < dropChancePercentage;
    }

    private Dropped GetRandomDrop() {
        DropConfig drop = SelectRandomItem();
        Dropped result = new Dropped();
        result.item = drop.item;
        result.itemDropped = drop.itemDropped;
        result.number = drop.GetRandomNumber();
        result.itemDropped.SetNumber(result.number);

        if (result.item is WeaponConfig) {
            //droppedWeapons.Add(result.item);
        }

        return result;
    }

    private DropConfig SelectRandomItem() {
        float totalChance = GetTotalChance();
        float randomRoll = UnityEngine.Random.Range(0, totalChance);
        float chanceTotal = 0;
        foreach (var drop in potentialDrops) {
            chanceTotal += drop.relativeChance;

            if (chanceTotal > randomRoll) {
                return drop;
            }
        }
        return null;
    }

    private float GetTotalChance() {
        float chanceTotal = 0;
        foreach (var drop in potentialDrops) {
            chanceTotal += drop.relativeChance;
        } 

        return chanceTotal;
    }
}
