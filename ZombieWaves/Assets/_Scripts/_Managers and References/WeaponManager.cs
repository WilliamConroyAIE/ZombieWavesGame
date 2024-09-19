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
    public int fGrenades = 3;
    public float throwForce = 40f;
    public GameObject fGrenadePrefab;
    public GameObject throwableSpawn;
    public float forceMultiplier = 0;
    public float forceMultiplierLimit = 2f;

    public int lethalsCount = 0;

    public Throwable.ThrowableType equippedType;

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

        if (Input.GetMouseButton(2) || Input.GetKey(KeyCode.G))
        {
            forceMultiplier += Time.deltaTime;

            if (forceMultiplier > forceMultiplierLimit)
            {
                forceMultiplier = forceMultiplierLimit;
            }
        }

        if (Input.GetMouseButtonUp(2)|| Input.GetKeyUp(KeyCode.G))
        {
            if (fGrenades > 0)
            {
                ThrowLethal();
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

    #region || --- Throwables --- |
    private void ThrowLethal()
    {
        GameObject lethalPrefab = GetThrowablePrefab();

        GameObject throwable = Instantiate(lethalPrefab, throwableSpawn.transform.position, Camera.main.transform.rotation);
        Rigidbody rb = throwable.GetComponent<Rigidbody>();

        rb.AddForce(Camera.main.transform.forward * (throwForce * forceMultiplier), ForceMode.Impulse);

        throwable.GetComponent<Throwable>().hasBeenThrown = true;

        fGrenades -= 1;
        HUDManager.Instance.UpdateThrowables(Throwable.ThrowableType.FragmentationGrenade);
    }

    private GameObject GetThrowablePrefab()
    {
        switch (equippedType)
        {
            case Throwable.ThrowableType.FragmentationGrenade:
                return fGrenadePrefab;
        }
        return new();
    }

    #endregion

    /*
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
*/
}
