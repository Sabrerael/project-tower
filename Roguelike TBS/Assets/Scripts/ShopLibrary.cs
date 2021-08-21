using UnityEngine;

[CreateAssetMenu(menuName = ("Shop/Shop Library"))]
public class ShopLibrary : ScriptableObject {
    [SerializeField] HealthPotion healthPotion = null;
    [SerializeField] int maxNumHealthPotions = 3;
    [SerializeField] ShopItemConfig[] possibleItemsForSale = null;

    [System.Serializable]
    class ShopItemConfig {
        public InventoryItem item;
        public int minNumber; // Future work as above
        public int maxNumber; // Future work as above

        public int GetRandomNumber() {
            if (!item.IsStackable()) {
                return 1;
            }

            return UnityEngine.Random.Range(minNumber, maxNumber+1);
        }
    }

    public struct ShopItem {
        public InventoryItem item;
        public int number;
    }

    public ShopItem GetHealthPotions() {
        ShopItem healthPotions = new ShopItem();
        healthPotions.item = healthPotion;
        healthPotions.number = UnityEngine.Random.Range(1, maxNumHealthPotions+1);

        return healthPotions;
    }

    public ShopItem GetRandomItem() {
        ShopItemConfig item = SelectRandomItem();
        ShopItem result = new ShopItem();
        result.item = item.item;
        result.number = item.GetRandomNumber();

        if (result.item is WeaponConfig) {
            //droppedWeapons.Add(result.item);
        }

        return result;
    }

    private ShopItemConfig SelectRandomItem() {
        int randomRoll = UnityEngine.Random.Range(0, possibleItemsForSale.Length);
        return possibleItemsForSale[randomRoll];
    }
       
}
