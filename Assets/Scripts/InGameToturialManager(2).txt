using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTLTMPro;
using Unity.VisualScripting;
using UnityEngine.UIElements;

public class InGameToturialManager : MonoBehaviour
{
    public static InGameToturialManager Instance;

    private const string cameraMovement = "cameraMovement";
    private const string shootTut = "shootTut";
    private const string aimTut = "aimTut";
    private const string settingTut = "settingTut";
    private const string killZombieTut = "killZombieTut";
    private const string reloadTut = "reloadTut";

    [Header("References")]
    public GameObject settingMenu;
    public GameObject settingButton;
    private SpawnController spawnController;
    private CameraController cameraController;
    private CannonController cannonController;
    public GameObject runningZombie;
    public Transform spawnPosition;
    private Zombie currentZombie;
    public float triggerDistance;

    [Header("UI Elements")]
    public RTLTextMeshPro cameraMovementTut;
    public Animator cameraMovementAnimator;
    public GameObject[] cameraMovementGO;
    public RTLTextMeshPro shootTutText;
    public GameObject[] shootTutGO;
    public RTLTextMeshPro aimTutText;
    public GameObject[] aimTutGO;
    public GameObject settingTutGO;
    public GameObject settingTutHand1;
    public GameObject settingTutHand2;
    public RTLTextMeshPro settingTutText;
    public RTLTextMeshPro settingTutText2;
    public GameObject killZombieGO;
    public RTLTextMeshPro killZombieText;
    public RTLTextMeshPro reloadTutText;
    public GameObject []reloadTutGO;
    public RTLTextMeshPro tutorialEndText;
    public GameObject tutorialEndGO;

    [Space]
    public int tutStage;
    public int subTutStage = 0;

    private void Awake()
    {
        Instance = this;

        StartCoroutine(awakeWait());

        spawnController = FindObjectOfType<SpawnController>();
        cameraController = FindObjectOfType<CameraController>();
        cannonController = FindObjectOfType<CannonController>();
    }
    IEnumerator awakeWait()
    {
        yield return null;
        tutStage = GetTutStage();

        if (tutStage == -1)
        {
            //all tutorials finished
            Destroy(this);
        }
        else
        {
            spawnController.enabled = false;

            if (tutStage != -1 && tutStage < 3)
            {
                settingButton.SetActive(false);
            }

            switch (tutStage)
            {
                case 0:
                    stage1();
                    break;
                case 1:
                    stage2();
                    break;
                case 2:
                    stage3();
                    break;
                case 3:
                    stage4();
                    break;
                case 4:
                    stage5();
                    break;
                case 5:
                    stage6();
                    break;
                case 6:
                    break;
                default:
                    break;
            }
        }
    }
    #region StateStart
    void stage1()
    {
        ToggleGoArray(cameraMovementGO, true);
        cameraMovementTut.text = "برای حرکت دوربین، انگشتت رو روی صفحه حرکت بده";
    }
    void stage2()
    {
        ToggleGoArray(shootTutGO, true);
        shootTutText.text = "برای شلیک، اینجا کلیک کن";
    }
    void stage3()
    {
        ToggleGoArray(aimTutGO, true);
        aimTutText.text = "برای نشونه گیری، این دکمه رو بزن";
    }
    void stage4()
    {
        settingButton.SetActive(true);
        settingTutGO.SetActive(true);

        settingTutHand1.SetActive(true);

        settingTutText.text = "برای تغییر تنظیمات کلیک کن";
    }
    void stage5()
    {
        settingTutHand2.SetActive(false);

        killZombieGO.SetActive(true);
        killZombieText.text = "یه زامبی داره به سمتت حمله میکنه! منتظرش باش و وقتی به تو نزدیک شد اونو بکش!";

        GameObject go = Instantiate(runningZombie, spawnPosition.position, Quaternion.identity);
        currentZombie = go.GetComponent<Zombie>();

        currentZombie.GetComponent<Health>().deathEvent.AddListener(ZombieKilled);
    }
    void stage6()
    {
        ToggleGoArray(reloadTutGO, true);
        reloadTutText.text = "برای خشاب گذاری این دکمه رو بزن";
    }
    void tutorialEnd()
    {
        spawnController.enabled = true;

        LevelManager.instance.totalKills = 0;
        LevelManager.instance.kills = 0;

        tutorialEndGO.SetActive(true);
        tutorialEndText.text = "حالا دیگه برای کشتن زامبی ها آماده ای! بزن بریم!";

        StartCoroutine(tutorialEndTimer());
    }
    IEnumerator tutorialEndTimer()
    {
        yield return new WaitForSeconds(3);
        tutorialEndGO.SetActive(false);

        tutStage = -1;
    }
    #endregion
    int GetTutStage()
    {
        if (PlayerPrefs.GetInt(cameraMovement, 0) == 0) return 0;
        if (PlayerPrefs.GetInt(shootTut, 0) == 0) return 1;
        if (PlayerPrefs.GetInt(aimTut, 0) == 0) return 2;
        if (PlayerPrefs.GetInt(reloadTut, 0) == 0) return 3;
        if (PlayerPrefs.GetInt(settingTut, 0) == 0) return 4;
        if (PlayerPrefs.GetInt(killZombieTut, 0) == 0) return 5;
        return -1;
    }

