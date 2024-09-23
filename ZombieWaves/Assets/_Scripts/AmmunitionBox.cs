using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmunitionBox : MonoBehaviour
{
    public int ammunitionAmount = 200;
    public AmmunitionType ammunitionType;
    public enum AmmunitionType
    {
        primaryAmmunition,
        secondaryAmmunition
        
    }

    private void Start()
    {
        GetComponent<Outline>().enabled = false;
    }
}
