using NaughtyAttributes;
using UnityEngine;

public static class PlayerData
{
    public static int GetPlayerCurrentLevel()
    {
        if (!PlayerPrefs.HasKey(ProjectConst.PlayerLevel))
        {
            PlayerPrefs.SetInt(ProjectConst.PlayerLevel, 1);
        }
        return PlayerPrefs.GetInt(ProjectConst.PlayerLevel);
    }
    public static int GetSoftCurrency()
    {
        if (!PlayerPrefs.HasKey(ProjectConst.SoftCurrency))
        {
            PlayerPrefs.SetInt(ProjectConst.SoftCurrency, 0);
        }
        return PlayerPrefs.GetInt(ProjectConst.SoftCurrency);
    }
    public static int GetHardCurrency()
    {
        if (!PlayerPrefs.HasKey(ProjectConst.HardCurrency))
        {
            PlayerPrefs.SetInt(ProjectConst.HardCurrency, 0);
        }
        return PlayerPrefs.GetInt(ProjectConst.HardCurrency);
    }
    
    public static void IncriseHardCurrency(int amount) => PlayerPrefs.SetInt(ProjectConst.HardCurrency, PlayerPrefs.GetInt(ProjectConst.HardCurrency) + amount);
    public static void DeacriseHardCurrency(int amount) => PlayerPrefs.SetInt(ProjectConst.HardCurrency, PlayerPrefs.GetInt(ProjectConst.HardCurrency) + amount);
    public static void DeacriseSoftCurrency(int amount) => PlayerPrefs.SetInt(ProjectConst.SoftCurrency, PlayerPrefs.GetInt(ProjectConst.SoftCurrency) + amount);
    public static void IncriseSoftCurrency(int amount) => PlayerPrefs.SetInt(ProjectConst.SoftCurrency, PlayerPrefs.GetInt(ProjectConst.SoftCurrency) + amount);

    public static void InccrementPlayerLevel() => PlayerPrefs.SetInt(ProjectConst.PlayerLevel, PlayerPrefs.GetInt(ProjectConst.PlayerLevel) + 1);
}
