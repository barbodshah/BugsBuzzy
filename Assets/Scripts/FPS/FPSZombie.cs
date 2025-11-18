using RootMotion.Dynamics;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks.Sources;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class FPSZombie : MonoBehaviour
{
    [Header("References")]
    public NavMeshAgent nav;
    public Animator anim;
    public Health targetEnemy;

    Rigidbody[] rigidbodies;

    [Header("Attributes")]
    public bool dead = false;
    public Transform headPos;
    public Transform torsoPos;
    public float stoppingDistance;
    public float damage;

    [Header("Obstacle Detection")]
    public float detectionRadius;
    public LayerMask obstacleLayer;
    public GameObject currentObstacle;
    public bool isDestroyingObstacle;

    [Header("Waypoint")]
    public GameObject WayPointPrefab;
    [HideInInspector] public GameObject currentWayPoint;

    private void Awake()
    {
        transform.parent = null;

        anim = GetComponent<Animator>();
        rigidbodies = GetComponentsInChildren<Rigidbody>();

        if (!nav)
        {
            nav = GetComponent<NavMeshAgent>();
        }

        foreach (Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = true;
        }

        FPSLevelManager.Instance.zombies.Add(this);

        currentWayPoint = Instantiate(WayPointPrefab, UIControllerFPS.instance.transform);
        currentWayPoint.GetComponent<Waypoint>().Initialize(headPos, FPSLevelManager.Instance.Humans[0].transform, 20);
    }
    private void Update()
    {
        if (dead) return;

        if(isDestroyingObstacle && currentObstacle == null)
        {
            currentObstacle = null;
            isDestroyingObstacle = false;
        }

        if (isDestroyingObstacle)
        {
            DestroyObstacle();
        }
        else
        {
            AttackPlayer();
        }
    }
    void DestroyObstacle()
    {
        nav.SetDestination(currentObstacle.transform.position);

        if (Vector3.Distance(currentObstacle.transform.position, transform.position) < stoppingDistance)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack") == false)
            {
                anim.SetTrigger("Attack");
            }
        }
    }
    void AttackPlayer()
    {

        FindClosestEnemy();
        nav.SetDestination(targetEnemy.transform.position);

        if (nav.hasPath == false || nav.isPathStale)
        {
            CheckForObstacle();
        }
        else
        {
            isDestroyingObstacle = false;
        }

        if (Vector3.Distance(targetEnemy.transform.position, transform.position) < stoppingDistance)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack") == false)
            {
                anim.SetTrigger("Attack");
            }
        }
    }
    void FindClosestEnemy()
    {
        float minDistance = 1000;
        Health temp = null;

        for (int i = 0; i < FPSLevelManager.Instance.Humans.Count; i++)
        {
            if(Vector3.Distance(transform.position, FPSLevelManager.Instance.Humans[i].transform.position) < minDistance)
            {
                temp = FPSLevelManager.Instance.Humans[i];
                minDistance = Vector3.Distance(transform.position, FPSLevelManager.Instance.Humans[i].transform.position);
            }
        }
        targetEnemy = temp;
    }
    public void Attack()
    {
        anim.ResetTrigger("Attack");

        if (isDestroyingObstacle)
        {
            if (Vector3.Distance(currentObstacle.transform.position, transform.position) > 3) return;
            currentObstacle.GetComponent<Health>().AddHealth(-damage);
        }
        else
        {
            if (Vector3.Distance(targetEnemy.transform.position, transform.position) > 3) return;
            targetEnemy.AddHealth(-damage);
        }
    }
    void CheckForObstacle()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position + transform.forward, detectionRadius, obstacleLayer);

        if (hits.Length > 0)
        {
            currentObstacle = hits[0].gameObject;
            isDestroyingObstacle = true;
        }
    }
    public void Death()
    {
        if (dead) return;

        foreach (Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = false;
        }
        dead = true;

        nav.enabled = false;
        anim.enabled = false;
        enabled = false;

        FPSLevelManager.Instance.ZombieKilled();
        FPSLevelManager.Instance.zombies.Remove(this);

        Destroy(gameObject, 4);
        Destroy(currentWayPoint);
    }
}
