using TMPro;
using UnityEngine;

public class FeatSlotUI : MonoBehaviour {
    [SerializeField] TextMeshProUGUI description = null;
    [SerializeField] int index = 0;

    // STATE
    Feat feat;

    // PUBLIC

    public void Setup(Feat feat, int index) {
        this.feat = feat;
        this.index = index;
        description.text = this.feat.GetAbilityName() + ": " + this.feat.GetAbilityDescription();
    }
}
