using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponButton : MonoBehaviour
{
    public GunSO Data;
    public Image image;

    public Sprite selectedSprite;
    public Sprite deselectedSprite;

    public bool selected = false;

    public TMP_Text weaponName;

    private void Awake()
    {
        weaponName.text = Data.GunName.ToUpper();
    }

    public void Select()
    {
        selected = true;
        image.sprite = selectedSprite;

        WeaponPreview.Instance.ButtonClicked(this);
    }
    public void Deselect()
    {
        selected = false;
        image.sprite = deselectedSprite;
    }
}
