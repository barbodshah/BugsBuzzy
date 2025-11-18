using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSSpawnController : MonoBehaviour
{
    public static FPSSpawnController instance;

    public Transform[] spawnPositions;
    public GameObject[] zombies;

    public bool canSpawn = true;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (canSpawn)
        {
            canSpawn = false;
            SpawnSingleZombie();

            StartCoroutine(SpawnWaitReset());
        }
    }
    void SpawnSingleZombie()
    {
        for (int i = 0; i < FPSLevelManager.Instance.spawnBatch; i++)
        {
            if (FPSLevelManager.Instance.spawnCount >= FPSLevelManager.Instance.spawnLimit || FPSLevelManager.Instance.inWaveWait) return;

            float rand = Random.Range(0f, 1f);
            GameObject temp;

            if (rand <= FPSLevelManager.Instance.runningZombieSpawnChance) temp = zombies[1];
            else temp = zombies[0];

            int randomIndex = Random.Range(0, spawnPositions.Length);
            Instantiate(temp, spawnPositions[randomIndex].position, spawnPositions[randomIndex].rotation);

            FPSLevelManager.Instance.totalSpawned++;
            FPSLevelManager.Instance.spawnCount++;
        }
    }
    IEnumerator SpawnWaitReset()
    {
        yield return new WaitForSeconds(FPSLevelManager.Instance.spawnWait);
        canSpawn = true;
    }
}
