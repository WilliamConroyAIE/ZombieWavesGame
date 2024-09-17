using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmmunitionManager : MonoBehaviour
{
    public static AmmunitionManager Instance { get; set; }

    public TextMeshProUGUI ammunitionDisplay;

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
}
