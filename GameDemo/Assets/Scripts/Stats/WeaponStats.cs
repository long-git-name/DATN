using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Stats: ", menuName = "MYDEV/Create Weapon Stats")]
public class WeaponStats : Stats
{
    [Header("Base Stat: ")]
    public int bullet;
    public float firerate;
    public float reloadTime;
    public float damage;

    [Header("Upgrade: ")]
    public int level;
    public int maxLevel;
    public int bulletUp;
    public float firerateUp;
    public float reloadTimeUp;
    public float damageUp;
    public int priceToUp;
    public int upPrice;

    [Header("Limit: ")]
    public float minFirerate;
    public float minReloadTime;

    public int BulletUpInfo{ get => bulletUp * (level + 1); }
    public float FirerateUpInfo{ get => firerateUp * Helper.GetUpgradeFormula(level + 1); }
    public float ReloadTimeUpInfo{ get => reloadTimeUp * Helper.GetUpgradeFormula(level + 1); }
    public float DamageUpInfo{ get => damageUp * Helper.GetUpgradeFormula(level + 1); }

    public override bool IsMaxLevel()
    {
        return level >= maxLevel;
    }

    public override void Load()
    {
        if(!string.IsNullOrEmpty(Prefs.weaponData))
        {
            JsonUtility.FromJsonOverwrite(Prefs.weaponData, this);
        }
    }

    public override void Save()
    {
       Prefs.weaponData = JsonUtility.ToJson(this);
    }

    public override void Upgrade(Action Onsucces = null, Action OnFailed = null)
    {
        if(Prefs.IsEnoughCoins(priceToUp) && !IsMaxLevel())
        {
            Prefs.coins -= priceToUp;
            level++;
            bullet += bulletUp * level;
            firerate -= firerateUp * Helper.GetUpgradeFormula(level);
            firerate = Mathf.Clamp(firerate, minFirerate, firerate);

            reloadTime -= reloadTimeUp * Helper.GetUpgradeFormula(level);
            reloadTime = Mathf.Clamp(reloadTime, minReloadTime, reloadTime);

            damage += damageUp * Helper.GetUpgradeFormula(level);
            priceToUp += upPrice * level;

            Save();
            Onsucces?.Invoke();
            return;
        }

        OnFailed?.Invoke();
    }
}
