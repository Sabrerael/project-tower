using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemPopup : MonoBehaviour {
    [SerializeField] TextMeshProUGUI popupText = null;

    public void ChangeAlpha(float alphaChange, bool appearing) {
        var currentColor = GetComponent<Image>().color;

        if (appearing) {
            currentColor.a = Mathf.Min(currentColor.a + alphaChange, 1);
        } else {
            currentColor.a = Mathf.Max(currentColor.a - alphaChange, 0);
        }
        GetComponent<Image>().color = currentColor;

        var currentTextColor = popupText.color;

        if (appearing) {
            currentTextColor.a = Mathf.Min(currentTextColor.a + alphaChange, 1);
        } else {
            currentTextColor.a = Mathf.Max(currentTextColor.a - alphaChange, 0);
        }
        popupText.color = currentTextColor;
    }

    public float GetAlphaValue() { return GetComponent<Image>().color.a; }
}
