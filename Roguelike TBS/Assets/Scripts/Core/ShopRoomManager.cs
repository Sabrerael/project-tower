using UnityEngine;

public class ShopRoomManager : RoomManager {
    [SerializeField] ShopTable[] shopTables = null;
    [SerializeField] ShopLibrary itemsForSale = null;

    private GameManager gameManager = null;

    private void Awake() {
        gameManager = GameManager.instance;
    }

    public void SetShopTables() {
        for (int i = 0; i < shopTables.Length; i++) {
            if (i == 0) {
                var healthPotions = itemsForSale.GetHealthPotions();
                shopTables[i].SetItemForSale(healthPotions.item, healthPotions.number);
            } else {
                var result = itemsForSale.GetRandomItems();
                // Technically don't need foreach loop
                foreach(var item in result) {
                    if (!item.item.IsStackable()) {
                        gameManager.AddItemToKnockoutList(item.item);
                    }
                    shopTables[i].SetItemForSale(item.item, item.number);
                }
            }
        }
    }
}
