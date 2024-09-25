using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Weapon;

public class SoundManager : MonoBehaviour
{
    #region Singleton
    public static SoundManager Instance { get; set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    #endregion

    #region Variables
    [Header("AudioSources")]
    public AudioSource shootingChannel;
    public AudioSource reloadingChannel;
    public AudioSource emptyChannel;
    public AudioSource throwablesChannel;
    public AudioSource enemyChannel;
    public AudioSource enemyChannel2;

    [Header("Silenced AudioClips")]
    public AudioClip shootingSoundSilencedAR;
    public AudioClip shootingSoundSilencedSMG;
    public AudioClip shootingSoundSilencedShotgun;


    [Header("NonSilenced AudioClips")]
    public AudioClip shootingSoundAR;
    public AudioClip shootingSoundDeagle;
    public AudioClip shootingSoundGlockPistol;
    public AudioClip shootingSoundNonGlockPistol;
    public AudioClip shootingSoundShotgun;
    public AudioClip shootingSoundSMG;
    public AudioClip shootingSoundSniper;

    [Header("MiscellaneousWeaponSoundClips")]
    public AudioClip reloadSoundClip;
    public AudioClip emptySoundClip, fragmentationGrenadeSoundClip;

    [Header("EnemySoundClips")]
    public AudioClip enemyWalk;
    public AudioClip enemyChase, enemyAttack, enemyHurt, enemyDeath;
#endregion

    public void PlayShootingSound(WeaponModel weapon)
    {
        switch(weapon)
        {
            case WeaponModel.M16A4: 
                shootingChannel.PlayOneShot(shootingSoundSilencedAR);
                break;
            case WeaponModel.M1911: 
                shootingChannel.PlayOneShot(shootingSoundNonGlockPistol);
                break;

            default:
                break;
        }
    }

    public void PlayReloadingSound()
    {
        reloadingChannel.PlayOneShot(reloadSoundClip);
    }
    public void PlayEmptySound()
    {
        emptyChannel.PlayOneShot(emptySoundClip);
    }
}
