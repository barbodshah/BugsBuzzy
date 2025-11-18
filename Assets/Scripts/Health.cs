using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public DamagePoint torso;

    public UnityEvent deathEvent;
    public float health;

    public bool dead = false;

    public bool destroyAfterDeath;
    public float destroyTimer;

    public void AddHealth(float amount)
    {
        print("ab" + health + " " + amount);
        if (dead) return;

        health += amount;
        print("abc" + health + " " + amount);

        if(health <= 0)
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
