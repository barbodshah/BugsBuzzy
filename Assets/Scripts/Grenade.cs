using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public GameObject particleEffect;
    public float effectiveRange;
    public float grenadeForce;

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
        GameObject go = Instantiate(particleEffect, transform.position, transform.rotation);

        Destroy(go, 5);

        Zombie[] zombies = FindObjectsOfType<Zombie>();

        foreach(Zombie zombie in zombies)
        {
            if(Vector3.Distance(transform.position, zombie.transform.position) < effectiveRange)
            {
                zombie.GetComponent<Health>().Bleed();
                zombie.GetComponent<Health>().AddHealth(-100);
            }
        }

        Rigidbody[] rigidbodies = FindObjectsOfType<Rigidbody>();

        foreach (Rigidbody rb in rigidbodies)
        {
            if (Vector3.Distance(transform.position, rb.transform.position) < effectiveRange)
            {
                //rb.AddExplosionForce(grenadeForce, transform.position, effectiveRange);
                rb.AddForce(rb.transform.position - transform.position * grenadeForce);
            }
        }
    }
}
