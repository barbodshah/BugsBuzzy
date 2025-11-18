using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopDownShooter
{
    public class PlayerController : MonoBehaviour
    {
        public Weapon activeWeapon;

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                activeWeapon.AttemptShoot();
            }
        }
    }
}
