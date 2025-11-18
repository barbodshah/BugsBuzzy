using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRepo : MonoBehaviour
{
    public static WeaponRepo Instance;

    public GunSO[] guns;

    private void Awake()
    {
        Instance = this;
    }
}
