using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DamagePoint : MonoBehaviour
{
    public Health health;
    public float damageMultiplier;

    public GameObject bloodPrefab;

    public UnityEvent hitEvent;

    private void Awake()
    {
        health = GetComponentInParent<Health>();
    }

    public void TakeDamage(float damage, string shooter="player")
    {
        bool dead = health.health <= 0;

        if(health.health > 0 && health.health <= damage * damageMultiplier)
        {
            hitEvent.Invoke();
            AudioManager.instance.PlayDeathSound(GetComponentInParent<AudioSource>());
        }

        health.AddHealth(-damageMultiplier * damage);

        GameObject blood = Instantiate(bloodPrefab, transform.position, transform.rotation);
        Destroy(blood, 5);

        if (GetComponentInParent<AudioSource>())
        {
            AudioManager.instance.PlayFleshHitClip(GetComponentInParent<AudioSource>());
        }

        //Dont add combo and marker effect if dead
        if (dead) return;

        if (shooter.Equals("player"))
        {
            if(health.health <= 0)
            {
                ComboController.instance.ZombieKilled();
                Crosshair.instance.Kill();
            }
            else
            {
                Crosshair.instance.NoKillHit();
            }
        }
    }
    public void Bleed()
    {
        GameObject blood = Instantiate(bloodPrefab, transform.position, transform.rotation);
        Destroy(blood, 5);
    }

    #region Events
    public void HeadShot()
    {
       if(LevelManager.instance) LevelManager.instance.HeadShot(transform);
       if(FPSLevelManager.Instance) FPSLevelManager.Instance.HeadShot();
    }
    #endregion
}
