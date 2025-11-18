using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigun : Weapon
{
    MinigunAnimation minigunAnimation;

    [HideInInspector] public float weaponTemp;
    [HideInInspector] public float coolDownTimer;

    public float coolDownTime;
    public float coolDownWait;
    public bool canCoolDown;

    private void Awake()
    {
        minigunAnimation = FindObjectOfType<MinigunAnimation>();
    }

    private void Update()
    {
        coolDownTimer += Time.deltaTime;
        if (coolDownTimer > coolDownTime && canCoolDown)
        {
            weaponTemp -= Time.deltaTime * 0.5f;
        }
        weaponTemp = Mathf.Clamp(weaponTemp, 0, 1);
    }

    public override bool CanShoot()
    {
        return minigunAnimation.currentSpeed >= minigunAnimation.maxSpeed / 2 && weaponTemp < 1 && base.CanShoot();
    }

    public override void Shoot()
    {
        base.Shoot();

        weaponTemp += 0.02f;
        coolDownTimer = 0;

        if (weaponTemp >= 1)
        {
            canCoolDown = false;
            StartCoroutine(CoolDownTimer());
        }
    }
    IEnumerator CoolDownTimer()
    {
        AudioManager.instance.PlayMinigunCooldown();
        yield return new WaitForSeconds(coolDownWait);

        canCoolDown = true;
    }
}
