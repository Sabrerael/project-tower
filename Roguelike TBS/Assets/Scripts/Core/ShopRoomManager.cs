using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopRoomManager : RoomManager {
    [SerializeField] ShopTable[] shopTables = null;
    [SerializeField] InventoryItem[] itemsForSale = null;

    public void SetShopTables() {
        for (int i = 0; i < shopTables.Length; i++) {
            shopTables[i].SetItemForSale(itemsForSale[i]);
        }
    }
}
