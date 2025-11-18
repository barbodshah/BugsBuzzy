using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FPSLevelManager : MonoBehaviour
{
    public static FPSLevelManager Instance;
    public static int MAX_ZOMBIES = 25;

    public List<FPSZombie> zombies = new List<FPSZombie>();
    public List<Health> Humans = new List<Health>();

    [Header("Stats")]
    public int totalKilled;
    public int totalSpawned;
    public int spawnCount;
    public int waveKilled;
    public int wave;
    public int score;

    [Header("Spawn Variables")]
    public float spawnWait;
    public float runningZombieSpawnChance;
    public int spawnLimit;
    public float waitTime;
    public bool inWaveWait;
    public int WaveWait;
    public int remainingTime;
    public float spawnBatch;

    private void Awake()
    {
        Instance = this;
        SetInitialVariables();
    }
    void SetInitialVariables()
    {
        spawnBatch = 1.8f;
        spawnWait = 3;
        runningZombieSpawnChance = 0.15f;
        spawnLimit = 25;
    }
    private void Update()
    {
        if (waveKilled >= spawnLimit && inWaveWait == false)
        {
            newWave();
        }
    }

    #region Wave
    void newWave()
    {
        wave++;
        spawnBatch += 0.2f;

        spawnWait -= 0.1f;
        if (spawnWait < 0.4f) spawnWait = 0.4f;

        runningZombieSpawnChance += 0.05f;
        if (runningZombieSpawnChance > 0.5f) runningZombieSpawnChance = 0.5f;

        waveKilled = 0;
        spawnLimit += 5;
        if (wave == 1) spawnLimit = 25;

        StartCoroutine(newWaveWait());
    }

    IEnumerator newWaveWait()
    {
        inWaveWait = true;
        for (int i = WaveWait - 1; i >= 0; i--)
        {
            remainingTime = i;
            yield return new WaitForSeconds(1);
        }
        spawnCount = 0;
        inWaveWait = false;
    }
    #endregion
    #region Events
    public void HeadShot()
    {
        score += 1;
    }
    public void ZombieKilled()
    {
        totalKilled++;
        waveKilled++;
        score += 2;
    }
    public void PlayerDead()
    {
        UIControllerFPS.instance.PlayerDeath();

        FPSSpawnController.instance.enabled = false;

        foreach(FPSZombie z in zombies)
        {
            Destroy(z.gameObject);
            Destroy(z.currentWayPoint);
        }
    }
    public void ReloadLevel()
    {
        SceneManager.LoadScene(0);
    }
    #endregion
}
