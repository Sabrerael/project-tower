using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class ShopTable : MonoBehaviour {
    [SerializeField] InventoryItem itemForSale = null;
    [SerializeField] int numberOfItems = 1;
    [SerializeField] TextMeshProUGUI priceLabel = null;
    [SerializeField] TextMeshProUGUI quantityLabel = null;
    [SerializeField] Image itemSprite = null;
    [SerializeField] Sprite soldOutSign = null;

    private bool canBuy = false;

    private void Start() {
        if (itemForSale) {
            itemSprite.sprite = itemForSale.GetIcon();
            SetPriceLabel();
            SetQuantityLabel();
        }
    }

    private void OnAttack(InputValue value) {
        if (value.isPressed && canBuy) {
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

    public void SetItemForSale(InventoryItem item, int quantity) { 
        itemForSale = item;
        numberOfItems = quantity;
        SetPriceLabel();
        SetQuantityLabel();
        SetItemSprite();
    } 

    private void GiveItemToPlayer(GameObject player) {
        if (itemForSale as WeaponConfig) {
            player.GetComponent<Inventory>().AddToFirstEmptyWeaponSlot(itemForSale as WeaponConfig);
            player.GetComponent<Fighter>().EquipWeapon(itemForSale as WeaponConfig);
        } else if (itemForSale as PassiveItem) {
            player.GetComponent<Inventory>().AddToFirstEmptyPassiveItemSlot(itemForSale as PassiveItem);
        } else if (itemForSale as ActionItem) {
            player.GetComponent<Inventory>().AddToFirstEmptyActionItemSlot(itemForSale as ActionItem, 1);
        }
        numberOfItems--;
        SetQuantityLabel();
        if (numberOfItems == 0) {
            SetSoldOut();
        }
    }

    private void PurchaseItem(GameObject player) {
        player.GetComponent<Purse>().UpdateBalance(-1*itemForSale.GetShopPrice());
        GiveItemToPlayer(player);
    }

    private bool PurchaseConditions(GameObject player) {
        if (itemForSale == null || !player.GetComponent<Purse>()) { return false; }
        bool hasEnoughMoney = player.GetComponent<Purse>().GetBalance() >= itemForSale.GetShopPrice();
        bool hasSpaceForItem = false;

        if (itemForSale as WeaponConfig) {
            hasSpaceForItem = player.GetComponent<Inventory>().HasSpaceForWeapon(itemForSale as WeaponConfig);
        } else if (itemForSale as ActionItem) {
            hasSpaceForItem = player.GetComponent<Inventory>().HasSpaceForActionItem(itemForSale as ActionItem);
        } else if (itemForSale as PassiveItem) {
            hasSpaceForItem = true;
        }

        return hasEnoughMoney && hasSpaceForItem;
    }

    private void SetItemSprite() {
        itemSprite.sprite = itemForSale.GetIcon();
    }

    private void SetPriceLabel() {
        priceLabel.text = itemForSale.GetShopPrice().ToString();
    }

    private void SetQuantityLabel() {
        if (numberOfItems <= 1) {
            quantityLabel.gameObject.SetActive(false);
        } else {
            quantityLabel.gameObject.SetActive(true);
            quantityLabel.text = "X" + numberOfItems.ToString();
        }
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
