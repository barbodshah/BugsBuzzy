using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetWayPoint : Waypoint
{
    Health health;
    float baseHealth;

    public Slider healthSlider;

    public void Initialize(Transform target, Transform parent, Health health, float triggerDistance = 15)
    {
        base.Initialize(target, parent, triggerDistance);

        this.health = health;
        baseHealth = health.health;

        healthSlider.maxValue = baseHealth;
        healthSlider.value = baseHealth;
    }

    private void Update()
    {
        UpdateLogic();
        healthSlider.value = health.health;
    }
}
