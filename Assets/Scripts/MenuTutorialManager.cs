using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTLTMPro;
using UnityEngine.EventSystems;

public class MenuTutorialManager : MonoBehaviour
{
    private const string BuyWeaponTutorial = "BuyWeaponTutorial";

    public GunSO secondWeaponSO;

    public RTLTextMeshPro buyWeaponText;
    public GameObject backButton;
    public GameObject clickShopHand;
    public GameObject clickWeaponHand;
    public GameObject buyWeaponHand;
    public GameObject backHand;

    public GameObject weaponMenu;
    public GameObject mainMenu;
    public GameObject buyWeapon;

    int subStage = 0;

    private void Awake()
    {
        StartCoroutine(startTutorial());
    }
    IEnumerator startTutorial()
    {
        yield return null;

        if (PlayerPrefs.GetInt(BuyWeaponTutorial, 0) != 0)
        {
            Destroy(this);

            buyWeaponHand.SetActive(false);
            clickShopHand.SetActive(false);
            clickWeaponHand.SetActive(false);
            buyWeaponText.gameObject.SetActive(false);
            backButton.SetActive(true);
        }
        else
        {
            if (GameController.instance.Gems >= secondWeaponSO.price)
            {
                stage1();
            }
        }
    }
    private void Update()
    {
        if(subStage == 1)
        {
            if (weaponMenu.activeInHierarchy)
            {
                stage2();
                subStage = 2;
            }
        }
        else if (subStage == 2)
        {
            if (buyWeapon.activeInHierarchy && 
                WeaponPreview.Instance.selectedButton == WeaponPreview.Instance.weaponButtons[1])
            {
                subStage = 3;
                stage3();
            }
        }
        else if(subStage == 4)
        {
            if (mainMenu.activeInHierarchy)
            {
                subStage = 5;
                backHand.SetActive(false);
            }
        }
    }
    void stage1()
    {
        subStage = 1;

        clickShopHand.SetActive(true);
        clickWeaponHand.SetActive(false);
        buyWeaponHand.SetActive(false);

        buyWeaponText.gameObject.SetActive(true);
        buyWeaponText.text = "برای خرید اسلحه های بهتر و قوی تر، باید به فروشگاه بری";
    }
    void stage2()
    {
        backButton.SetActive(false);
        buyWeaponText.gameObject.SetActive(false);

        clickShopHand.SetActive(false);
        clickWeaponHand.SetActive(true);
        buyWeaponHand.SetActive(false);
    }
    void stage3()
    {
        clickShopHand.SetActive(false);
        clickWeaponHand.SetActive(false);
        buyWeaponHand.SetActive(true);
    }
    public void stage4()
    {
        subStage = 4;
        backButton.SetActive(true);

        clickShopHand.SetActive(false);
        clickWeaponHand.SetActive(false);
        buyWeaponHand.SetActive(false);

        PlayerPrefs.SetInt(BuyWeaponTutorial, 1);
        PlayerPrefs.Save();

        backHand.SetActive(true);

        FindObjectOfType<BoosterTutorialManager>().stage1();
    }
}
