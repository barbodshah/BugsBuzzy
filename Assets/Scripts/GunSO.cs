using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "GunSO", menuName = "Game/Weapon")]
public class GunSO : ScriptableObject
{
    [Header("Identification")]
    public string GunID;
    public string GunName;

    [Header("Stats")]
    public string MagazineSize;
    public float damage;
    public string reloadSpeed;
    public float fireRate;

    [Header("Purchase Data")]
    public int price;

    [SerializeField] private bool isOwned;

    public UnityEvent buyEvent;

    public bool IsOwned()
    {
        return isOwned || GameController.instance.HasItem(GunID);
    }
    public void BuyItem()
    {
        GameController.instance.BuyItem(GunID,  price);
        buyEvent?.Invoke();
    }

    public void BuySecondWeapon()
    {
        if (FindObjectOfType<MenuTutorialManager>())
        {
            FindObjectOfType<MenuTutorialManager>().stage4();
        }
    }
}
