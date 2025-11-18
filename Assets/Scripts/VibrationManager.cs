using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibrationManager : MonoBehaviour
{
    public static VibrationManager instance;
    public bool vibrationToggle;

    private void Awake()
    {
        instance = this;
    }

    public void Vibrate()
    {
        if (!vibrationToggle) return;

#if UNITY_ANDROID || UNITY_IOS
        Handheld.Vibrate();
#endif
    }
}
