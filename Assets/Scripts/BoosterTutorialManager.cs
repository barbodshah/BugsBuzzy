using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTLTMPro;

public class BoosterTutorialManager : MonoBehaviour
{
    private const string BuyWeaponTutorial = "BuyWeaponTutorial";
    private const string BuyBoosterTutorial = "BuyBoosterTutorial";

    public GameObject buyBombHand;
    public GameObject buyTimeFreezeHand;
    public RTLTextMeshPro boosterTutorialText;

    private void Awake()
    {
        StartCoroutine(startTutorial());
    }
    IEnumerator startTutorial()
    {
        yield return null;

        if (PlayerPrefs.GetInt(BuyBoosterTutorial, 0) != 0 || PlayerPrefs.GetInt(BuyWeaponTutorial, 0) == 0)
        {
            Destroy(this);
        }
        else
        {
            if (GameController.instance.TimeFreeze != 0 || GameController.instance.Grenades != 0)
            {
                PlayerPrefs.SetInt(BuyBoosterTutorial, 1);
                PlayerPrefs.Save();

                Destroy(this);
            }
            else if (GameController.instance.Gems >= 5)
            {
                stage1();
            }
        }
    }
    private void Update()
    {
        if (GameController.instance.TimeFreeze != 0 || GameController.instance.Grenades != 0)
        {
            PlayerPrefs.SetInt(BuyBoosterTutorial, 1);
            PlayerPrefs.Save();

            Destroy(this);

            buyBombHand.SetActive(false);
            buyTimeFreezeHand.SetActive(false);
            boosterTutorialText.gameObject.SetActive(false);
        }
    }
    void stage1()
    {
        buyBombHand.SetActive(true);
        buyTimeFreezeHand.SetActive(true);
        boosterTutorialText.gameObject.SetActive(true);

        boosterTutorialText.text = "با خرید تقویت کننده ها، زامبی هارو راحت بکش!";
    }
}
