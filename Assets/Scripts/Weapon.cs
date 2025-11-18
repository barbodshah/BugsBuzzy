using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Weapon : MonoBehaviour
{
    public bool automatic;
    public float fireRate;
    public float range;
    public float damage;

    public bool canShoot;

    public ParticleSystem shootParticle;
    public float force;

    public UnityEvent ShootEvent;

    public Transform firePlace;

    public AimHandler aimHandler;
    public bool canAim;

    public virtual bool CanShoot()
    {
        return canShoot;
    }

    public virtual void Shoot()
    {
        canShoot = false;
        StartCoroutine(FireWait());

        shootParticle.gameObject.SetActive(true);
        shootParticle.Play();

        ShootEvent.Invoke();
    }
    IEnumerator FireWait()
    {
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }
    #region Aim
    public virtual void ToggleAim()
    {
    }
    public bool isAiming()
    {
        if (aimHandler == null) return false;
        return aimHandler.isAiming;
    }
    #endregion
}
