using System.Collections;
using System.Collections.Generic;
using RPG.Stats;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour {
    public static HUDManager instance = null;

    [SerializeField] RectTransform healthForeground;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] RectTransform experienceForeground;
    [SerializeField] TextMeshProUGUI experienceText;
    [SerializeField] TextMeshProUGUI purseText;
    [SerializeField] Image weaponIcon;
    [SerializeField] Image activeItemIcon;
    [SerializeField] Image characterAbilityIcon;
    [SerializeField] ItemPopup itemPopup;
    [SerializeField] float itemPopupFadeTime = 1;
    [SerializeField] float itemPopupTime = 3;

    private Character character;
    private Health health;
    private BaseStats baseStats;
    private Experience experience;
    private Purse purse; 
    private Fighter fighter;
    private Inventory weaponInventory;
    private ActionItemInventory actionItemInventory;
    private PotionInventory potionInventory;

    private bool isItemPopUpAppearing = false;
    private bool isItemPopupDisappearing = false;

    private void Awake() {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Start() {
        character = Character.Instance;
        health = character?.GetComponent<Health>();
        baseStats = character?.GetComponent<BaseStats>();
        experience = character?.GetComponent<Experience>();
        purse = character?.GetComponent<Purse>();
        weaponInventory = character?.GetComponent<Inventory>();
        actionItemInventory = character?.GetComponent<ActionItemInventory>();
        potionInventory = character?.GetComponent<PotionInventory>();

        RedrawAll();
    }

    // Clean up, all this shouldn't be calculated every frame
    private void Update() {
        if (isItemPopUpAppearing) {
            var alphaChange = Time.deltaTime / itemPopupFadeTime;
            itemPopup.ChangeAlpha(alphaChange, true);

            if (itemPopup.GetAlphaValue() == 1) {
                isItemPopUpAppearing = false;
                StartCoroutine(ItemPopupTimer());
            }
        } else if (isItemPopupDisappearing) {
            var alphaChange = Time.deltaTime / itemPopupFadeTime;
            itemPopup.ChangeAlpha(alphaChange, false);

            if (itemPopup.GetAlphaValue() == 0) {
                isItemPopupDisappearing = false;
                ToggleItemPopup();
            }
        }
        RedrawAll();
    }

    private void RedrawAll() {
        RedrawHealth();
        RedrawExperience();
        RedrawAbilities();
        RedrawPurse();
    }

    private void RedrawHealth() {
        if (health == null) {
            healthText.text = "Dead";
            healthForeground.localScale = new Vector3(0, 1, 1);
            return;
        }

        healthText.text = health.GetHealthPoints().ToString() + "/" + health.GetMaxHealthPoints().ToString();
        healthForeground.localScale = new Vector3(health.GetFraction(), 1, 1);
    }

    private void RedrawExperience() {
        levelText.text = "Lv " + baseStats.GetLevel().ToString();
        experienceText.text = experience.GetPoints().ToString() + "/" + baseStats.GetPointsToLevelUp().ToString();
        experienceForeground.localScale = new Vector3(baseStats.GetExperienceFraction(), 1, 1);
    }

    private void RedrawAbilities() {
        weaponIcon.sprite = weaponInventory.GetEquipWeaponConfig().GetIcon();
        activeItemIcon.sprite = actionItemInventory.GetActiveActionItemSprite();
        characterAbilityIcon.sprite = character.GetActiveAbilityIcon();
    }

    private void RedrawPurse() {
        purseText.text = purse.GetBalance().ToString();
    }

    private void ToggleItemPopup() {
        itemPopup.gameObject.SetActive(!itemPopup.gameObject.activeInHierarchy);
    }

    private void SetItemPopupText(string textToSet) {
        itemPopup.GetComponentInChildren<TextMeshProUGUI>().text = textToSet;
    }

    private IEnumerator ItemPopupTimer() {
        yield return new WaitForSeconds(itemPopupTime);
        isItemPopupDisappearing = true;
    }

    public void LaunchItemPopup(string textToSet) {
        ToggleItemPopup();
        SetItemPopupText(textToSet);
        isItemPopUpAppearing = true;
    }
}
