using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    public Weapon activeWeapon;

    public Weapon[] weapons;

    public GameObject Grenade;
    public float grenadeForce;

    public GameObject hitParticle;

    public float randomnessFactor;

    Camera mainCam;

    private void Awake()
    {
        mainCam = Camera.main;
        StartCoroutine(SetActiveWeapon());
    }
    IEnumerator SetActiveWeapon()
    {
        yield return null;
        int activeWeaponIndex = GameController.instance.WeaponIndex;

        for(int i = 0; i < weapons.Length; i++)
        {
            if(i == activeWeaponIndex)
            {
                weapons[i].gameObject.SetActive(true);
                activeWeapon = weapons[i];
            }
            else
            {
                weapons[i].gameObject.SetActive(false);
            }
        }
        UIController.instance.WeaponSet();
    }
    private void Update()
    {
        if (activeWeapon.automatic)
        {
            if (InputManager.Instance.hold)
            {
                if (activeWeapon.CanShoot())
                {
                    Shoot();
                }
            }
        }
        else
        {
            if (InputManager.Instance.click)
            {
                if (activeWeapon.CanShoot())
                {
                    Shoot();
                }
            }
        }
        if (InputManager.Instance.aiming)
        {
            activeWeapon.ToggleAim();
        }
        if (InputManager.Instance.reloading)
        {
            if (activeWeapon is ReloadableWeapon)
            {
                ReloadableWeapon reloadableWeapon = (ReloadableWeapon)activeWeapon;
                if (reloadableWeapon.currentAmmo != reloadableWeapon.initialAmmo) reloadableWeapon.Reload();
            }
        }
    }
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
            Crosshair.instance.ActivateMarker();
        }
        else
        {
            GameObject particle = Instantiate(hitParticle, hit.point, Quaternion.identity);
            Destroy(particle, 5);
        }

        if (rb)
        {
            if(rb.isKinematic == false)
            {
                Vector3 forceDirection = ray.direction + Random.insideUnitSphere * randomnessFactor;
                rb.AddForce(forceDirection * activeWeapon.force);
            }
        }
    }

    public void ThrowGrenade()
    {
        GameObject g = Instantiate(Grenade, activeWeapon.firePlace.position, activeWeapon.firePlace.rotation);
        g.GetComponent<Rigidbody>().AddForce(transform.forward * grenadeForce);
    }
}
