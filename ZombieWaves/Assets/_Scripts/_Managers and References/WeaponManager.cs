using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AmmunitionBox;
using static Throwable;
public class WeaponManager : MonoBehaviour
{
    #region || --- Variables --- ||
    public static WeaponManager Instance { get; set; }
    public List<GameObject> weaponSlots;
    public GameObject activeWeaponSlot;

    [Header("Ammunition")]
    public int totalPrimaryAmmunition = 0, totalSecondaryAmmunition = 0;

    [Header("Throwables")]
    public float throwForce = 40f;
    public GameObject throwableSpawn;
    public float forceMultiplier = 0;
    public float forceMultiplierLimit = 2f;

    [Header("Lethals")]
    public int maxLethals = 3;
    public int lethalsCount = 0;
    public Throwable.ThrowableType equippedLethalType;
    public GameObject fGrenadePrefab/*, mCocktailPrefab, pBombPrefab*/;

    
    [Header("Tacticals")]
    public int maxTacticals = 3;
    public int tacticalsCount = 0;
    public Throwable.ThrowableType equippedTacticalType;
    public GameObject sGrenadePrefab/*, decoyPrefab, fBangPrefab*/;
    

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

    #region || --- WeaponSwitching --- ||
    private void Start()
    {
        activeWeaponSlot = weaponSlots[0];

        equippedLethalType = Throwable.ThrowableType.None;
        equippedTacticalType = Throwable.ThrowableType.None;
    }

