using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalReference : MonoBehaviour
{
    public static GlobalReference Instance { get; set; }

    [Header("BulletHitEffects")]
    public GameObject bulletImpactEffectPrefab;
    public GameObject hitEffectPrefab;

    [Header("ThrowableEffects")]
    public GameObject grenadeExplosionEffect;
    public GameObject PBEffect, MCEffect, SmokeExplosionEffect, BlindnessEffect;


    public int waveNumber;

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
