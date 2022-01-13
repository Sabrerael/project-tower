using System.Collections;
using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour {
    public static HUD instance = null;

    [SerializeField] ItemPopup itemPopup = null;
    [SerializeField] float fadeTime = 1;
    [SerializeField] float popupTime = 3;

    private bool isAppearing = false;
    private bool isDisappearing = false;

    private void Awake() {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Update() {
        if (isAppearing) {
            var alphaChange = Time.deltaTime / fadeTime;
            itemPopup.ChangeAlpha(alphaChange, true);

            if (itemPopup.GetAlphaValue() == 1) {
                isAppearing = false;
                StartCoroutine(ItemPopupTimer());
            }
        } else if (isDisappearing) {
            var alphaChange = Time.deltaTime / fadeTime;
            itemPopup.ChangeAlpha(alphaChange, false);

            if (itemPopup.GetAlphaValue() == 0) {
                isDisappearing = false;
                ToggleItemPopup();
            }
        }
    }

    private void ToggleItemPopup() {
        itemPopup.gameObject.SetActive(!itemPopup.gameObject.activeInHierarchy);
    }

    private void SetItemPopupText(string textToSet) {
        itemPopup.GetComponentInChildren<TextMeshProUGUI>().text = textToSet;
    }

    private IEnumerator ItemPopupTimer() {
        yield return new WaitForSeconds(popupTime);
        isDisappearing = true;
    }

    public void LaunchItemPopup(string textToSet) {
        ToggleItemPopup();
        SetItemPopupText(textToSet);
        isAppearing = true;
    }
}
