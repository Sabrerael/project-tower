using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Inventory/Drop Library"))]
public class DropLibrary : ScriptableObject {
    [SerializeField] DropConfig[] potentialDrops;
    [SerializeField] float dropChancePercentage; // Future work: turn this into an array to represent different rarities of drops

    [System.Serializable]
    class DropConfig {
        public InventoryItem item;
        public Pickup itemDropped;
        public float relativeChance; // Future work as above
        public int minNumber; // Future work as above
        public int maxNumber; // Future work as above

        public int GetRandomNumber() {
            //if (!item.IsStackable()) {
                return 1;
            //}

            //return UnityEngine.Random.Range(minNumber, maxNumber+1);
        }
    }

    public struct DroppedOld {
        public InventoryItem item;
        public Pickup itemDropped;
        public int number;
    }

    // These don't have to be IEnumerables
    public IEnumerable<DroppedOld> GetRandomDrops() {
        if (!ShouldRandomDrop()) {
            yield break;
        }

        var gameManager = GameManager.instance;
        var randomDrop = new DroppedOld();

        do {
            randomDrop = GetRandomDrop();
        } while (gameManager.ItemIsInKnockoutList(randomDrop.item));

        yield return randomDrop;
    }

    private bool ShouldRandomDrop() {
        return UnityEngine.Random.Range(0,100) < dropChancePercentage;
    }

    private DroppedOld GetRandomDrop() {
        DropConfig drop = SelectRandomItem();
        DroppedOld result = new DroppedOld();
        result.item = drop.item;
        result.itemDropped = drop.itemDropped;
        result.number = drop.GetRandomNumber();
        result.itemDropped.SetNumber(result.number);

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
