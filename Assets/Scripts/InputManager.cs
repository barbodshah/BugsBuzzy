using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    public bool aiming;
    public bool click;
    public bool hold;
    public bool reloading;

    public float mouseX;
    public float mouseY;

    FixedTouchField fixedTouchField;
    CameraController controller;
    CannonController cannonController;

    public GameObject BoosterTutorial;

    public bool pc;

    private void Awake()
    {
        Instance = this;

        fixedTouchField = FindObjectOfType<FixedTouchField>();
        controller = FindObjectOfType<CameraController>();
        cannonController = FindObjectOfType<CannonController>();

        if (pc)
        {
            fixedTouchField.gameObject.SetActive(false);
            controller.mouseSensitivity = 50;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            Application.targetFrameRate = 60;
        }
        else
        {
            //FindObjectOfType<WeaponSway>().enabled = false;
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (Time.timeScale == 1) Time.timeScale = 0;
            else Time.timeScale = 1;
        }

        if (!pc)
        {
            mouseX = fixedTouchField.TouchDist.x;
            mouseY = fixedTouchField.TouchDist.y;

            if (Input.GetKeyDown(KeyCode.A))
            {
                UIController.instance.UseTimeFreeze();
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                UIController.instance.UseGrenade();
            }
        }
        else
        {
            if (BoosterTutorial.activeInHierarchy) return;

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (UIController.instance.SettingPanel.activeInHierarchy)
                {
                    UIController.instance.CloseSetting();
                    //Cursor.lockState = CursorLockMode.Locked;
                    //Cursor.visible = false;
                }
                else
                {
                    UIController.instance.OpenSetting();
                    Cursor.lockState = CursorLockMode.Confined;
                    Cursor.visible = true;
                }
            }
            if (UIController.instance.SettingPanel.activeInHierarchy) return;

            aiming = Input.GetMouseButtonDown(1);

            mouseX = Input.GetAxis("Mouse X");
            mouseY = Input.GetAxis("Mouse Y");

            click = Input.GetMouseButtonDown(0);
            hold = Input.GetMouseButton(0);

            reloading = Input.GetKeyDown(KeyCode.R);

            if (Input.GetKeyDown(KeyCode.H))
            {
                UIController.instance.UseTimeFreeze();
            }
            if (Input.GetKeyDown(KeyCode.G))
            {
                UIController.instance.UseGrenade();
            }
        }
    }

    public void PointerClicked()
    {
        click = true;
        StartCoroutine(clickReset());
    }
    public void PointerDown()
    {
        hold = true;
    }
    public void PointerUp()
    {
        hold = false;
    }
    public void Aim()
    {
        cannonController.activeWeapon.ToggleAim();
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
    IEnumerator clickReset()
    {
        yield return null;
        click = false;
    }
}
