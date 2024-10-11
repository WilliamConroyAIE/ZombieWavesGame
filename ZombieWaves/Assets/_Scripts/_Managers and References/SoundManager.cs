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
    public AudioSource playerChannel;

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

    #region NonGunAudio
    [Header("MiscellaneousWeaponSoundClips")]
    public AudioClip reloadSoundClip;
    public AudioClip emptySoundClip, fragmentationGrenadeSoundClip;

    [Header("EnemySoundClips")]
    public AudioClip enemyWalk;
    public AudioClip enemyChase, enemyAttack, enemyHurt, enemyDeath;

    [Header("PlayerSoundClips")]
    public AudioClip playerHurtClip;
    public AudioClip playerDeathClip, gameOverClip;
    #endregion

#endregion

    public void PlayShootingSound(WeaponModel weapon)
    {
        switch(weapon)
        {
            case WeaponModel.M16A4: 
                shootingChannel.PlayOneShot(shootingSoundSilencedAR);
                break;
            //
            case WeaponModel.MicroUzi: 
                shootingChannel.PlayOneShot(shootingSoundSMG);
                break;
            //
            case WeaponModel.M1Garand: 
                shootingChannel.PlayOneShot(shootingSoundSniper);
                break;
            //
            case WeaponModel.MG42: 
                shootingChannel.PlayOneShot(shootingSoundAR);
                break;
            //
            case WeaponModel.SA80: 
                shootingChannel.PlayOneShot(shootingSoundSilencedAR);
                break;
            //
            case WeaponModel.Famas: 
                shootingChannel.PlayOneShot(shootingSoundSilencedSMG);
                break;
            //
            case WeaponModel.MP5: 
                shootingChannel.PlayOneShot(shootingSoundSilencedSMG);
                break;
            //
            case WeaponModel.MP40: 
                shootingChannel.PlayOneShot(shootingSoundAR);
                break;
            //
            case WeaponModel.AK47: 
                shootingChannel.PlayOneShot(shootingSoundAR);
                break;
            //
            case WeaponModel.AK74U: 
                shootingChannel.PlayOneShot(shootingSoundSilencedSMG);
                break;
            //
            case WeaponModel.Dragunov: 
                shootingChannel.PlayOneShot(shootingSoundSniper);
                break;
            //
            case WeaponModel.PPSH: 
                shootingChannel.PlayOneShot(shootingSoundSMG);
                break;
            //
            case WeaponModel.Luger: 
                shootingChannel.PlayOneShot(shootingSoundNonGlockPistol);
                break;
            //
            case WeaponModel.DesertEagle: 
                shootingChannel.PlayOneShot(shootingSoundDeagle);
                break;
            //
            case WeaponModel.M9Beretta: 
                shootingChannel.PlayOneShot(shootingSoundGlockPistol);
                break;
            //
            case WeaponModel.Revolver: 
                shootingChannel.PlayOneShot(shootingSoundDeagle);
                break;
            //
            case WeaponModel.Glock17: 
                shootingChannel.PlayOneShot(shootingSoundGlockPistol);
                break;
            //
            case WeaponModel.Tokarev: 
                shootingChannel.PlayOneShot(shootingSoundNonGlockPistol);
                break;
            //
            case WeaponModel.Makarov: 
                shootingChannel.PlayOneShot(shootingSoundNonGlockPistol);
                break;
            //
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
