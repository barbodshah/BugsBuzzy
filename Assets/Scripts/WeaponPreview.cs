using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using RTLTMPro;

public class WeaponPreview : MonoBehaviour
{
    public static WeaponPreview Instance;

    public WeaponButton[] weaponButtons;
    public GameObject[] weapons;

    public WeaponButton selectedButton;

    public GameObject weaponSelected, buyWeapon, selectWeapon;
    int selectedWeaponIndex = 0;

    public TMP_Text damageText;
    public RTLTextMeshPro pDamageText;
    public TMP_Text fireRateText;
    public RTLTextMeshPro pFireRateText;
    public TMP_Text reloadSpeedText;
    public RTLTextMeshPro pReloadSpeedText;
    public TMP_Text magazineSizeText;
    public RTLTextMeshPro pMagazineSizeText;
    public TMP_Text GemText;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        weaponButtons[GameController.instance.WeaponIndex].Select();
    }

    public void ButtonClicked(WeaponButton button)
    {
        selectedWeaponIndex = 0;
        selectedButton = button;

        damageText.text = "DAMAGE: " + button.Data.damage.ToString();
        pDamageText.text = "قدرت: " + button.Data.damage.ToString();
        fireRateText.text = "FIRE RATE: " + button.Data.fireRate.ToString() + " RPM";
        pFireRateText.text = "سرعت شلیک: " + button.Data.fireRate.ToString() + " گلوله در دقیقه";
        magazineSizeText.text = "MAGAZINE SIZE: " + button.Data.MagazineSize;
        pMagazineSizeText.text = "اندازه خشاب: " + button.Data.MagazineSize;
        reloadSpeedText.text = "RELOAD SPEED: " + button.Data.reloadSpeed;
        GemText.text = GameController.instance.Gems.ToString();

        foreach(WeaponButton weaponButton in weaponButtons)
        {
            if(weaponButton != button)
            {
                weaponButton.Deselect();
            }
        }
        for(int i = 0; i < weapons.Length; i++)
        {
            if (weaponButtons[i] == button)
            {
                weapons[i].SetActive(true);
                selectedWeaponIndex = i;
            }
            else
            {
                weapons[i].SetActive(false);
            }
        }

        buyWeapon.SetActive(false);
        selectWeapon.SetActive(false);
        weaponSelected.SetActive(false);

        if (button.Data.IsOwned())
        {
            if(selectedWeaponIndex == GameController.instance.WeaponIndex)
            {
                weaponSelected.SetActive(true);
            }
            else
            {
                selectWeapon.SetActive(true);
            }
        }
        else
        {
            buyWeapon.SetActive(true);
            buyWeapon.GetComponentInChildren<TMP_Text>().text = button.Data.price.ToString();
        }
    }
    public void SelectWeapon()
    {
        GameController.instance.SetWeaponIndex(selectedWeaponIndex);
        ButtonClicked(selectedButton);
    }
    public void BuyWeapon()
    {
        GameController.instance.BuyItem(selectedButton.Data.GunID, selectedButton.Data.price);
        ButtonClicked(selectedButton);

        SelectWeapon();

        if (selectedWeaponIndex == 1)
        {
            if (FindObjectOfType<MenuTutorialManager>())
            {
                FindObjectOfType<MenuTutorialManager>().stage4();
            }
        }
    }
}
