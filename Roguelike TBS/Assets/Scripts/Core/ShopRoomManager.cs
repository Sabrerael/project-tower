using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopRoomManager : RoomManager {
    [SerializeField] ShopTable[] shopTables = null;
    [SerializeField] ShopLibrary itemsForSale = null;

    public void SetShopTables() {
        for (int i = 0; i < shopTables.Length; i++) {
            if (i == 0) {
                var result = itemsForSale.GetHealthPotions();
                shopTables[i].SetItemForSale(result.item, result.number);
            } else {
                var result = itemsForSale.GetRandomItem();
                shopTables[i].SetItemForSale(result.item, result.number);
            }
        }
    }
}
