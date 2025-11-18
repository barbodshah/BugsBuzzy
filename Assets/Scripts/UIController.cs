using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Xml;
using RTLTMPro;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    CannonController cannonController;
    CameraController cameraController;

    //public Text ammoText;
    [Header("Stats")]
    public TMP_Text scoreText;
    public TMP_Text endScoreText;
    public TMP_Text endGemText;
    public TMP_Text killText;
    public TMP_Text playerHealth;
    public GameObject ScorePanel;

    [Header("Special Abilities")]
    public TMP_Text grenadeText;
    public TMP_Text timeFreezeText;
    public Slider timeFreezeSlider;
    public ButtonFiller backUpButton;
    public GameObject HighScore;
    public TMP_Text WaveText;
    public RTLTextMeshPro pWaveText;

    [Header("Ammo")]
    public Slider weaponTemp;
    public GameObject ammoPanel;
    public TMP_Text ammo;

    [Header("HUD")]
    public GameObject PlayerDeathScreen;
    public GameObject ShootButton;
    public GameObject Crosshair;
    public bloodSplatter bloodSplatter;

    [Header("Setting")]
    public GameObject SettingPanel;
    public GameObject CloseSettingButton;
    public GameObject SettingButton;
    public Slider sensitivitySlider;
    public Slider volumeSlider;
    public GameObject VibrationCheck;
    public Animator settingAnimator;

    public GameObject RestartButton;
    public GameObject TouchField;

    [Header("Feedback")]
    public GameObject headShotFeedback;
    public GameObject[] deathFeedback;
    public GameObject killFeedbackPrefab;
    public Transform killFeedbackParent;
    public Transform scoreFeedbackParent1;
    public Transform scoreFeedbackParent2;

    [Header("Combo")]
    public TMP_Text mainComboText;
    public TMP_Text ComboText;
    public TMP_Text xText;
    public Animator comboTextAnimator;
    public Animator fastComboAnimator;
    public TMP_Text fastComboText;
    Coroutine fadeOutRoutine;
    Coroutine fastFadeOutRoutine;

    private void Awake()
    {
        instance = this;

        if(!cannonController) cannonController = FindObjectOfType<CannonController>();
        if(!cameraController) cameraController = FindObjectOfType<CameraController>();

        if (InputManager.Instance.pc)
        {
            ShootButton.gameObject.SetActive(false);
        }
        //ScaleUI();

        if (GameController.instance.Vibration == 1) VibrationManager.instance.vibrationToggle = true;
        else VibrationManager.instance.vibrationToggle = false;

        VibrationCheck.SetActive(VibrationManager.instance.vibrationToggle);
    }
    public void WeaponSet()
    {
        if(!cannonController) cannonController = FindObjectOfType<CannonController>();

        if(cannonController.activeWeapon is Minigun)
        {
            weaponTemp.gameObject.SetActive(true);
            ammoPanel.gameObject.SetActive(false);
        }
        else if(cannonController.activeWeapon is ReloadableWeapon)
        {
            weaponTemp.gameObject.SetActive(false);
            ammoPanel.gameObject.SetActive(true);
        }
        else
        {
            weaponTemp.gameObject.SetActive(false);
            ammoPanel.gameObject.SetActive(false);
        }
        sensitivitySlider.value = GameController.instance.Sensitivity;
        volumeSlider.value = GameController.instance.Volume;
    }
    void ScaleUI()
    {
        ShootButton.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        ShootButton.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.height / 4.5f, Screen.height / 4.5f);

        SettingButton.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        SettingButton.GetComponent<RectTransform>().sizeDelta = ShootButton.GetComponent<RectTransform>().sizeDelta * 0.44f;

        weaponTemp.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.height / 4.5f * 1.6f, Screen.height / 4.5f * 0.2f);

        SettingPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.height / 4.5f * 2.7f, Screen.height / 4.5f);
        RestartButton.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.height / 4.5f * 2.7f, Screen.height / 4.5f);
        CloseSettingButton.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.height / 4.5f * 0.33f, Screen.height / 4.5f * 0.33f);

        TouchField.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        TouchField.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height * 0.75f);

        ScorePanel.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.height / 4.5f * 2.79f, Screen.height / 4.5f * 1.2f);
    }
    private void Update()
    {
        //ammoText.text = " Ammo: " + cannonController.activeWeapon.ammo.ToString();
        if (GameController.instance.waveMode)
        {
            scoreText.text = LevelManager.instance.score.ToString() + " / " + Mathf.Max(GameController.instance.WavehighScore, LevelManager.instance.score).ToString();
            endScoreText.text = LevelManager.instance.score.ToString() + " / " + Mathf.Max(GameController.instance.WavehighScore, LevelManager.instance.score).ToString();
        }
        else
        {
            scoreText.text = LevelManager.instance.score.ToString() + " / " + Mathf.Max(GameController.instance.HardcoreHighScore, LevelManager.instance.score).ToString();
            endScoreText.text = LevelManager.instance.score.ToString() + " / " + Mathf.Max(GameController.instance.HardcoreHighScore, LevelManager.instance.score).ToString();
        }

        if (GameController.instance.waveMode)
            killText.text = LevelManager.instance.kills.ToString() + " / " + LevelManager.instance.spawnLimit;
        else
            killText.text = LevelManager.instance.totalKills.ToString();

        if (cannonController.activeWeapon is Minigun)
        {
            Minigun minigun = (Minigun)cannonController.activeWeapon;
            weaponTemp.value = minigun.weaponTemp;
        }
        else if(cannonController.activeWeapon is ReloadableWeapon)
        {
            ReloadableWeapon reloadableWeapon = (ReloadableWeapon)cannonController.activeWeapon;
            ammo.text = reloadableWeapon.currentAmmo.ToString() + " / " + reloadableWeapon.initialAmmo.ToString();
        }

        //WaveText.gameObject.SetActive(LevelManager.instance.inWaveWait && SettingPanel.activeInHierarchy == false);
        pWaveText.gameObject.SetActive(LevelManager.instance.inWaveWait && 
            SettingPanel.activeInHierarchy == false &&
            InGameToturialManager.Instance.tutStage == -1);

        //WaveText.text = GetOrdinal(LevelManager.instance.wave) + " wave coming in " + LevelManager.instance.remainingTime.ToString() + " seconds!";
        pWaveText.text = "موج جدید زامبی ها " + (LevelManager.instance.remainingTime + 1).ToString() + " ثانیه دیگر حمله میکنند";

        timeFreezeText.text = GameController.instance.TimeFreeze.ToString();
        grenadeText.text = GameController.instance.Grenades.ToString();

        playerHealth.text = LevelManager.instance.playerHeath.health.ToString();

        Crosshair.SetActive(!LevelManager.instance.inWaveWait &&
            !SettingPanel.activeInHierarchy &&
            !cannonController.activeWeapon.isAiming());
    }

    public void PlayerDead()
    {
        PlayerDeathScreen.SetActive(true);

        if (GameController.instance.waveMode)
        {
            if (LevelManager.instance.score > GameController.instance.WavehighScore)
            {
                HighScore.SetActive(true);
                GameController.instance.NewWaveHighScore(LevelManager.instance.score);
            }
        }
        else
        {
            if (LevelManager.instance.score > GameController.instance.HardcoreHighScore)
            {
                HighScore.SetActive(true);
                GameController.instance.NewHardcoreHighScore(LevelManager.instance.score);
            }
        }
        StartCoroutine(addCoin());
    }
    IEnumerator addCoin()
    {
        int text = 0;
        endGemText.text = "+" + text;

        while(text < LevelManager.instance.score / 10)
        {
            text++;
            endGemText.text = "+" + text;
            yield return new WaitForSeconds(10 / LevelManager.instance.score);
        }
        text = LevelManager.instance.score / 10;
        endGemText.text = "+" + text;
    }
    public void OpenSetting()
    {
        SettingButton.SetActive(false);
        SettingPanel.SetActive(true);

        //Time.timeScale = 0;

        settingAnimator.Play("Open");
        StartCoroutine(openSetting());
    }
    IEnumerator openSetting()
    {
        yield return new WaitForSeconds(0.25f);
        Time.timeScale = 0;
    }
    public void CloseSetting()
    {
        SettingButton.SetActive(true);
        //SettingPanel.SetActive(false);

        settingAnimator.Play("Close");
        StartCoroutine(closeSetting());
    }
    IEnumerator closeSetting()
    {
        Time.timeScale = LevelManager.instance.globalTimeScale;
        yield return new WaitForSeconds(0.25f);
        SettingPanel.SetActive(false);
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(1);
    }
    public void ReturnLevel()
    {
        SceneManager.LoadScene(0);
    }
    public void ChangeSensitivity()
    {
        cameraController.mouseSensitivity = sensitivitySlider.value;
        GameController.instance.SetSensitivity((int)sensitivitySlider.value);
    }
    public void ChangeVolume()
    {
        AudioManager.instance.volume = volumeSlider.value;
        GameController.instance.SetVolume(volumeSlider.value);
    }
    public void ToggleVibration()
    {
        VibrationCheck.SetActive(!VibrationCheck.activeInHierarchy);
        VibrationManager.instance.vibrationToggle = VibrationCheck.activeInHierarchy;

        if (VibrationManager.instance.vibrationToggle) GameController.instance.SetVibration(1);
        else GameController.instance.SetVibration(0);
    }
    string GetOrdinal(int number)
    {
        if (number <= 0)
            return number.ToString(); // No ordinal for non-positive numbers

        int lastDigit = number % 10;
        int lastTwoDigits = number % 100;

        // Check for 11-13 special case
        if (lastTwoDigits >= 11 && lastTwoDigits <= 13)
            return number + "th";

        // Determine suffix based on last digit
        string suffix = lastDigit switch
        {
            1 => "st",
            2 => "nd",
            3 => "rd",
            _ => "th"
        };

        return number + suffix;
    }
    public void UseGrenade()
    {
        if (GameController.instance.Grenades <= 0) return;
        GameController.instance.AddGrenade(-1);

        LevelManager.instance.ThrowGrenade();
    }
    public void UseTimeFreeze()
    {
        if (GameController.instance.TimeFreeze <= 0) return;
        GameController.instance.AddTimeFreeze(-1);

        timeFreezeSlider.gameObject.SetActive(true);
        timeFreezeSlider.value = 1;

        LevelManager.instance.TimeFreeze();
        StartCoroutine(timeFreezeSliderWait());
    }
    public void HeadShotFeedback(Transform headpos)
    {
        GameObject go = Instantiate(headShotFeedback, transform);
        go.GetComponentInChildren<UIElement>().Init(headpos.transform, new Vector3(0, 0.5f, 0));
    }
    public void ZombieDeathFeedback(Transform headpos)
    {
        GameObject go = Instantiate(deathFeedback[Random.Range(0, deathFeedback.Length)], transform);
        go.GetComponentInChildren<UIElement>().Init(headpos.transform, new Vector3(0, 0.5f, 0));
    }
    public void DeathFeedback()
    {
        GameObject go = Instantiate(killFeedbackPrefab, killFeedbackParent);
        Destroy(go, 1);
    }
    public void ScoreFeedback1(int score)
    {
        GameObject go = Instantiate(killFeedbackPrefab, scoreFeedbackParent1);
        go.GetComponentInChildren<TMP_Text>().text = "+" + score.ToString();

        Destroy(go, 1);
    }
    public void ScoreFeedback2(int score)
    {
        GameObject go = Instantiate(killFeedbackPrefab, scoreFeedbackParent2);
        go.GetComponentInChildren<TMP_Text>().text = "+" + score.ToString();

        Destroy(go, 1);
    }
    public void CallBackup()
    {
        SpawnController.instance.CallBackup();
    }
    IEnumerator timeFreezeSliderWait()
    {
        float elapsedTime = 1;

        while(elapsedTime > 0)
        {
            print(elapsedTime);
            elapsedTime -= Time.deltaTime / LevelManager.instance.freezeDuration;
            timeFreezeSlider.value = elapsedTime;

            yield return null;
        }
        timeFreezeSlider.value = 0;
        timeFreezeSlider.gameObject.SetActive(false);
    }
    #region Combo
    public void FastCombo(int streak)
    {
        if (streak != 2 && streak != 3) return;

        fastComboText.gameObject.SetActive(true);
        fastComboText.color = new Color(fastComboText.color.r, fastComboText.color.g, fastComboText.color.b, 255);
        
        if(streak == 2)
        {
            fastComboText.text = "DOUBLE KILL!";

            ScoreFeedback2(5);
            LevelManager.instance.AddScore(5);
        }
        else
        {
            fastComboText.text = "KILLING SPREE!";

            ScoreFeedback2(10);
            LevelManager.instance.AddScore(5);
        }

        if (fastComboAnimator.GetCurrentAnimatorStateInfo(0).IsTag("scaling"))
            fastComboAnimator.Play("combo_text 0");
        else
            fastComboAnimator.Play("combo_text");

        if(fastFadeOutRoutine != null) StopCoroutine(fastFadeOutRoutine);
        fastFadeOutRoutine = StartCoroutine(FastFadeOutRoutine());
    }
    private IEnumerator FastFadeOutRoutine()
    {
        float time = 2;
        yield return new WaitForSeconds(1);

        Color originalColor1 = fastComboText.color;
        float elapsed = 0f;

        while (elapsed < time)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsed / time);

            fastComboText.color = new Color(originalColor1.r, originalColor1.g, originalColor1.b, alpha);

            elapsed += Time.deltaTime;
            yield return null;
        }

        fastComboText.color = new Color(originalColor1.r, originalColor1.g, originalColor1.b, 0f);
        fastComboText.gameObject.SetActive(false);
    }
    public void UpdateCombo(int streak)
    {
        ScoreFeedback1(2);
        LevelManager.instance.AddScore(2);

        mainComboText.gameObject.SetActive(true);

        ComboText.color = new Color(ComboText.color.r, ComboText.color.g, ComboText.color.b, 255);
        mainComboText.color = new Color(mainComboText.color.r, mainComboText.color.g, mainComboText.color.b, 255);
        xText.color = new Color(xText.color.r, xText.color.g, xText.color.b, 255);

        if(fadeOutRoutine != null)
        {
            StopCoroutine(fadeOutRoutine);
        }

        ComboText.text = streak.ToString();

        if (comboTextAnimator.GetCurrentAnimatorStateInfo(0).IsTag("scaling"))
            comboTextAnimator.Play("combo_text 0");
        else
            comboTextAnimator.Play("combo_text");

        fadeOutRoutine = StartCoroutine(FadeOutRoutine());
    }
    private IEnumerator FadeOutRoutine()
    {
        float time = ComboController.instance.comboBreaker;
        yield return new WaitForSeconds(1);

        Color originalColor1 = ComboText.color;
        Color originalColor2 = mainComboText.color;
        Color originalColor3 = xText.color;

        float elapsed = 0f;

        while (elapsed < time)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsed / time);

            ComboText.color = new Color(originalColor1.r, originalColor1.g, originalColor1.b, alpha);
            mainComboText.color = new Color(originalColor2.r, originalColor2.g, originalColor2.b, alpha);
            xText.color = new Color(originalColor3.r, originalColor3.g, originalColor3.b, alpha);

            elapsed += Time.deltaTime;
            yield return null;
        }

        ComboText.color = new Color(originalColor1.r, originalColor1.g, originalColor1.b, 0f);
        mainComboText.color = new Color(originalColor2.r, originalColor2.g, originalColor2.b, 0f);
        xText.color = new Color(originalColor3.r, originalColor3.g, originalColor3.b, 0f);

        mainComboText.gameObject.SetActive(false);
    }
    #endregion
}
