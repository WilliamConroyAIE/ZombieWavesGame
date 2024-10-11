using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using TMPro;
using UnityEngine.UI;

public class EnemySpawnController : MonoBehaviour
{
    public int initialEnemiesPerWave = 5;
    public int currentEnemiesPerWave;

    public float spawnDelay = 0.5f;

    public int currentWave = 0;
    public float waveCooldown = 10.0f;

    public bool inCooldown;
    public float cooldownCounter = 0;

    public List<Enemy> currentEnemiesAlive;

    public GameObject enemyPrefab;

    public TextMeshProUGUI waveOverUI;
    public TextMeshProUGUI cooldownCounterUI;

    public TextMeshProUGUI waveCounterUI;

    private void Start()
    {
        currentEnemiesPerWave = initialEnemiesPerWave;
        waveOverUI.gameObject.SetActive(false);
        cooldownCounterUI.gameObject.SetActive(false);
        GlobalReference.Instance.waveNumber = currentWave;
        
        StartNextWave();
    }

    private void StartNextWave()
    {
        currentEnemiesAlive.Clear();
        
        currentWave++;
        waveOverUI.gameObject.SetActive(false);
        cooldownCounterUI.gameObject.SetActive(false);

        GlobalReference.Instance.waveNumber = currentWave;

        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        for (int i = 0; i < currentEnemiesPerWave; i++)
        {
            Vector3 spawnOffset = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
            Vector3 spawnPosition = transform.position + spawnOffset;

            var enemy1 = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

            Enemy enemyScript = enemy1.GetComponent<Enemy>();

            currentEnemiesAlive.Add(enemyScript);

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private void Update()
    {
        List<Enemy> enemiesToRemove = new List<Enemy>();
        foreach (Enemy zombie in currentEnemiesAlive)
        {
            if (zombie.isDead)
            {
                enemiesToRemove.Add(zombie);
            }
        }

        foreach (Enemy zombie in enemiesToRemove)
        {
            currentEnemiesAlive.Remove(zombie);
        }

        enemiesToRemove.Clear();

        if (currentEnemiesAlive.Count == 0 && !inCooldown)
        {
            StartCoroutine(WaveCooldown());
        }

        if (inCooldown)
        {
            cooldownCounter -= Time.deltaTime;
        }
        else
        {
            cooldownCounter = waveCooldown;
        }
        
        waveCounterUI.text = $"Wave NO: {currentWave}";


        cooldownCounterUI.text = cooldownCounter.ToString("F0");
    }

    private IEnumerator WaveCooldown()
    {
        inCooldown = true;

        waveOverUI.gameObject.SetActive(true);
        cooldownCounterUI.gameObject.SetActive(true);

        yield return new WaitForSeconds(waveCooldown);

        inCooldown = false;

        int randomDeathValue = Random.Range(0,2);

        if (randomDeathValue == 0)
        {
            currentEnemiesPerWave += 2;
        }
        if (randomDeathValue == 1)
        {
            currentEnemiesPerWave *= 2;
        }
        if (randomDeathValue == 2)
        {
            currentEnemiesPerWave *= 2;
        }

        StartNextWave();
    }
}
