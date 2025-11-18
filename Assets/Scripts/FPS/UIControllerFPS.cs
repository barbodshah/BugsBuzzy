using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIControllerFPS : MonoBehaviour
{
    public static UIControllerFPS instance;

    [Header("References")]
    public Health player;
    public FPSCamera fPSCamera;
    public FixedTouchField touchField;

    [Header("UI Elements")]
    public TMP_Text ammoText;
    public TMP_Text healthText;
    public TMP_Text killCount;
    public GameObject settingButton;
    public GameObject settingPanel;
    public Slider sensitivitySlider;
    public GameObject BuildingPanel;
    public TMP_Text newWaveText;
    public TMP_Text scoreText;
    public GameObject DeathScreen;
    public GameObject crossHair;

    private void Awake()
    {
        instance = this;
        fPSCamera.mouseSensitivity = sensitivitySlider.value;
    }
    private void Update()
    {
        if(FPSController.instance.activeWeapon is ReloadableWeapon)
        {
            ReloadableWeapon reloadableWeapon = (ReloadableWeapon) FPSController.instance.activeWeapon;
            if (reloadableWeapon.inifiniteAmmo)
            {
                ammoText.text = reloadableWeapon.currentAmmo + " / ∞";
            }
            else
            {
                ammoText.text = reloadableWeapon.currentAmmo + " / " + reloadableWeapon.totalAmmo;
            }
        }
        healthText.text = player.health.ToString() + " / 100";
        killCount.text = FPSLevelManager.Instance.waveKilled.ToString() + " / " + FPSLevelManager.Instance.spawnLimit.ToString();

        scoreText.text = FPSLevelManager.Instance.score.ToString();
        crossHair.SetActive(!settingPanel.activeInHierarchy &&
            !newWaveText.gameObject.activeInHierarchy &&
            !FPSController.instance.activeWeapon.isAiming());

        newWaveText.gameObject.SetActive(FPSLevelManager.Instance.inWaveWait && !settingPanel.activeInHierarchy);
        newWaveText.text = "New wave coming in " + FPSLevelManager.Instance.remainingTime.ToString() + " seconds...";
    }
    public void OpenSettingPanel()
    {
        Time.timeScale = 0;

        settingPanel.SetActive(true);
        settingButton.SetActive(false);
    }
    public void CloseSettingPanel()
    {
        Time.timeScale = 1;

        settingPanel.SetActive(false);
        settingButton.SetActive(true);
    }
    public void OpenBuildingPanel()
    {
        BuildingPanel.SetActive(!BuildingPanel.activeInHierarchy);
    }
    public void SensitivityValueChanged()
    {
        fPSCamera.mouseSensitivity = sensitivitySlider.value;
    }
    public void PlayerDeath()
    {
        DeathScreen.SetActive(true);
    }
}
