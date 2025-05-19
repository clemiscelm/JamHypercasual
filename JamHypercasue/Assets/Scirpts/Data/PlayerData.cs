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

    public static void InccrementPlayerLevel() => PlayerPrefs.SetInt(ProjectConst.PlayerLevel, PlayerPrefs.GetInt(ProjectConst.PlayerLevel) + 1);
}
