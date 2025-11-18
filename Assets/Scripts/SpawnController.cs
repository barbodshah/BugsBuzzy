using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public static SpawnController instance;

    public Transform[] spawnPositions;
    public Transform bossSpawnPosition;

    public Transform[] soldierSpawnPositions;
    public Transform[] soldierDestinations;

    public GameObject soldierPrefab;

    //public float spawnWait;
    //public int spawnBatch;

    public bool canSpawn = true;

    public GameObject[] zombies;
    public GameObject boss;

    private void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        if (GameController.instance.waveMode)
        {
            if (canSpawn)
            {
                spawnZombies();
                canSpawn = false;

                StartCoroutine(SpawnWait());
            }
        }
        else
        {
            if (canSpawn && LevelManager.instance.zombies.Count < LevelManager.MAX_ZOMBIES)
            {
                spawnZombieHardcore();
                canSpawn = false;

                StartCoroutine (SpawnWait());
            }
        }
    }
    public void CallBackup()
    {
        StartCoroutine(spawnBackUp());
    }
    IEnumerator spawnBackUp()
    {
        for (int i = 0; i < soldierDestinations.Length; i++)
        {
            GameObject go = Instantiate(soldierPrefab, soldierSpawnPositions[i].position, soldierSpawnPositions[i].rotation);
            go.GetComponent<SoldierAI>().destination = soldierDestinations[i];

            yield return new WaitForSeconds(1f);
        }
    }
    public void SpawnBoss()
    {
        Instantiate(boss, bossSpawnPosition.position, bossSpawnPosition.rotation);
        LevelManager.instance.totalSpawnCount++;
    }
    public void spawnZombieHardcore()
    {
        for (int i = 0; i < LevelManager.instance.spawnBatch; i++)
        {
            LevelManager.instance.spawnCount++;
            LevelManager.instance.totalSpawnCount++;

            int randomIndex = Random.Range(0, spawnPositions.Length);
            float zombieType = Random.Range(0, 1f);


            if (zombieType > LevelManager.instance.fastZombieSpawnChance)
            {
                Instantiate(zombies[0], spawnPositions[randomIndex].position, Quaternion.identity);
            }
            else
            {
                Instantiate(zombies[1], spawnPositions[randomIndex].position, Quaternion.identity);
            }
        }
    }
    public void spawnZombies()
    {
        for (int i = 0; i < LevelManager.instance.spawnBatch; i++)
        {
            if (LevelManager.instance.spawnCount >= LevelManager.instance.spawnLimit || LevelManager.instance.inWaveWait) return;

            LevelManager.instance.spawnCount++;
            LevelManager.instance.totalSpawnCount++;

            int randomIndex = Random.Range(0, spawnPositions.Length);
            float zombieType = Random.Range(0, 1f);


            if (zombieType > LevelManager.instance.fastZombieSpawnChance)
            {
                float rand = Random.Range(0, 1f);

                if (rand > LevelManager.instance.tankZombieSpawnChance)
                {
                    Instantiate(zombies[0], spawnPositions[randomIndex].position, Quaternion.identity);
                }
                else
                {
                    Instantiate(zombies[2], spawnPositions[randomIndex].position, Quaternion.identity);
                }
            }
            else
            {
                Instantiate(zombies[1], spawnPositions[randomIndex].position, Quaternion.identity);
            }
        }
        //StartCoroutine(SpawnZombiesTimer());
    }
    IEnumerator SpawnZombiesTimer()
    {
        int fastZombies = (int) (LevelManager.instance.fastZombieSpawnChance * LevelManager.instance.spawnLimit);
        int slowZombies = LevelManager.instance.spawnLimit - fastZombies;

        for (int i = 0; i < slowZombies; i++)
        {
            LevelManager.instance.spawnCount++;
            yield return new WaitForSeconds(LevelManager.instance.spawnWait);

            int randomIndex = Random.Range(0, spawnPositions.Length);
            Instantiate(zombies[0], spawnPositions[randomIndex].position, Quaternion.identity);
        }
        for (int i = 0; i < fastZombies; i++)
        {
            LevelManager.instance.spawnCount++;
            yield return new WaitForSeconds(LevelManager.instance.spawnWait);

            int randomIndex = Random.Range(0, spawnPositions.Length);
            Instantiate(zombies[1], spawnPositions[randomIndex].position, Quaternion.identity);
        }
    }
    IEnumerator SpawnWait()
    {
        yield return new WaitForSeconds(LevelManager.instance.spawnWait);
        canSpawn = true;
    }
}
