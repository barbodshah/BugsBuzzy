using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopDownShooter
{
    public class Weapon : MonoBehaviour
    {
        public WeaponSO weaponSO;
        public Transform muzzle;

        public bool canShoot;

        public void AttemptShoot()
        {
            if (!canShoot) return;
            ShootWeapon();
        }

        private void ShootWeapon()
        {
            GameObject particle = Instantiate(weaponSO.muzzleEffect, muzzle.transform.position, muzzle.transform.rotation);
            particle.transform.parent = muzzle.transform;
            Destroy(particle, 2);

            if (weaponSO.shootRay)
            {
                RaycastHit hit;

                if(Physics.Raycast(muzzle.transform.position, muzzle.transform.forward, out hit, weaponSO.range))
                {
                    Collider col = hit.collider;

                    if (col.GetComponent<Character>())
                    {
                        Character character = col.GetComponent<Character>();
                        character.TakeDamage(weaponSO.damage);
                    }
                    else
                    {
                        Vector3 spawnPos = hit.point + hit.normal * 0.01f;
                        GameObject effect = Instantiate(weaponSO.generaicHitEffect, spawnPos, Quaternion.LookRotation(hit.normal));
                        Destroy(effect, 2);
                    }
                }
            }
            canShoot = false;
            StartCoroutine(fireRateReset());
        }
        IEnumerator fireRateReset()
        {
            yield return new WaitForSeconds(weaponSO.fireRate);
            canShoot = true;
        }
    }
}
