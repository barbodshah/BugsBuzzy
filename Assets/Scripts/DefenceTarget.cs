using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DefenceTarget : MonoBehaviour
{
    public Health health;

    public GameObject damageParticle;
    public Transform damageParticleSpawnPosition;

    public GameObject WayPointPrefab;
    private GameObject currentWayPoint;

    public Transform player;
    public Transform headPos;

    public Slider healthSlider;
    public AudioSource audioSource;

    private void Awake()
    {
        if(!audioSource) audioSource = GetComponent<AudioSource>();
        healthSlider.maxValue = health.health;
        healthSlider.value = health.health;

        StartCoroutine(spawnWait());
    }
    IEnumerator spawnWait()
    {
        yield return null;
        currentWayPoint = Instantiate(WayPointPrefab, UIController.instance.transform);
        currentWayPoint.GetComponent<TargetWayPoint>().Initialize(headPos, player, health);
    }

    public void Damaged()
    {
        healthSlider.value = health.health;
        AudioManager.instance.MetalHitClip(audioSource);

        GameObject go = Instantiate(damageParticle, damageParticleSpawnPosition.position, damageParticleSpawnPosition.rotation);
        Destroy(go, 2);
    }
    public void Death()
    {
        healthSlider.value = 0;
        Destroy(healthSlider.transform.parent.gameObject, 1);
        Destroy(currentWayPoint);

        LevelManager.instance.Humans.Remove(transform);

        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer renderer in meshRenderers)
        {
            renderer.enabled = false;
        }

        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }
    }
}
