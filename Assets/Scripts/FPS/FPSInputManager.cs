using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSInputManager : MonoBehaviour
{
    public static FPSInputManager Instance;

    public FixedTouchField fixedTouchField;
    public FPSController controller;

    public bool reloading, shooting, aiming;

    public float mouseX, mouseY;
    public float horizontal, vertical;
    public float mobileSpeedFactor;

    public bool PC;

    private void Awake()
    {
        Instance = this;

        if (PC)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            StartCoroutine(sf());
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    IEnumerator sf()
    {
        yield return null;
        //UIControllerFPS.instance.fPSCamera.mouseSensitivity *= 100;
    }

    private void Update()
    {
        if (PC)
        {
            shooting = Input.GetMouseButton(0);
            reloading = Input.GetKeyDown(KeyCode.R);
            aiming = Input.GetMouseButtonDown(1);

            mouseX = Input.GetAxis("Mouse X");
            mouseY = Input.GetAxis("Mouse Y");

            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
        }
        else
        {
            mouseX = fixedTouchField.TouchDist.x;
            mouseY = fixedTouchField.TouchDist.y;

            horizontal = UltimateJoystick.GetHorizontalAxis("Movement") * mobileSpeedFactor;
            vertical = UltimateJoystick.GetVerticalAxis("Movement") * mobileSpeedFactor;
        }
    }

    public void Shoot()
    {
        shooting = true;
    }
    public void ShootRelease()
    {
        shooting = false;
    }
    public void Aim()
    {
        controller.activeWeapon.ToggleAim();
    }
    public void Reload()
    {
        reloading = true;
        StartCoroutine(resetReload());
    }
    IEnumerator resetReload()
    {
        yield return null;
        reloading = false;
    }
}
