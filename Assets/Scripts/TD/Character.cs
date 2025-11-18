using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TopDownShooter
{
    public class Character : MonoBehaviour
    {
        public float Health;

        public UnityEvent damageEvent;
        public UnityEvent deathEvent;

        public void TakeDamage(float damage)
        {
            Health -= damage;
        }
    }
}