    private void Update()
    {
        if (tutStage == 0)
        {
            float x = cameraController.transform.eulerAngles.x;
            float y = cameraController.transform.eulerAngles.y;

            if (Mathf.Abs(x) >= 20 || Mathf.Abs(y - 90) >= 20)
            {
                tutStage++;

                PlayerPrefs.SetInt(cameraMovement, 1);
                PlayerPrefs.Save();

                ToggleGoArray(cameraMovementGO, false);
                stage2();
            }
        }
        else if (tutStage == 1)
        {
            if (InputManager.Instance.hold || InputManager.Instance.click)
            {
                tutStage++;

                PlayerPrefs.SetInt(shootTut, 1);
                PlayerPrefs.Save();

                ToggleGoArray(shootTutGO, false);
                stage3();
            }
        }
        else if (tutStage == 2)
        {
            if (cannonController.activeWeapon.isAiming())
            {
                subTutStage++;

                if (subTutStage == 1)
                {
                    StartCoroutine(aimStageWait());
                    ToggleGoArray(aimTutGO, false);

                    aimTutText.text = "برای خروج از حالت نشونه گیری همین دکمه رو باید دوباره فشار بدی";
                }
            }
            else if(subTutStage != 0)
            {
                ToggleGoArray(aimTutGO, false);

                tutStage++;
                subTutStage = 0;

                PlayerPrefs.SetInt(aimTut, 1);
                PlayerPrefs.Save();

                stage4();
            }
        }
        else if (tutStage == 3)
        {
            if (subTutStage == 0)
            {
                if (settingMenu.transform.localScale == Vector3.one && Time.timeScale == 0)
                {
                    subTutStage++;

                    settingTutText.gameObject.SetActive(false);
                    settingTutText2.gameObject.SetActive(true);

                    settingTutHand1.SetActive(false);
                    StartCoroutine(settingTutHand());
                    settingTutText2.text = "از اینجا میتونی سرعت چرخش دوربین و بقیه تنظیمات رو تغییر بدی";
                }
            }
            else
            {
                if (settingMenu.transform.localScale == Vector3.zero)
                {
                    PlayerPrefs.SetInt(settingTut, 1);
                    PlayerPrefs.Save();

                    tutStage++;
                    subTutStage = 0;

                    settingTutText.gameObject.SetActive(false);
                    settingTutText2.gameObject.SetActive(false);

                    settingTutGO.SetActive(false);
                    stage5();
                }
            }
        }
        else if (tutStage == 4)
        {
            if (subTutStage == 0)
            {
                if (Vector3.Distance(cameraController.transform.position, currentZombie.transform.position) <= triggerDistance)
                {
                    subTutStage++;
                    killZombieText.text = "یه زامبی داره بهت نزدیک میشه! بهش شلیک کن و خلاصش کن";
                }
            }
        }
        else if(tutStage == 5)
        {
            if (InputManager.Instance.reloading)
            {
                PlayerPrefs.SetInt(reloadTut, 1);
                PlayerPrefs.Save();

                ToggleGoArray(reloadTutGO, false);
                tutStage++;

                tutorialEnd();
            }
        }
    }
    IEnumerator aimStageWait()
    {
        yield return new WaitForSeconds(3f);

        if(tutStage == 2)
            ToggleGoArray(aimTutGO, true);
    }
    IEnumerator settingTutHand()
    {
        Time.timeScale = 1;
        yield return new WaitForSeconds(3);

        if (settingMenu.activeInHierarchy)
        {
            settingTutHand2.SetActive(true);
        }
    }
    void ZombieKilled()
    {
        PlayerPrefs.SetInt(killZombieTut, 1);
        PlayerPrefs.Save();

        killZombieGO.SetActive(false);
        tutStage++;

        stage6();
    }
    void ToggleGoArray(GameObject[] go, bool toggle)
    {
        foreach(GameObject go2 in go)
        {
            go2.SetActive(toggle);
        }
    }
}
