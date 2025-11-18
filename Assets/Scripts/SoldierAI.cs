using RootMotion.Demos;
using RootMotion.FinalIK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SoldierAI : MonoBehaviour
{
    [Header("References")]
    public NavMeshAgent nav;
    public Animator anim;
    public AimIK aimIK;
    public AimController aimController;

    [Header("Attributes")]
    public float stoppingDistnace;
    public Transform destination;
    public Zombie targetEnemy;
    public float range;
    public float accuracy;
    public Transform torso;

    [Header("Weapon and Damage")]
    public float damage;
    public Transform firePlace;
    public ParticleSystem shootParticle;
    public GameObject bloodPrefab;
    public GameObject hitParticle;
    public float shootTimer;
    public float shootInterval;
    public float weaponForce;

    private Transform m_transform;
    Rigidbody[] rigidbodies;

    AudioSource audioSource;

    private void Awake()
    {
        m_transform = GetComponent<Transform>();
        LevelManager.instance.Humans.Add(transform);

        rigidbodies = GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = true;
        }
        audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        float distnace = Vector3.Distance(m_transform.position, destination.position);

        if(distnace > stoppingDistnace)
        {
            nav.SetDestination(destination.position);
            nav.isStopped = false;

            anim.SetBool("Run", true);
            anim.SetBool("Kneel", false);

            aimController.weight = 0;
        }
        else
        {
            nav.isStopped = true;

            anim.SetBool("Run", false);
            anim.SetBool("Kneel", true);

            FindClosestZombie();

            if (targetEnemy)
            {
                aimController.weight = 1;
                aimController.target = targetEnemy.torsoPos;

                shootTimer += Time.deltaTime;

                if(shootTimer >= shootInterval)
                {
                    shootTimer = 0;
                    shootInterval = 0.3f;

                    shootParticle.Play();

                    Shoot();
                }
            }
            else
            {
                aimController.weight = 0;
                aimController.target = null;
            }
        }
    }
    void Shoot()
    {
        AudioManager.instance.PlaySMGFire(AudioManager.instance.playerAudioSource);
        float temp = Random.Range(0f, 1f);

        if (temp > accuracy) return;

        RaycastHit hit;

        if (Physics.Raycast(firePlace.position, firePlace.forward, out hit, range))
        {
            BulletHit(hit);
        }
    }
    void BulletHit(RaycastHit hit)
    {
        DamagePoint dp = hit.collider.GetComponent<DamagePoint>();
        Rigidbody rb = hit.collider.GetComponent<Rigidbody>();

        if (dp)
        {
            dp.TakeDamage(damage, "AI");
        }
        else
        {
            GameObject particle = Instantiate(hitParticle, hit.point, Quaternion.identity);
            Destroy(particle, 5);
        }

        if (rb)
        {
            if (rb.isKinematic == false)
            {
                Vector3 forceDirection = firePlace.forward;
                rb.AddForce(forceDirection * weaponForce);
            }
        }
    }
    void FindClosestZombie()
    {
        float minDistance = 1000;
        Zombie newTargetEnemy = null;

        foreach(Zombie z in LevelManager.instance.zombies)
        {
            if(Vector3.Distance(m_transform.position, z.transform.position) < minDistance)
            {
                minDistance = Vector3.Distance(z.transform.position, m_transform.position);
                newTargetEnemy = z;
            }
        }
        if(minDistance > range)
        {
            newTargetEnemy = null;
        }
        if(targetEnemy != newTargetEnemy)
        {
            shootInterval = 2;
        }
        targetEnemy = newTargetEnemy;
    }
    public void PlayFootstep()
    {
        AudioManager.instance.PlayFootstep(AudioManager.instance.playerAudioSource);
    }
    public void Death()
    {
        GameObject blood = Instantiate(bloodPrefab, torso.position, torso.rotation);
        Destroy(blood, 5);

        LevelManager.instance.RemoveHuman(m_transform);
        rigidbodies = GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = false;
        }

        nav.enabled = false;
        anim.enabled = false;
        enabled = false;
        aimController.weight = 0;

        Destroy(gameObject, 4);
    }
}