    private void Update()
    {
        foreach (GameObject weaponSlot in weaponSlots)
        {
            if (weaponSlot == activeWeaponSlot)
            {
                weaponSlot.SetActive(true);
            }
            else
            {
                weaponSlot.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchActiveSlot(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchActiveSlot(1);
        }

        //LethalThrow
        if (Input.GetMouseButton(2) || Input.GetKey(KeyCode.G))
        {
            forceMultiplier += Time.deltaTime;

            if (forceMultiplier > forceMultiplierLimit)
            {
                forceMultiplier = forceMultiplierLimit;
            }
        }

        if (Input.GetMouseButtonUp(2) || Input.GetKeyUp(KeyCode.G))
        {
            if (lethalsCount > 0)
            {
                ThrowLethal();
            }

            forceMultiplier = 0;
        }

        //ThrowTactical
        if (Input.GetKey(KeyCode.Alpha4))
        {
            forceMultiplier += Time.deltaTime;

            if (forceMultiplier > forceMultiplierLimit)
            {
                forceMultiplier = forceMultiplierLimit;
            }
        }

        if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            if (tacticalsCount > 0)
            {
                ThrowTactical();
            }

            forceMultiplier = 0;
        }
    }

    public void SwitchActiveSlot(int slotNumber)
    {
        if (activeWeaponSlot.transform.childCount > 0)
        {
            Weapon currentWeapon = activeWeaponSlot.transform.GetChild(0).GetComponent<Weapon>();
            currentWeapon.isActiveWeapon = false;
        }

        activeWeaponSlot = weaponSlots[slotNumber];

        if (activeWeaponSlot.transform.childCount > 0)
        {
            Weapon newWeapon = activeWeaponSlot.transform.GetChild(0).GetComponent<Weapon>();
            newWeapon.isActiveWeapon = true;
        }
    }
    #endregion

    #region || --- Weapon --- ||
    public void PickupWeapon(GameObject pickedupWeapon)
    {
        AddWeaponIntoActiveSlot(pickedupWeapon);
    }

    private void AddWeaponIntoActiveSlot(GameObject pickedupWeapon)
    {
        DropCurrentWeapon(pickedupWeapon);

        pickedupWeapon.transform.SetParent(activeWeaponSlot.transform, false);

        Weapon weapon = pickedupWeapon.GetComponent<Weapon>();

        pickedupWeapon.transform.localPosition = new Vector3(weapon.spawnPosition.x, weapon.spawnPosition.y, weapon.spawnPosition.z);
        pickedupWeapon.transform.localRotation = Quaternion.Euler(weapon.spawnRotation.x, weapon.spawnRotation.y, weapon.spawnRotation.z);

        weapon.isActiveWeapon = true;
        weapon.animator.enabled = true; 
    }

    internal void PickupAmmunition(AmmunitionBox ammunition)
    {
        switch (ammunition.ammunitionType)
        {
            case AmmunitionBox.AmmunitionType.primaryAmmunition:
                totalPrimaryAmmunition += ammunition.ammunitionAmount;
                break;
            case AmmunitionBox.AmmunitionType.secondaryAmmunition:
                totalSecondaryAmmunition += ammunition.ammunitionAmount;
                break;

            default:
                break;
        }
    }

    private void DropCurrentWeapon(GameObject pickedUpWeapon)
    {
        if (activeWeaponSlot.transform.childCount > 0)
        {
            var weaponToDrop = activeWeaponSlot.transform.GetChild(0).gameObject;

            weaponToDrop.GetComponent<Weapon>().isActiveWeapon = false;
            weaponToDrop.GetComponent<Weapon>().animator.enabled = false;
            
            weaponToDrop.transform.SetParent(pickedUpWeapon.transform.parent);
            weaponToDrop.transform.localPosition = pickedUpWeapon.transform.localPosition;
            weaponToDrop.transform.localRotation = pickedUpWeapon.transform.localRotation;
        }
    }

    #endregion

    #region || --- Ammunition --- ||
    internal void DecreaseTotalAmmunition(int bulletsToDecrease, Weapon.WeaponModel thisWeaponModel)
    {
        switch (thisWeaponModel)
        {
            case Weapon.WeaponModel.M16A4:
                totalPrimaryAmmunition -= bulletsToDecrease;
                break;
            case Weapon.WeaponModel.M1911:
                totalSecondaryAmmunition -= bulletsToDecrease;
                break;
            default:
                break;
        }
    }

    public int CheckAmmunitionLeftFor(Weapon.WeaponModel thisWeaponModel)
    {
        switch (thisWeaponModel)
        {
            case Weapon.WeaponModel.M16A4:
                return totalPrimaryAmmunition;
            case Weapon.WeaponModel.M1911:
                return totalSecondaryAmmunition;

            default:
                return 0;
        }
    }
    #endregion 

    #region || --- LethalThrowables --- ||

    public void PickupThrowable(Throwable throwable)
    {
        switch (throwable.throwableType)
        {
            case Throwable.ThrowableType.FragmentationGrenade:
                PickupThrowableAsLethal(Throwable.ThrowableType.FragmentationGrenade);
                break;
                /*
            case Throwable.ThrowableType.Pipebomb:
                PickupThrowableAsLethal(throwable.ThrowableType.Pipebomb);
                break;
            case Throwable.ThrowableType.MolotovCocktail:
                PickupThrowableAsLethal(throwable.ThrowableType.MolotovCocktail);
                break;
            case Throwable.ThrowableType.Decoy:
                PickupThrowableAsTactical(Throwable.ThrowableType.Decoy);
                break; */
            case Throwable.ThrowableType.SmokeGrenade:
                PickupThrowableAsTactical(Throwable.ThrowableType.SmokeGrenade);
                break;
            //case Throwable.ThrowableType.Flashbang:
                //PickupThrowableAsTactical(Throwable.ThrowableType.Flashbang);
                //break;
            default:
                break;
        }
    }

    private void PickupThrowableAsLethal(Throwable.ThrowableType lethal)
    {
        if (equippedLethalType == lethal || equippedLethalType == Throwable.ThrowableType.None)
        {
            equippedLethalType = lethal;

            if (lethalsCount < maxLethals)
            {
                lethalsCount += 1;
                Destroy(InteractionManager.Instance.hoveredThrowable.gameObject);
                HUDManager.Instance.UpdateLethalThrowables(equippedLethalType);
            }
            else
            {
                print("LimitReached (L)");
            }
        }
    }

    private void ThrowLethal()
    {
        GameObject lethalPrefab = GetLethalThrowablePrefab();

        GameObject throwable = Instantiate(lethalPrefab, throwableSpawn.transform.position, Camera.main.transform.rotation);
        Rigidbody rb = throwable.GetComponent<Rigidbody>();

        rb.AddForce(Camera.main.transform.forward * (throwForce * forceMultiplier), ForceMode.Impulse);

        throwable.GetComponent<Throwable>().hasBeenThrown = true;
        lethalsCount -= 1;

        if (lethalsCount == 0)
        {
            equippedLethalType = Throwable.ThrowableType.None;
        }

        HUDManager.Instance.UpdateLethalThrowables(equippedLethalType);
    }

    internal GameObject GetLethalThrowablePrefab()
    {
        switch (equippedLethalType)
        {
            case Throwable.ThrowableType.FragmentationGrenade:
                return fGrenadePrefab;/*
            case Throwable.ThrowableType.Pipebomb:
                return pBombPrefab;
            case Throwable.ThrowableType.MolotovCocktail:
                return mCocktailPrefab;*/
        }
        return new();
    }

#endregion

    #region || --- TacticalThrowables --- ||
    
    private void PickupThrowableAsTactical(Throwable.ThrowableType tactical)
    {
        if (equippedTacticalType == tactical || equippedTacticalType == Throwable.ThrowableType.None)
        {
            equippedTacticalType = tactical;

            if (tacticalsCount < maxTacticals)
            {
                tacticalsCount += 1;
                Destroy(InteractionManager.Instance.hoveredThrowable.gameObject);
                HUDManager.Instance.UpdateTacticalThrowables(equippedTacticalType);
            }
            else
            {
                print("LimitReached (T)");
            }
        }
    }

    private void ThrowTactical()
    {
        GameObject tacticalPrefab = GetTacticalThrowablePrefab();

        GameObject throwable = Instantiate(tacticalPrefab, throwableSpawn.transform.position, Camera.main.transform.rotation);
        Rigidbody rb = throwable.GetComponent<Rigidbody>();

        rb.AddForce(Camera.main.transform.forward * (throwForce * forceMultiplier), ForceMode.Impulse);

        throwable.GetComponent<Throwable>().hasBeenThrown = true;

        tacticalsCount -= 1;
        HUDManager.Instance.UpdateTacticalThrowables(equippedTacticalType);
    }

    internal GameObject GetTacticalThrowablePrefab()
    {
        switch (equippedTacticalType)
        {
            case Throwable.ThrowableType.SmokeGrenade:
                return sGrenadePrefab;
            /*case Throwable.ThrowableType.Decoy:
                return decoyPrefab;
            case Throwable.ThrowableType.Flashbang:
                return fBangPrefab;*/
        }
        return new();
    }
    
    #endregion
}
