using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Shop/Shop Library"))]
public class ShopLibrary : ScriptableObject {
    [SerializeField] ActionItem healthPotion = null;
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

    // These don't have to be IEnumerables
    public IEnumerable<ShopItem> GetRandomItems() {
        var gameManager = GameManager.instance;
        var randomItem = new ShopItem();

        do {
            randomItem = GetRandomItem();
        } while (gameManager.ItemIsInKnockoutList(randomItem.item));

        yield return randomItem;
    }

    public ShopItem GetHealthPotions() {
        ShopItem healthPotions = new ShopItem();
        healthPotions.item = healthPotion;
        healthPotions.number = UnityEngine.Random.Range(1, maxNumHealthPotions+1);

        return healthPotions;
    }

    private ShopItem GetRandomItem() {
        ShopItemConfig item = SelectRandomItem();
        ShopItem result = new ShopItem();
        result.item = item.item;
        result.number = item.GetRandomNumber();

        return result;
    }

    private ShopItemConfig SelectRandomItem() {
        int randomRoll = UnityEngine.Random.Range(0, possibleItemsForSale.Length);
        return possibleItemsForSale[randomRoll];
    }
       
}
