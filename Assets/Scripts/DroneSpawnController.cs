using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneSpawnController : MonoBehaviour
{
    public string DroneTutorialKey = "DroneTutorial";

    public GameObject dronePrefab;
    public Transform droneSpawnPosition;

    public bool canSpawn;
    public float waitTime = 10;

    private void Update()
    {
        if(LevelManager.instance.kills % 17 == 0 && canSpawn && LevelManager.instance.kills != 0)
        {
            SpawnDrone();
            canSpawn = false;
        }
    }
    void SpawnDrone()
    {
        if(PlayerPrefs.GetInt(DroneTutorialKey, 0) == 0)
        {
            PlayerPrefs.SetInt(DroneTutorialKey, 1);
            AudioManager.instance.DroneDialogue();
        }

        Instantiate(dronePrefab, droneSpawnPosition.position, droneSpawnPosition.rotation);
        StartCoroutine(spawnWait());
    }
    IEnumerator spawnWait()
    {
        yield return new WaitForSeconds(waitTime);
        canSpawn = true;
    }

}
