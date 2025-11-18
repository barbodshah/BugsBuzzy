using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ReloadableWeapon : Weapon
{
    public int initialAmmo;
    public int currentAmmo;
    public bool inifiniteAmmo;
    public int totalAmmo;

    public bool reloading = false;
    public UnityEvent ReloadingEvent;

    public Animator animator;
    public float reloadLength;

    bool lastAiming;

    private void Awake()
    {
        currentAmmo = initialAmmo;
    }

    public override bool CanShoot()
    {
        if (currentAmmo == 0 && totalAmmo == 0 && inifiniteAmmo == false) return false;
        if (currentAmmo == 0 && base.CanShoot()) Reload();
        return base.CanShoot() && currentAmmo > 0 && !reloading;
    }
    public override void Shoot()
    {
        base.Shoot();
        currentAmmo--;
    }
    public void Reload()
    {
        if (reloading) return;
        if (totalAmmo == 0) return;

        if (aimHandler.isAiming)
        {
            lastAiming = true;
            ToggleAim();
        }
        ReloadingEvent.Invoke();   
        StartCoroutine(ReloadEvent());
    }
    IEnumerator ReloadEvent()
    {
        reloading = true;
        yield return null;

        animator.Play("Reload");
        yield return new WaitForSeconds(reloadLength);

        reloading = false;

        if (inifiniteAmmo)
        {
            currentAmmo = initialAmmo;
        }
        else
        {
            if(totalAmmo >= initialAmmo - currentAmmo)
            {
                totalAmmo -= (initialAmmo - currentAmmo);
                currentAmmo = initialAmmo;
            }
            else
            {
                currentAmmo += totalAmmo;
                totalAmmo = 0;
            }
        }

        if (lastAiming)
        {
            lastAiming = false;
            ToggleAim();
        }
    }
    public override void ToggleAim()
    {
        base.ToggleAim();
        if (reloading) return;

        aimHandler.ToggleAim();

        if (FindObjectOfType<HandAnimator>())
        {
            FindObjectOfType<HandAnimator>().handAnim.SetBool("Aim", aimHandler.isAiming);
        }
    }
}
