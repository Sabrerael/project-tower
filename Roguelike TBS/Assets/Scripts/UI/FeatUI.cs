using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeatUI : MonoBehaviour {
    // CONFIG DATA
    [SerializeField] FeatSlotUI FeatTextPrefab = null;

    // CACHE
    Character player;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
        player.onFeatAdded += Redraw;

        Redraw();
    }

    // PRIVATE

    private void Redraw() {
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }

        var feats = player.GetSelectedAbilities();

        for (int i = 0; i < feats.Count; i++) {
            var featUI = Instantiate(FeatTextPrefab, transform);
            featUI.Setup(feats[i], i);
        }
    }   
}
