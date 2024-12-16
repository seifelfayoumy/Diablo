using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource musicSrc;
    public AudioSource SFXSrc;

    public AudioClip menuMusic;
    public AudioClip mainLevelMusic;
    public AudioClip bossLevelMusic;

    public AudioClip shieldActivateSFX;
    public AudioClip infernoSFX;
    public AudioClip cloneSFX;
    public AudioClip chargeDashSFX;
    public AudioClip arrowFiredSFX;
    public AudioClip explosionSFX;
    public AudioClip fireballSFX;
    public AudioClip pickupSFX;
    public AudioClip wandererDamageSFX;
    public AudioClip healingPotionSFX;
    public AudioClip wandererDiesSFX;
    public AudioClip enemyDiesSFX;
    public AudioClip summonSFX;
    public AudioClip bossStompsDownSFX;
    public AudioClip bossCastSpellSFX;
    public AudioClip bossSwingHandsSFX;
    public AudioClip bossHitSFX;
    public AudioClip bossDiesSFX;

    public void PlaySFX(AudioClip clip)
    {
        SFXSrc.PlayOneShot(clip);
    }
}
