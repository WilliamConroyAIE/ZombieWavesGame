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
    public float forceMultiplier = 0, timeMultiplier = 0;
    public float forceMultiplierLimit = 2f, timeMultiplierLimit = 5f;
    public bool throwableCooldownDone;
    public float throwableCooldown;

    [Header("Lethals")]
    public int maxLethals = 3;
    public int lethalsCount = 0;
    public Throwable.ThrowableType equippedLethalType;
    public GameObject fGrenadePrefab, mCocktailPrefab, pBombPrefab;

    
    [Header("Tacticals")]
    public int maxTacticals = 3;
    public int tacticalsCount = 0;
    public Throwable.ThrowableType equippedTacticalType;
    public GameObject sGrenadePrefab, decoyPrefab, fBangPrefab;
    

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

        throwableCooldown = 0f;

        throwableCooldownDone = true;
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
            if (throwableCooldownDone)
            {
                forceMultiplier += Time.deltaTime;

                if (forceMultiplier > forceMultiplierLimit)
                {
                    forceMultiplier = forceMultiplierLimit;
                    timeMultiplier += Time.deltaTime;
                }
                if (timeMultiplier > timeMultiplierLimit)
                {
                    timeMultiplier = timeMultiplierLimit;
                }

                if (forceMultiplier == forceMultiplierLimit && timeMultiplier == timeMultiplierLimit)
                {
                    ThrowLethal();
                    timeMultiplier = 0;
                }
            }
        }

        if (Input.GetMouseButtonUp(2) || Input.GetKeyUp(KeyCode.G))
        {
            if (lethalsCount > 0)
            {
                if (throwableCooldownDone)
                    ThrowLethal();
            }

            forceMultiplier = 0;
        }

        //ThrowTactical
        if (Input.GetKey(KeyCode.Alpha4))
        {
            if (throwableCooldownDone)
            {
                forceMultiplier += Time.deltaTime;

                if (forceMultiplier > forceMultiplierLimit)
                {
                    forceMultiplier = forceMultiplierLimit;
                    timeMultiplier += Time.deltaTime;
                }
                if (timeMultiplier > timeMultiplierLimit)
                {
                    timeMultiplier = timeMultiplierLimit;
                }

                if (forceMultiplier == forceMultiplierLimit && timeMultiplier == timeMultiplierLimit)
                {
                    ThrowTactical();
                    timeMultiplier = 0;
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            if (tacticalsCount > 0)
            {
                if (throwableCooldownDone)
                    ThrowTactical();
            }

            forceMultiplier = 0;
        }

        if (throwableCooldown > 0.1f)
        {
            if (!throwableCooldownDone)
            {
                throwableCooldown -= Time.deltaTime;
                
                if (throwableCooldown < 0f)
                {
                    throwableCooldown = 0f;
                    throwableCooldownDone = true;
                }
            }
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
            //
            case Weapon.WeaponModel.MicroUzi:
                totalPrimaryAmmunition -= bulletsToDecrease;
                break;
            //
            case Weapon.WeaponModel.M1Garand:
                totalPrimaryAmmunition -= bulletsToDecrease;
                break;
            //
            case Weapon.WeaponModel.MG42:
                totalPrimaryAmmunition -= bulletsToDecrease;
                break;
            //
            case Weapon.WeaponModel.SA80:
                totalPrimaryAmmunition -= bulletsToDecrease;
                break;
            //
            case Weapon.WeaponModel.Famas:
                totalPrimaryAmmunition -= bulletsToDecrease;
                break;
            //
            case Weapon.WeaponModel.MP5:
                totalPrimaryAmmunition -= bulletsToDecrease;
                break;
            //
            case Weapon.WeaponModel.MP40:
                totalPrimaryAmmunition -= bulletsToDecrease;
                break;
            //
            case Weapon.WeaponModel.AK47:
                totalPrimaryAmmunition -= bulletsToDecrease;
                break;
            //
            case Weapon.WeaponModel.AK74U:
                totalPrimaryAmmunition -= bulletsToDecrease;
                break;
            //
            case Weapon.WeaponModel.Dragunov:
                totalPrimaryAmmunition -= bulletsToDecrease;
                break;
            //
            case Weapon.WeaponModel.PPSH:
                totalPrimaryAmmunition -= bulletsToDecrease;
                break;
            //
            case Weapon.WeaponModel.Luger:
                totalSecondaryAmmunition -= bulletsToDecrease;
                break;
            //
            case Weapon.WeaponModel.DesertEagle:
                totalSecondaryAmmunition -= bulletsToDecrease;
                break;
            //
            case Weapon.WeaponModel.M9Beretta:
                totalSecondaryAmmunition -= bulletsToDecrease;
                break;
            //
            case Weapon.WeaponModel.Revolver:
                totalSecondaryAmmunition -= bulletsToDecrease;
                break;
            //
            case Weapon.WeaponModel.Glock17:
                totalSecondaryAmmunition -= bulletsToDecrease;
                break;
            //
            case Weapon.WeaponModel.Tokarev:
                totalSecondaryAmmunition -= bulletsToDecrease;
                break;
            //
            case Weapon.WeaponModel.Makarov:
                totalSecondaryAmmunition -= bulletsToDecrease;
                break;
            //
            case Weapon.WeaponModel.M1911:
                totalSecondaryAmmunition -= bulletsToDecrease;
                break;
            //
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
            //
            case Weapon.WeaponModel.MicroUzi:
                return totalPrimaryAmmunition;
            //
            case Weapon.WeaponModel.M1Garand:
                return totalPrimaryAmmunition;
            //
            case Weapon.WeaponModel.MG42:
                return totalPrimaryAmmunition;
            //
            case Weapon.WeaponModel.SA80:
                return totalPrimaryAmmunition;
            //
            case Weapon.WeaponModel.Famas:
                return totalPrimaryAmmunition;
            //
            case Weapon.WeaponModel.MP5:
                return totalPrimaryAmmunition;
            //
            case Weapon.WeaponModel.MP40:
                return totalPrimaryAmmunition;
            //
            case Weapon.WeaponModel.AK47:
                return totalPrimaryAmmunition;
            //
            case Weapon.WeaponModel.AK74U:
                return totalPrimaryAmmunition;
            //
            case Weapon.WeaponModel.Dragunov:
                return totalPrimaryAmmunition;
            //
            case Weapon.WeaponModel.PPSH:
                return totalPrimaryAmmunition;
            //
            case Weapon.WeaponModel.Luger:
                return totalSecondaryAmmunition;
            //
            case Weapon.WeaponModel.DesertEagle:
                return totalSecondaryAmmunition;
            //
            case Weapon.WeaponModel.M9Beretta:
                return totalSecondaryAmmunition;
            //
            case Weapon.WeaponModel.Revolver:
                return totalSecondaryAmmunition;
            //
            case Weapon.WeaponModel.Glock17:
                return totalSecondaryAmmunition;
            //
            case Weapon.WeaponModel.Tokarev:
                return totalSecondaryAmmunition;
            //
            case Weapon.WeaponModel.Makarov:
                return totalSecondaryAmmunition;
            //
            case Weapon.WeaponModel.M1911:
                return totalSecondaryAmmunition;
            //
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
            case Throwable.ThrowableType.Pipebomb:
                PickupThrowableAsLethal(Throwable.ThrowableType.Pipebomb);
                break;
            case Throwable.ThrowableType.MolotovCocktail:
                PickupThrowableAsLethal(Throwable.ThrowableType.MolotovCocktail);
                break;
            case Throwable.ThrowableType.Decoy:
                PickupThrowableAsTactical(Throwable.ThrowableType.Decoy);
                break; 
            case Throwable.ThrowableType.SmokeGrenade:
                PickupThrowableAsTactical(Throwable.ThrowableType.SmokeGrenade);
                break;
            case Throwable.ThrowableType.Flashbang:
                PickupThrowableAsTactical(Throwable.ThrowableType.Flashbang);
                break;
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
        if (throwableCooldownDone)
        {
            GameObject lethalPrefab = GetLethalThrowablePrefab();

            GameObject throwable = Instantiate(lethalPrefab, throwableSpawn.transform.position, Camera.main.transform.rotation);

            if (forceMultiplier == forceMultiplierLimit && timeMultiplier == timeMultiplierLimit)
            {
                throwable.GetComponent<Throwable>().playerHeldTooLong = true;
            }

            if (throwable.GetComponent<Throwable>().playerHeldTooLong == false)
            {
                Rigidbody rb = throwable.GetComponent<Rigidbody>();
                rb.AddForce(Camera.main.transform.forward * (throwForce * forceMultiplier), ForceMode.Impulse);
            }

            throwable.GetComponent<Throwable>().hasBeenThrown = true;
            lethalsCount -= 1;

            if (lethalsCount == 0)
            {
                equippedLethalType = Throwable.ThrowableType.None;
            }

            HUDManager.Instance.UpdateLethalThrowables(equippedLethalType);

            throwableCooldown = 1.5f;
        }
    }

    internal GameObject GetLethalThrowablePrefab()
    {
        switch (equippedLethalType)
        {
            case Throwable.ThrowableType.FragmentationGrenade:
                return fGrenadePrefab;
            case Throwable.ThrowableType.Pipebomb:
                return pBombPrefab;
            case Throwable.ThrowableType.MolotovCocktail:
                return mCocktailPrefab;
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
        if (throwableCooldownDone)
        {
            GameObject tacticalPrefab = GetTacticalThrowablePrefab();

            GameObject throwable1 = Instantiate(tacticalPrefab, throwableSpawn.transform.position, Camera.main.transform.rotation);

            if (forceMultiplier == forceMultiplierLimit && timeMultiplier == timeMultiplierLimit)
            {
                throwable1.GetComponent<Throwable>().playerHeldTooLong = true;
            }
            
            if (throwable1.GetComponent<Throwable>().playerHeldTooLong == false)
            {
                Rigidbody rb = throwable1.GetComponent<Rigidbody>();
                rb.AddForce(Camera.main.transform.forward * (throwForce * forceMultiplier), ForceMode.Impulse);
            }

            throwable1.GetComponent<Throwable>().hasBeenThrown = true;
            tacticalsCount -= 1;

            if (tacticalsCount == 0)
            {
                equippedTacticalType = Throwable.ThrowableType.None;
            }

            HUDManager.Instance.UpdateTacticalThrowables(equippedTacticalType);

            throwableCooldown = 1.5f;
        }
    }


    internal GameObject GetTacticalThrowablePrefab()
    {
        switch (equippedTacticalType)
        {
            case Throwable.ThrowableType.Flashbang:
                return fBangPrefab;
            case Throwable.ThrowableType.SmokeGrenade:
                return sGrenadePrefab;
            case Throwable.ThrowableType.Decoy:
                return decoyPrefab;
        }
        return new();
    }
    
    #endregion
}
