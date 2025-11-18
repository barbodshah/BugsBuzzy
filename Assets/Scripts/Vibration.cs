using UnityEngine;

public static class Vibration
{
#if UNITY_ANDROID && !UNITY_EDITOR
    private static AndroidJavaObject vibrator;

    static Vibration()
    {
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject context = currentActivity.Call<AndroidJavaObject>("getApplicationContext");
        AndroidJavaClass vibrationEffectClass = new AndroidJavaClass("android.os.VibrationEffect");

        vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
    }

    public static void Vibrate(long milliseconds)
    {
        if (vibrator == null) return;

        if (AndroidVersion >= 26)
        {
            AndroidJavaClass vibrationEffectClass = new AndroidJavaClass("android.os.VibrationEffect");
            AndroidJavaObject effect = vibrationEffectClass.CallStatic<AndroidJavaObject>(
                "createOneShot", milliseconds, 255); // 255 = default amplitude
            vibrator.Call("vibrate", effect);
        }
        else
        {
            vibrator.Call("vibrate", milliseconds);
        }
    }

    private static int AndroidVersion =>
        new AndroidJavaClass("android.os.Build$VERSION").GetStatic<int>("SDK_INT");
#else
    public static void Vibrate(long milliseconds)
    {
        Handheld.Vibrate(); // fallback
    }
#endif
}
