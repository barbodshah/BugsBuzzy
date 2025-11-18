using RootMotion.Demos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : MonoBehaviour
{
    public static FPSController instance;

    public Weapon activeWeapon;
    public GameObject hitParticle;

    public float randomnessFactor;

    Camera mainCam;

    public Weapon[] weapons;
    public int currentWeaponIndex;

    private void Awake()
    {
        instance = this;
        mainCam = Camera.main;
    }

    private void Update()
    {
        if(FPSInputManager.Instance.shooting)
        {
            if (activeWeapon.CanShoot())
            {
                Shoot();
            }
        }
        if(FPSInputManager.Instance.aiming)
        {
            activeWeapon.ToggleAim();
        }

        if (FPSInputManager.Instance.reloading)
        {
            if (activeWeapon is ReloadableWeapon)
            {
                ReloadableWeapon reloadableWeapon = (ReloadableWeapon)activeWeapon;
                if(reloadableWeapon.currentAmmo != reloadableWeapon.initialAmmo) reloadableWeapon.Reload();
            }
        }
    }

    #region Shoot
    void Shoot()
    {
        activeWeapon.Shoot();

        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Ray ray = mainCam.ScreenPointToRay(screenCenter);

        if (Physics.Raycast(ray, out RaycastHit hit, activeWeapon.range))
        {
            BulletHit(hit, ray);
        }
    }
    void BulletHit(RaycastHit hit, Ray ray)
    {
        DamagePoint dp = hit.collider.GetComponent<DamagePoint>();
        Rigidbody rb = hit.collider.GetComponent<Rigidbody>();

        if (dp)
        {
            dp.TakeDamage(activeWeapon.damage);
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
                Vector3 forceDirection = ray.direction + Random.insideUnitSphere * randomnessFactor;
                rb.AddForce(forceDirection * activeWeapon.force);
            }
        }
    }
    #endregion
    public void SwitchWeapon()
    {
        activeWeapon.gameObject.SetActive(false);
        activeWeapon.shootParticle.gameObject.SetActive(false);

        currentWeaponIndex++;
        currentWeaponIndex = currentWeaponIndex % weapons.Length;

        activeWeapon = weapons[currentWeaponIndex];
        activeWeapon.gameObject.SetActive(true);
    }
}
