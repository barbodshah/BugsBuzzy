using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public DamagePoint torso;

    public UnityEvent deathEvent, damageEvent;
    public float health;

    public bool dead = false;

    public bool destroyAfterDeath;
    public float destroyTimer;

    public void AddHealth(float amount)
    {
        if (dead) return;

        health += amount;
        damageEvent.Invoke();

        if (health <= 0)
        {
            Death();
        }
    }
    public void Bleed()
    {
        if (dead) return;
        torso.Bleed();
    }
    void Death()
    {
        if (dead) return;

        deathEvent.Invoke();
        dead = true;

        if (destroyAfterDeath) Destroy(gameObject, destroyTimer);
    }
}
