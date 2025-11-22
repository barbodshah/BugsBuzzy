using RootMotion.Dynamics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Zombie : MonoBehaviour
{
    public NavMeshAgent nav;
    public Animator anim;
    //public PuppetMaster master;

    Rigidbody[] rigidbodies; 

    public bool dead = false;

    public Transform headPos;
    public Transform torsoPos;

    public GameObject WayPointPrefab;
    private GameObject currentWayPoint;

    public Transform player;
    public Transform targetEnemy;

    public float zombieDamage;

    private void Awake()
    {
        if (player == null) player = GameObject.FindGameObjectWithTag("MainCamera").transform;
        transform.parent = null;

        anim = GetComponent<Animator>();    
        rigidbodies = GetComponentsInChildren<Rigidbody>();

        if (!nav)
        {
            nav = GetComponent<NavMeshAgent>(); 
        }

        foreach(Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = true;
        }

        LevelManager.instance.zombies.Add(this);
        if (LevelManager.instance.timeFreeze) SlowDown();

        currentWayPoint = Instantiate(WayPointPrefab, UIController.instance.transform);
        currentWayPoint.GetComponent<Waypoint>().Initialize(headPos, player);
    }
    private void Update()
    {
        if (dead) return;

        FindClosestEnemy();
        nav.SetDestination(targetEnemy.position);

        if(Vector3.Distance(targetEnemy.position, transform.position) < 3)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack") == false)
            {
                anim.SetBool("Attack", true);
            }
            nav.isStopped = true;
        }
        else
        {
            anim.SetBool("Attack", false);
            nav.isStopped = false;
        }
    }
    void FindClosestEnemy()
    {
        if(LevelManager.instance.Humans.Count == 1)
        {
            targetEnemy = LevelManager.instance.Humans[0];
            return;
        }
        float minDistnace = 1000;

        foreach(Transform t in LevelManager.instance.Humans)
        {
            if(Vector3.Distance(transform.position, t.position) < minDistnace)
            {
                minDistnace = Vector3.Distance(transform.position, t.position);
                targetEnemy = t;
            }
        }
    }

    #region TimeFreeze
    public void SlowDown()
    {
        nav.speed /= LevelManager.instance.slowDownFactor;
        nav.angularSpeed /= LevelManager.instance.slowDownFactor;

        anim.speed /= LevelManager.instance.slowDownFactor;
    }
    public void ReturnToNormal()
    {
        nav.speed *= LevelManager.instance.slowDownFactor;
        nav.angularSpeed *= LevelManager.instance.slowDownFactor;

        anim.speed *= LevelManager.instance.slowDownFactor;
    }
    #endregion

    #region Event
    public void AttackStart()
    {
        //AudioManager.instance.PlayZombieAttack();
    }
    public void Attack()
    {
        //anim.ResetTrigger("Attack");

        if (Vector3.Distance(targetEnemy.position, transform.position) > 3) return;

        if (targetEnemy.GetComponent<CannonController>())
        {
            //LevelManager.instance.PlayerDead();
            LevelManager.instance.playerHeath.AddHealth(-zombieDamage);
            UIController.instance.bloodSplatter.bleedNow = true;
            AudioManager.instance.PlayPlayerPain();
        }
        else
        {
            //targetEnemy.GetComponent<SoldierAI>().Death();
            targetEnemy.GetComponent<Health>().AddHealth(-zombieDamage);
        }
    }
    public void Death()
    {
        if (dead) return;

        foreach (Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = false;
            rigidbody.gameObject.layer = LayerMask.NameToLayer("DeadBody");
        }
        dead = true;

        nav.enabled = false;
        anim.enabled = false;
        enabled = false;

        LevelManager.instance.ZombieKilledEvent(headPos);
        LevelManager.instance.zombies.Remove(this);

        UIController.instance.DeathFeedback();

        Destroy(gameObject, 4);
        Destroy(currentWayPoint);
    }
    #endregion
}
