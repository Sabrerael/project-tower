using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wallet Multipler Effect", menuName = "Abilities/Effects/Wallet Multipler")]
public class WalletMultiplerEffect : EffectStrategy {
    [SerializeField] float multiplier = 1.5f;

    public override void StartEffect(AbilityData data, Action finished) {
        Purse purse = data.GetUser().GetComponent<Purse>();

        if (purse) {
            purse.SetMoneyMultiplier(multiplier);
        }
        
        finished();
    }
}
