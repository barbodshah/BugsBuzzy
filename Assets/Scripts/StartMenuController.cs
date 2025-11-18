using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using RTLTMPro;

public class StartMenuController : MonoBehaviour
{
    public GameObject startMenu;

    [Header("Setting")]
    public GameObject SettingMenu;
    public Animator settingAnimator;
    public Slider sensitivitySlider;
    public Slider volumeSlider;
    public GameObject vibrationCheck;
    public bool settingOpen = false;

    [Header("Shop")]
    public TMP_Text timeFreezeAmount;
    public TMP_Text grenadeAmount;
    public TMP_Text gemAmount;
    public TMP_Text gemAmount2;
    public TMP_Text grenadePriceText;
    public TMP_Text timeFreezePriceText;
    public int grenadePrice;
    public int timeFreezePrice;

    [Header("Weapon Select Menu")]
    public GameObject weaponSelectMenu;
    public GameObject weaponPreview;
    public GameObject weaponSelectButton;

    [Header("High Score")]
    public TMP_Text waveHighScore;
    public RTLTextMeshPro waveHighScorePersian;
    public TMP_Text hardcoreHighScore;


    void Start()
    {
        StartCoroutine(startWait());
    }
    private void Update()
    {
        gemAmount.text = GameController.instance.Gems.ToString();
        gemAmount2.text = GameController.instance.Gems.ToString();
    }
    IEnumerator startWait()
    {
        yield return null;

        waveHighScore.text = "HIGHSCORE: " + GameController.instance.WavehighScore.ToString();
        waveHighScorePersian.text = "بیشترین امتیاز: " + GameController.instance.WavehighScore.ToString();
        hardcoreHighScore.text = "Hardcore Highscore: " + GameController.instance.HardcoreHighScore.ToString();

        sensitivitySlider.value = GameController.instance.Sensitivity;
        volumeSlider.value = GameController.instance.Volume;
        vibrationCheck.SetActive(GameController.instance.Vibration == 1);

        SetShopValues();
    }
    void SetShopValues()
    {
        gemAmount.text = GameController.instance.Gems.ToString();
        gemAmount2.text = GameController.instance.Gems.ToString();

        grenadeAmount.text = GameController.instance.Grenades.ToString();
        timeFreezeAmount.text = GameController.instance.TimeFreeze.ToString();
        timeFreezePriceText.text = timeFreezePrice.ToString();
        grenadePriceText.text = grenadePrice.ToString();
    }
    public void SelectGameMode()
    {

    }
    public void StartGame(bool waveMode)
    {
        GameController.instance.waveMode = waveMode;
        SceneManager.LoadScene(1);
    }
    #region Setting
    public void OpenSettingMenu()
    {
        if (settingOpen)
        {
            settingAnimator.Play("Close");
            settingOpen = false;
        }
        else
        {
            settingAnimator.Play("Open");
            settingOpen = true;
        }
    }
    public void CloseSettingMenu()
    {
        settingAnimator.Play("Close");
        settingOpen = false;
    }
    public void ChangeSensitivity()
    {
        GameController.instance.SetSensitivity((int)sensitivitySlider.value);
    }
    public void ChangeVolume()
    {
        GameController.instance.SetVolume(volumeSlider.value);
    }
    public void ToggleVibration()
    {
        if(GameController.instance.Vibration == 1)
        {
            vibrationCheck.SetActive(false);
            GameController.instance.SetVibration(0);
        }
        else
        {
            vibrationCheck.SetActive(true);
            GameController.instance.SetVibration(1);
        }
    }
    #endregion
    #region Shop
    public void BuyGrenade()
    {
        if (GameController.instance.Gems < grenadePrice) return;
        GameController.instance.AddGem(-grenadePrice);
        GameController.instance.AddGrenade(1);

        SetShopValues();
    }
    public void BuyTimeFreeze()
    {
        if (GameController.instance.Gems < grenadePrice) return;
        GameController.instance.AddGem(-grenadePrice);
        GameController.instance.AddTimeFreeze(1);

        SetShopValues();
    }
    public void GetGems()
    {
        GameController.instance.AddGem(1);
        SetShopValues();
    }
    #endregion
    #region WeaponSelectMenu
    public void OpenWeaponSelectMenu()
    {
        weaponPreview.SetActive(true);
        weaponSelectMenu.SetActive(true);
        startMenu.SetActive(false);
    }
    public void CloseWeaponSelectMenu()
    {
        weaponPreview.SetActive(false);
        weaponSelectMenu.SetActive(false);
        startMenu.SetActive(true);
    }
    #endregion
}
