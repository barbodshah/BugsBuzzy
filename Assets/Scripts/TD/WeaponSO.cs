using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopDownShooter
{
    [CreateAssetMenu(fileName = "WeaponSO", menuName = "ScriptableObjects/Weapon", order = 1)]
    public class WeaponSO : ScriptableObject
    {
        public string weaponID;
        public string weaponName;

        public int damage;
        public float fireRate;
        public float range;
        public bool shootRay;

        public GameObject muzzleEffect;
        public GameObject generaicHitEffect;
        public GameObject bulletPrefab;
    }
}
