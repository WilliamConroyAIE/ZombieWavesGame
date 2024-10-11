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
    //[Header("Ammunition")]
    public TextMeshProUGUI magazineAmmuntionUI;
    public TextMeshProUGUI totalAmmuntionUI;
    //public Image ammuntionTypeUI;

    [Header("Weapon")]
    //public Image activeWeaponUI;
    //public Image unActiveWeaponUI;
    public TextMeshProUGUI activeWeaponModelText;

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
        //Weapon unActiveWeapon = GetUnActiveWeaponSlot().GetComponentInChildren<Weapon>();

        if (activeWeapon)
        {
            magazineAmmuntionUI.text = $"{activeWeapon.bulletsLeft /activeWeapon.bulletsPerBurst}";
            totalAmmuntionUI.text = $"{WeaponManager.Instance.CheckAmmunitionLeftFor(activeWeapon.thisWeaponModel)}";

            Weapon.WeaponModel model = activeWeapon.thisWeaponModel;
            activeWeaponModelText.text = GetWeaponText(model);
            //ammuntionTypeUI.sprite = GetAmmunitionSprite(model);

            //activeWeaponUI.sprite = GetWeaponSprite(model);

            /*if (unActiveWeapon)
            {
                unActiveWeaponUI.sprite = GetWeaponSprite(unActiveWeapon.thisWeaponModel);
            }*/
        }
        else
        {
            magazineAmmuntionUI.text = "";
            totalAmmuntionUI.text = "";

            //ammuntionTypeUI.sprite = emptySlot;
            //activeWeaponUI.sprite = emptySlot;
            //unActiveWeaponUI.sprite = emptySlot;
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

    /*private GameObject GetUnActiveWeaponSlot()
    {
        foreach (GameObject weaponSlot in WeaponManager.Instance.weaponSlots)
        {
            if (weaponSlot != WeaponManager.Instance.activeWeaponSlot)
            {
                return weaponSlot;
            }
        }
        return null;
    }*/

    /*private Sprite GetWeaponSprite(Weapon.WeaponModel model)
    {
        switch (model)
        {
            case Weapon.WeaponModel.M16A4:
                return Resources.Load<GameObject>("RifleM16A4_Weapon").GetComponent<SpriteRenderer>().sprite;
            //
            case Weapon.WeaponModel.MicroUzi:
                return Resources.Load<GameObject>("SMGMicroUzi_Weapon").GetComponent<SpriteRenderer>().sprite;
            //    
            case Weapon.WeaponModel.M1Garand:
                return Resources.Load<GameObject>("RifleM1Garand_Weapon").GetComponent<SpriteRenderer>().sprite;
            //
            case Weapon.WeaponModel.MG42:
                return Resources.Load<GameObject>("RifleAK47_Weapon").GetComponent<SpriteRenderer>().sprite;
            //
            case Weapon.WeaponModel.SA80:
                return Resources.Load<GameObject>("SMGMP5_Weapon").GetComponent<SpriteRenderer>().sprite;
            //
            case Weapon.WeaponModel.Famas:
                return Resources.Load<GameObject>("RifleAK47_Weapon").GetComponent<SpriteRenderer>().sprite;
            //
            case Weapon.WeaponModel.MP5:
                return Resources.Load<GameObject>("SMGMP5_Weapon").GetComponent<SpriteRenderer>().sprite;
            //
            case Weapon.WeaponModel.MP40:
                return Resources.Load<GameObject>("SMGMicroUzi_Weapon").GetComponent<SpriteRenderer>().sprite;
            //
            case Weapon.WeaponModel.AK47:
                return Resources.Load<GameObject>("RifleAK47_Weapon").GetComponent<SpriteRenderer>().sprite;
            //
            case Weapon.WeaponModel.AK74U:
                return Resources.Load<GameObject>("RifleAK47_Weapon").GetComponent<SpriteRenderer>().sprite;
            //
            case Weapon.WeaponModel.Dragunov:
                return Resources.Load<GameObject>("SniperDragunov_Weapon").GetComponent<SpriteRenderer>().sprite;
            //
            case Weapon.WeaponModel.PPSH:
                return Resources.Load<GameObject>("RifleAK47_Weapon").GetComponent<SpriteRenderer>().sprite;
            //
            case Weapon.WeaponModel.Luger:
                return Resources.Load<GameObject>("PistolGlock_Weapon").GetComponent<SpriteRenderer>().sprite;
            //
            case Weapon.WeaponModel.DesertEagle:
                return Resources.Load<GameObject>("Pistol1911_Weapon").GetComponent<SpriteRenderer>().sprite;
            //
            case Weapon.WeaponModel.M9Beretta:
                return Resources.Load<GameObject>("PistolM9Beretta_Weapon").GetComponent<SpriteRenderer>().sprite;
            //
            case Weapon.WeaponModel.Revolver:
                return Resources.Load<GameObject>("PistolColtPython_Weapon").GetComponent<SpriteRenderer>().sprite;
            //
            case Weapon.WeaponModel.Glock17:
                return Resources.Load<GameObject>("PistolGlock17_Weapon").GetComponent<SpriteRenderer>().sprite;
            //
            case Weapon.WeaponModel.Tokarev:
                return Resources.Load<GameObject>("PistolM9Beretta_Weapon").GetComponent<SpriteRenderer>().sprite;
            //
            case Weapon.WeaponModel.Makarov:
                return Resources.Load<GameObject>("PistolMakarov_Weapon").GetComponent<SpriteRenderer>().sprite;
            //
            case Weapon.WeaponModel.M1911:
                return Resources.Load<GameObject>("Pistol1911_Weapon").GetComponent<SpriteRenderer>().sprite;
            //
            default:
                return null;
        }
    }

    
    private Sprite GetAmmunitionSprite(Weapon.WeaponModel model)
    {
        switch (model)
        {
            case Weapon.WeaponModel.M16A4:
                return Resources.Load<GameObject>("Rifle5.56_Bullet").GetComponent<SpriteRenderer>().sprite;
            //
            case Weapon.WeaponModel.MicroUzi:
                return Resources.Load<GameObject>("Pistol9_Bullet").GetComponent<SpriteRenderer>().sprite;
            //
            case Weapon.WeaponModel.M1Garand:
                return Resources.Load<GameObject>("Rifle30.06_Bullet").GetComponent<SpriteRenderer>().sprite;
            //
            case Weapon.WeaponModel.MG42:
                return Resources.Load<GameObject>("Rifle5.56_Bullet").GetComponent<SpriteRenderer>().sprite;
            //
            case Weapon.WeaponModel.SA80:
                return Resources.Load<GameObject>("Rifle5.56_Bullet").GetComponent<SpriteRenderer>().sprite;
            //
            case Weapon.WeaponModel.Famas:
                return Resources.Load<GameObject>("Pistol45_Bullet").GetComponent<SpriteRenderer>().sprite;
            //
            case Weapon.WeaponModel.MP5:
                return Resources.Load<GameObject>("Pistol45_Bullet").GetComponent<SpriteRenderer>().sprite;
            //
            case Weapon.WeaponModel.MP40:
                return Resources.Load<GameObject>("Pistol9_Bullet").GetComponent<SpriteRenderer>().sprite;
            //
            case Weapon.WeaponModel.AK47:
                return Resources.Load<GameObject>("Rifle7.62_Bullet").GetComponent<SpriteRenderer>().sprite;
            //
            case Weapon.WeaponModel.AK74U:
                return Resources.Load<GameObject>("Rifle7.62_Bullet").GetComponent<SpriteRenderer>().sprite;
            //
            case Weapon.WeaponModel.Dragunov:
                return Resources.Load<GameObject>("Rifle30.06_Bullet").GetComponent<SpriteRenderer>().sprite;
            //
            case Weapon.WeaponModel.PPSH:
                return Resources.Load<GameObject>("Pistol9_Bullet").GetComponent<SpriteRenderer>().sprite;
            //
            case Weapon.WeaponModel.Luger:
                return Resources.Load<GameObject>("Pistol9_Bullet").GetComponent<SpriteRenderer>().sprite;
            //
            case Weapon.WeaponModel.DesertEagle:
                return Resources.Load<GameObject>("Pistol50AE_Bullet").GetComponent<SpriteRenderer>().sprite;
            //
            case Weapon.WeaponModel.M9Beretta:
                return Resources.Load<GameObject>("Pistol9_Bullet").GetComponent<SpriteRenderer>().sprite;
            //
            case Weapon.WeaponModel.Revolver:
                return Resources.Load<GameObject>("Pistol45_Bullet").GetComponent<SpriteRenderer>().sprite;
            //
            case Weapon.WeaponModel.Glock17:
                return Resources.Load<GameObject>("Pistol9_Bullet").GetComponent<SpriteRenderer>().sprite;
            //
            case Weapon.WeaponModel.Tokarev:
                return Resources.Load<GameObject>("Pistol9_Bullet").GetComponent<SpriteRenderer>().sprite;
            //
            case Weapon.WeaponModel.Makarov:
                return Resources.Load<GameObject>("Pistol9_Bullet").GetComponent<SpriteRenderer>().sprite;
            //
            case Weapon.WeaponModel.M1911:
                return Resources.Load<GameObject>("Pistol45_Bullet").GetComponent<SpriteRenderer>().sprite;
            //
            default:
                return null;
        }
    }
    */
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
            case Throwable.ThrowableType.Pipebomb:
                return Resources.Load<GameObject>("Pipebomb_Throwable").GetComponent<SpriteRenderer>().sprite;
            case Throwable.ThrowableType.MolotovCocktail:
                return Resources.Load<GameObject>("MolotovCocktail_Throwable").GetComponent<SpriteRenderer>().sprite;
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
                
            case Throwable.ThrowableType.Decoy:
                return Resources.Load<GameObject>("Decoy_Throwable").GetComponent<SpriteRenderer>().sprite;

            case Throwable.ThrowableType.Flashbang:
                return Resources.Load<GameObject>("Flashbang_Throwable").GetComponent<SpriteRenderer>().sprite;
                
            default: 
                return null;
        }
    }
    #endregion

    public string GetWeaponText(Weapon.WeaponModel thisWeaponModel)
    {
        switch (thisWeaponModel)
        {
            case Weapon.WeaponModel.M16A4:
                return "M16A4:";
            case Weapon.WeaponModel.MicroUzi:
                return "Micro-Uzi:";
            case Weapon.WeaponModel.M1Garand:
                return "M1 Garand:";
            case Weapon.WeaponModel.MG42:
                return "MG42:";
            case Weapon.WeaponModel.SA80:
                return "SA80:";
            case Weapon.WeaponModel.Famas:
                return "Famas:";
            case Weapon.WeaponModel.MP5:
                return "HK-MP5:";
            case Weapon.WeaponModel.MP40:
                return "MP-40:";
            case Weapon.WeaponModel.AK47:
                return "AK-47:";
            case Weapon.WeaponModel.AK74U:
                return "AK-74U:";
            case Weapon.WeaponModel.Dragunov:
                return "Dragunov:";
            case Weapon.WeaponModel.Luger:
                return "Luger:";
            case Weapon.WeaponModel.DesertEagle:
                return "Desert Eagle:";
            case Weapon.WeaponModel.M9Beretta:
                return "M9:";
            case Weapon.WeaponModel.Revolver:
                return "380 Colt Python";
            case Weapon.WeaponModel.Tokarev:
                return "Tokarev:";
            case Weapon.WeaponModel.Makarov:
                return "Makarov:";
            case Weapon.WeaponModel.M1911:
                return "Colt-M1911:";
            default: 
                return "";
        }
    }
}
