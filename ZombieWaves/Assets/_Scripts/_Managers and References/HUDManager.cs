using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

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

        if (WeaponManager.Instance.fGrenades <= 0)
        {
            lethalUI.sprite = greySlot;
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
                return Instantiate(Resources.Load<GameObject>("Pistol1911_Weapon")).GetComponent<SpriteRenderer>().sprite;
            case Weapon.WeaponModel.M16A4:
                return Instantiate(Resources.Load<GameObject>("RifleM16A4_Weapon")).GetComponent<SpriteRenderer>().sprite;
            default:
                return null;
        }
    }

    private Sprite GetAmmunitionSprite(Weapon.WeaponModel model)
    {
        switch (model)
        {
            case Weapon.WeaponModel.M1911:
                return Instantiate(Resources.Load<GameObject>("Pistol45_Bullet")).GetComponent<SpriteRenderer>().sprite;
            case Weapon.WeaponModel.M16A4:
                return Instantiate(Resources.Load<GameObject>("Rifle5.56_Bullet")).GetComponent<SpriteRenderer>().sprite;
            default:
                return null;
        }
    }

    internal void UpdateThrowables(Throwable.ThrowableType throwableType)
    {
        lethalAmountUI.text = $"{WeaponManager.Instance.lethalsCount}";

        switch (throwableType)
        {
            case Throwable.ThrowableType.FragmentationGrenade:
                lethalAmountUI.text = $"{WeaponManager.Instance.fGrenades}";
                lethalUI.sprite = Resources.Load<GameObject>("FragmentationGrenade_Throwable").GetComponent<SpriteRenderer>().sprite;
                break;
        }
    }
}
