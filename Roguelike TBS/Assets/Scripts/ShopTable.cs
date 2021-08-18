using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// TODO Check for conditions (enough money, space for item) and change label color to red in conditions fail
public class ShopTable : MonoBehaviour {
    [SerializeField] InventoryItem itemForSale = null;
    [SerializeField] TextMeshProUGUI priceLabel = null;
    [SerializeField] Image itemSprite = null;
    [SerializeField] Sprite soldOutSign = null;

    private bool canBuy = true;

    private void Awake() {
        if (itemSprite) {
            itemSprite.sprite = itemForSale.GetIcon();
        }

        if (itemForSale) {
            SetPriceLabel();
        }
    }

    private void Update() {
        if (canBuy && Input.GetKeyDown(KeyCode.Mouse0)) {
            PurchaseItem(GameObject.FindGameObjectWithTag("Player"));
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (itemForSale) {
            ToggleLabel();
        }

        if (!PurchaseConditions(other.gameObject)) {
            priceLabel.color = Color.red;
        } else {
            canBuy = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (itemForSale) {
            ToggleLabel();
            canBuy = false;
        }
    }

    public void SetItemForSale(InventoryItem item) { itemForSale = item ;} 

    private void GiveItemToPlayer(GameObject player) {
        if (itemForSale as WeaponConfig) {
            // TODO Weapon should be automatically equipped when purchased
            player.GetComponent<Inventory>().AddToFirstEmptyWeaponSlot(itemForSale as WeaponConfig);
        } else if (itemForSale as PassiveItem) {
            player.GetComponent<Inventory>().AddToFirstEmptyPassiveItemSlot(itemForSale as PassiveItem);
        } else if (itemForSale as ActionItem) {
            // TODO Add ability to buy multiple action items (potions, etc.)
            player.GetComponent<Inventory>().AddToFirstEmptyActionItemSlot(itemForSale as ActionItem, 1);
        }
    }

    private void PurchaseItem(GameObject player) {
        player.GetComponent<Purse>().UpdateBalance(-1*itemForSale.GetShopPrice());
        GiveItemToPlayer(player);
        SetSoldOut();
    }

    private bool PurchaseConditions(GameObject player) {
        bool hasEnoughMoney = player.GetComponent<Purse>().GetBalance() >= itemForSale.GetShopPrice();
        bool hasSpaceForItem = false;

        if (itemForSale as WeaponConfig) {
            // TODO Weapon should be automatically equipped when purchased
            hasSpaceForItem = player.GetComponent<Inventory>().HasSpaceForWeapon(itemForSale as WeaponConfig);
        } else if (itemForSale as ActionItem) {
            hasSpaceForItem = player.GetComponent<Inventory>().HasSpaceForActionItem(itemForSale as ActionItem);
        }

        return hasEnoughMoney && hasSpaceForItem;
    }

    private void SetPriceLabel() {
        priceLabel.text = itemForSale.GetShopPrice().ToString();
    }

    private void SetSoldOut() {
        itemSprite.sprite = soldOutSign;
        GetComponent<CircleCollider2D>().enabled = false;
        itemForSale = null;
    }

    private void ToggleLabel() {
        priceLabel.gameObject.SetActive(!priceLabel.gameObject.activeInHierarchy);
    }
}
