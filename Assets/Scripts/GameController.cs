using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    private const string savedWaveHighScore = "WaveHighScore";
    private const string savedHardcoreHighScore = "HardcoreHighScore";
    private const string savedGrenades = "Grenades";
    private const string savedTimeFreeze = "Freeze";
    private const string savedGems = "Gems";
    private const string savedWeaponIndex = "selectedWeapon";
    private const string savedSensitivity = "Sensitivity";
    private const string savedVibration = "Vibration";
    private const string savedVolume = "Volume";

    public int WavehighScore;
    public int HardcoreHighScore;

    public int Grenades;
    public int TimeFreeze;
    public int Gems;
    public int WeaponIndex;
    public int Sensitivity;
    public int Vibration;
    public float Volume;

    public bool waveMode;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }

        LoadPlayerPrefs();
    }
    void LoadPlayerPrefs()
    {
        LoadOrInitialize(savedWaveHighScore, ref WavehighScore);
        LoadOrInitialize(savedHardcoreHighScore, ref HardcoreHighScore);
        LoadOrInitialize(savedGems, ref Gems);
        LoadOrInitialize(savedTimeFreeze, ref TimeFreeze);
        LoadOrInitialize(savedGrenades, ref Grenades);
        LoadOrInitialize(savedWeaponIndex, ref WeaponIndex, 0);
        LoadOrInitialize(savedSensitivity, ref Sensitivity, 50);
        LoadOrInitialize(savedVibration, ref Vibration, 1);
        LoadOrInitialize(savedVolume, ref Volume, 1f);
    }
    private void LoadOrInitialize(string key, ref int variable, int defaultValue=0)
    {
        if (PlayerPrefs.HasKey(key))
        {
            variable = PlayerPrefs.GetInt(key);
        }
        else
        {
            variable = defaultValue;
            PlayerPrefs.SetInt(key, variable);
            PlayerPrefs.Save();
        }
    }
    private void LoadOrInitialize(string key, ref float variable, float defaultValue = 0)
    {
        if (PlayerPrefs.HasKey(key))
        {
            variable = PlayerPrefs.GetFloat(key);
        }
        else
        {
            variable = defaultValue;
            PlayerPrefs.SetFloat(key, variable);
            PlayerPrefs.Save();
        }
    }
    public void NewWaveHighScore(int newHighScore)
    {
        WavehighScore = newHighScore;
        PlayerPrefs.SetInt(savedWaveHighScore, newHighScore);
    }
    public void NewHardcoreHighScore(int newHighScore)
    {
        HardcoreHighScore = newHighScore;
        PlayerPrefs.SetInt(savedHardcoreHighScore, newHighScore);
    }

    #region GetterSetter
    public void AddGem(int amount)
    {
        Gems += amount;
        PlayerPrefs.SetInt(savedGems, Gems);
        PlayerPrefs.Save();
    }
    public void AddGrenade(int amount)
    {
        Grenades += amount;
        PlayerPrefs.SetInt(savedGrenades, Grenades);
        PlayerPrefs.Save();
    }
    public void AddTimeFreeze(int amount)
    {
        TimeFreeze += amount;
        PlayerPrefs.SetInt(savedTimeFreeze, TimeFreeze);
        PlayerPrefs.Save();
    }
    public void SetWeaponIndex(int index)
    {
        WeaponIndex = index;
        PlayerPrefs.SetInt(savedWeaponIndex, WeaponIndex);
        PlayerPrefs.Save();
    }
    public void SetSensitivity(int value)
    {
        Sensitivity = value;
        PlayerPrefs.SetInt(savedSensitivity, Sensitivity);
        PlayerPrefs.Save();
    }
    public void SetVibration(int value)
    {
        Vibration = value;
        PlayerPrefs.SetInt(savedVibration, Vibration);
        PlayerPrefs.Save();
    }
    public void SetVolume(float value)
    {
        Volume = value;
        PlayerPrefs.SetFloat(savedVolume, Volume);
        PlayerPrefs.Save();
    }
    public bool HasItem(string itemId)
    {
        return PlayerPrefs.GetInt(itemId, 0) == 1;
    }
    public void BuyItem(string itemId, int price)
    {
        if(Gems < price)
        {
            print("Not enough money");
            return;
        }
        AddGem(-price);
        AddItem(itemId);
    }
    public void AddItem(string itemId)
    {
        PlayerPrefs.SetInt(itemId, 1);
    }
    public void RemoveItem(string itemId)
    {
        PlayerPrefs.SetInt(itemId, 0);
    }
    #endregion
}
