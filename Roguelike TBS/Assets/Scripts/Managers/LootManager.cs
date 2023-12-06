using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : MonoBehaviour {
    public static LootManager instance = null;

    [SerializeField] int cTierCeiling = 50;
    [SerializeField] int bTierCeiling = 85;
    [SerializeField] List<InventoryItem> cTierItems;
    [SerializeField] List<InventoryItem> bTierItems;
    [SerializeField] List<InventoryItem> aTierItems;

    private List<InventoryItem> selectableCTierItems;
    private List<InventoryItem> selectableBTierItems;
    private List<InventoryItem> selectableATierItems;

    private void Awake() {
        if (instance == null) {
            instance = this;
            SetUpSelectableLists();
        } else if (instance != this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public List<InventoryItem> GetRandomLootItems(int numOfItems) {
        List<InventoryItem> selectedItems = new List<InventoryItem>();

        for (int i = 0; i < numOfItems; i++) {
            selectedItems.Add(GetRandomLootItem());
        }

        return selectedItems;
    }

    private InventoryItem GetRandomLootItem() {
        int lootIndex = Random.Range(1, 101);
        InventoryItem selectedItem = null;

        if (lootIndex <= cTierCeiling) {
            int cTierIndex = Random.Range(0, selectableCTierItems.Count);
            selectedItem = selectableCTierItems[cTierIndex];
            selectableCTierItems.Remove(selectedItem);
            cTierCeiling -= 5;
        } else if (cTierCeiling < lootIndex && lootIndex <= bTierCeiling) {
            int bTierIndex = Random.Range(0, selectableBTierItems.Count);
            selectedItem = selectableBTierItems[bTierIndex];
            selectableBTierItems.Remove(selectedItem);
            bTierCeiling -= 5;
        } else if (bTierCeiling < lootIndex) {
            int aTierIndex = Random.Range(0, selectableATierItems.Count);
            selectedItem = selectableATierItems[aTierIndex];
            selectableATierItems.Remove(selectedItem);
        }

        return selectedItem;
    }

    private void SetUpSelectableLists() {
        selectableCTierItems = cTierItems;
        selectableBTierItems = bTierItems;
        selectableATierItems = aTierItems;
    }

    // TODO Remove if not necessary
    public struct Dropped {
        public InventoryItem item;
        public Pickup itemDropped;
        public int number;
    }
}
