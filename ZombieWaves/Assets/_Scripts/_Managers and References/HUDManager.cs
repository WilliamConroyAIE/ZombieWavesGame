using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using static WeaponManager;
using static InteractionManager;

public class HUDManager : MonoBehaviour
{
    #region Singleton
    public static HUDManager Instance { get; set; }
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

    #region Values
    [Header("Ammunition")]
    public TextMeshProUGUI magazineAmmuntionUI;
    public TextMeshProUGUI totalAmmuntionUI;
    public Image ammuntionTypeUI;

    [Header("Weapon")]
    public Image activeWeaponUI;
    public Image unActiveWeaponUI;

    [Header("Throwables")]
    public Image lethalUI;
    public TextMeshProUGUI lethalAmountUI;
    public Image tacticalUI;
    public TextMeshProUGUI tacticalAmountUI;

    public Sprite emptySlot;
    public Sprite greySlot;

    public GameObject crosshair;
    #endregion

    #region Weapon&AmmunitionSpriteUpdates
    private void Update()
    {
        Weapon activeWeapon = WeaponManager.Instance.activeWeaponSlot.GetComponentInChildren<Weapon>();
        Weapon unActiveWeapon = GetUnActiveWeaponSlot().GetComponentInChildren<Weapon>();

        if (activeWeapon)
        {
            magazineAmmuntionUI.text = $"{activeWeapon.bulletsLeft / activeWeapon.bulletsPerBurst}";
            totalAmmuntionUI.text = $"{WeaponManager.Instance.CheckAmmunitionLeftFor(activeWeapon.thisWeaponModel)}";

            Weapon.WeaponModel model = activeWeapon.thisWeaponModel;
            ammuntionTypeUI.sprite = GetAmmunitionSprite(model);

            activeWeaponUI.sprite = GetWeaponSprite(model);

            if (unActiveWeapon)
            {
                unActiveWeaponUI.sprite = GetWeaponSprite(unActiveWeapon.thisWeaponModel);
            }
        }
        else
        {
            magazineAmmuntionUI.text = "";
            totalAmmuntionUI.text = "";

            ammuntionTypeUI.sprite = emptySlot;
            activeWeaponUI.sprite = emptySlot;
            unActiveWeaponUI.sprite = emptySlot;
        }

        if (WeaponManager.Instance.lethalsCount <= 0)
        {
            lethalUI.sprite = greySlot;
        }
        if (WeaponManager.Instance.tacticalsCount <= 0)
        {
            tacticalUI.sprite = greySlot;
        }
    }

    private GameObject GetUnActiveWeaponSlot()
    {
        foreach (GameObject weaponSlot in WeaponManager.Instance.weaponSlots)
        {
            if (weaponSlot != WeaponManager.Instance.activeWeaponSlot)
            {
                return weaponSlot;
            }
        }
        return null;
    }

    private Sprite GetWeaponSprite(Weapon.WeaponModel model)
    {
        switch (model)
        {
            case Weapon.WeaponModel.M1911:
                return Resources.Load<GameObject>("Pistol1911_Weapon").GetComponent<SpriteRenderer>().sprite;
            case Weapon.WeaponModel.M16A4:
                return Resources.Load<GameObject>("RifleM16A4_Weapon").GetComponent<SpriteRenderer>().sprite;
            default:
                return null;
        }
    }

    private Sprite GetAmmunitionSprite(Weapon.WeaponModel model)
    {
        switch (model)
        {
            case Weapon.WeaponModel.M1911:
                return Resources.Load<GameObject>("Pistol45_Bullet").GetComponent<SpriteRenderer>().sprite;
            case Weapon.WeaponModel.M16A4:
                return Resources.Load<GameObject>("Rifle5.56_Bullet").GetComponent<SpriteRenderer>().sprite;
            default:
                return null;
        }
    }
    #endregion
    #region Throwables
    internal void UpdateLethalThrowables(Throwable.ThrowableType throwableType)
    {
        lethalAmountUI.text = $"{WeaponManager.Instance.lethalsCount}";
        lethalUI.sprite = GetLethalSprite(throwableType);
    }

    private Sprite GetLethalSprite(Throwable.ThrowableType throwableType)
    {
        switch (throwableType)
        {
            case Throwable.ThrowableType.FragmentationGrenade:
                return Resources.Load<GameObject>("FragmentationGrenade_Throwable").GetComponent<SpriteRenderer>().sprite;
                /*
            case Throwable.ThrowableType.Pipebomb:
                Resources.Load<GameObject>("Pipebomb_Throwable").GetComponent<SpriteRenderer>().sprite;
                break;
            case Throwable.ThrowableType.MolotovCocktail:
                Resources.Load<GameObject>("MolotovCocktail_Throwable").GetComponent<SpriteRenderer>().sprite;
                break; */
            default:
                return null;
        }
    }
    
    internal void UpdateTacticalThrowables(Throwable.ThrowableType throwableType)
    {
        tacticalAmountUI.text = $"{WeaponManager.Instance.tacticalsCount}";
        tacticalUI.sprite = GetTacticalSprite(throwableType);
    }

    private Sprite GetTacticalSprite(Throwable.ThrowableType throwableType)
    {
        switch (throwableType)
        {
            case Throwable.ThrowableType.SmokeGrenade:
                return Resources.Load<GameObject>("SmokeGrenade_Throwable").GetComponent<SpriteRenderer>().sprite;
                
            /*case Throwable.ThrowableType.Decoy:
                return Resources.Load<GameObject>("Decoy_Throwable").GetComponent<SpriteRenderer>().sprite;

            case Throwable.ThrowableType.Flashbang:
                return Resources.Load<GameObject>("Flashbang_Throwable").GetComponent<SpriteRenderer>().sprite;
                */
            default: 
                return null;
        }
    }
    #endregion
}
