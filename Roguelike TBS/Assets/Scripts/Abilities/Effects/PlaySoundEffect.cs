using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Play Sound Effect", menuName = "Abilities/Effects/Play Sound")]
public class PlaySoundEffect : EffectStrategy {
    [SerializeField] AudioClip sfx = null;

    public override void StartEffect(AbilityData data, Action finished) {
        AudioSource.PlayClipAtPoint(sfx, data.GetUser().transform.position);
        finished();
    }
}
