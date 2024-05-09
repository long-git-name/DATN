using UnityEngine;

public static class Prefs
{
    public static int coins
    {
        set => PlayerPrefs.SetInt(PrefsConst.COIN_KEY, value);
        get => PlayerPrefs.GetInt(PrefsConst.COIN_KEY, 0);
    }

    public static string playerData
    {
        set => PlayerPrefs.SetString(PrefsConst.PLAYER_DATA_KEY, value);
        get => PlayerPrefs.GetString(PrefsConst.PLAYER_DATA_KEY);
    }

    public static string enemyData
    {
        set => PlayerPrefs.SetString(PrefsConst.ENEMY_DATA_KEY, value);
        get => PlayerPrefs.GetString(PrefsConst.ENEMY_DATA_KEY);
    }

    public static string weaponData
    {
        set => PlayerPrefs.SetString(PrefsConst.WEAPON_DATA_KEY, value);
        get => PlayerPrefs.GetString(PrefsConst.WEAPON_DATA_KEY);
    }

    public static bool IsEnoughCoins(int coinToCheck)
    {
        return coins >= coinToCheck;
    }
}