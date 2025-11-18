using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public List<Zombie> zombies = new List<Zombie>();
    public List<Transform> Humans = new List<Transform>();

    public static int MAX_ZOMBIES = 25;
    public static float BACKUP_COOLDOWN = 120;

    CannonController cannonController;

    [Header("Level Variables")]
    public float globalTimeScale;
    public float spawnBatch;
    public float spawnWait;
    public int spawnLimit;
    public int spawnCount = 0;
    public int totalSpawnCount = 0;
    public int bossSpawnLimit = 25;
    public int bossSpawnInterval = 25;
    public float fastZombieSpawnChance;
    public float tankZombieSpawnChance;

    [Header("Stats")]
    public int score = 0;
    public int kills = 0;
    public int totalKills;
    public int wave = 0;

    //For UI
    [Header("UI")]
    public bool inWaveWait = false;
    public int remainingTime;
    public int WaveWait;
    public bool playerDead = false;
    public Health playerHeath;

    [Header("Time Freeze")]
    public bool timeFreeze = false;
    public float freezeDuration;
    public float slowDownFactor = 3f;
    public PostProcessVolume processVolume;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        if (!cannonController) cannonController = FindObjectOfType<CannonController>();

        SetInitialVariables();
    }
    void SetInitialVariables()
    {
        globalTimeScale = 1;
        Physics.gravity = new Vector3(0f, -9.81f, 0f);

        if (GameController.instance.waveMode)
        {
            spawnBatch = 1.8f;
            spawnWait = 3;
            fastZombieSpawnChance = 0.15f;
            tankZombieSpawnChance = 0.1f;
        }
        else
        {
            spawnBatch = 1;
            spawnWait = 4;
            fastZombieSpawnChance = 0.2f;
            tankZombieSpawnChance = 0.1f;
        }
    }
    private void Update()
    {
        if (GameController.instance.waveMode)
        {
            if (kills >= spawnLimit && inWaveWait == false)
            {
                newWave();
            }
        }
        else
        {
            if (totalSpawnCount >= bossSpawnLimit)
            {
                SpawnController.instance.SpawnBoss();
                bossSpawnLimit += bossSpawnInterval;
            }
            if (kills >= spawnLimit)
            {
                newWave();
            }
        }
    }
    #region WaveMode
    void newWave()
    {
        wave++;
        spawnBatch += 0.2f;

        spawnWait -= 0.1f;
        if (spawnWait < 0.4f) spawnWait = 0.4f;

        fastZombieSpawnChance += 0.05f;
        if (fastZombieSpawnChance > 0.5f) fastZombieSpawnChance = 0.5f;

        tankZombieSpawnChance += 0.05f;
        if (tankZombieSpawnChance > 0.7f) tankZombieSpawnChance = 0.7f;

        kills = 0;
        spawnLimit += 5;
        if (wave == 1) spawnLimit = 25;

        if(GameController.instance.waveMode) StartCoroutine(newWaveWait());

        if (wave % 5 == 0)
        {
            if(wave % 10 != 0)
            {
                BiomeController.instance.StartColorTransition(0);
            }
            else
            {
                BiomeController.instance.StartColorTransition(1);
            }
        }
    }
    IEnumerator newWaveWait()
    {
        inWaveWait = true;
        for(int i = WaveWait - 1; i >= 0; i--)
        {
            remainingTime = i;
            yield return new WaitForSeconds(1);

            if (i == 0) AudioManager.instance.Boom1();
            else AudioManager.instance.Boom2();
        }
        spawnCount = 0;
        inWaveWait = false;
    }
    #endregion

    #region Events
    public void AddScore(int score, bool feedback = false)
    {
        this.score += score;

        if (feedback)
        {
            UIController.instance.ScoreFeedback1(score);
        }
    }
    public void ZombieKilledEvent(Transform t)
    {
        score += 2;
        kills += 1;
        totalKills += 1;

        //int rand = Random.Range(0, 3);
        //UIController.instance.ZombieDeathFeedback(t);
    }
    public void PlayerDead()
    {
        if (playerDead) return;
        playerDead = true;

        UIController.instance.PlayerDead();
        AudioManager.instance.PlayerDeath();
        GameController.instance.AddGem(score / 10);

        Destroy(SpawnController.instance.gameObject);
        Zombie[] zombies = FindObjectsOfType<Zombie>();

        this.zombies = new List<Zombie>();

        foreach(Zombie zombie in zombies)
        {
            Destroy(zombie.gameObject);
        }
    }
    public void HeadShot(Transform headPosition)
    {
        UIController.instance.HeadShotFeedback(headPosition);
        score += 1;
    }

    public void RemoveHuman(Transform t)
    {
        Humans.Remove(t);

        if(Humans.Count == 1)
        {
            UIController.instance.backUpButton.Refill();
        }
    }
    #endregion

    #region SpecialAbilities
    public void ThrowGrenade()
    {
        FindAnyObjectByType<CannonController>().ThrowGrenade();
    }
    public void TimeFreeze()
    {
        timeFreeze = true;
        Zombie[] zombies = FindObjectsOfType<Zombie>();

        foreach(Zombie zombie in zombies)
        {
            zombie.SlowDown();
        }
        Physics.gravity /= slowDownFactor;

        StartCoroutine(timeFreezeWait());
        StartCoroutine(effectChange());
    }
    IEnumerator timeFreezeWait()
    {
        yield return new WaitForSeconds(freezeDuration);

        Zombie[] zombies = FindObjectsOfType<Zombie>();

        foreach (Zombie zombie in zombies)
        {
            zombie.ReturnToNormal();
        }
        timeFreeze = false;
        Physics.gravity *= slowDownFactor;
    }
    IEnumerator effectChange()
    {
        ColorGrading colorGrading;

        if(processVolume.profile.TryGetSettings<ColorGrading>(out colorGrading))
        {
            float elapsedTime = 0;
            float target = -100;

            while(elapsedTime < freezeDuration)
            {
                yield return null;
                target += Time.deltaTime * 50 / freezeDuration;
                colorGrading.temperature.value = target;

                elapsedTime += Time.deltaTime;
            }
            while(colorGrading.temperature.value > 0)
            {
                colorGrading.temperature.value -= Time.deltaTime * 25;
                yield return null;
            }
            colorGrading.temperature.value = 0;
        }
    }
    #endregion
}
