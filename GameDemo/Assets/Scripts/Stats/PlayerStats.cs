using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Stats: ", menuName = "MYDEV/Create Player Stats")]
public class PlayerStats : ActorStats
{
    [Header("Level Up Base: ")]
    public int level;
    public float maxLevel;
    public float exp;
    public float expToUpLevel;

    [Header("Level Up: ")]
    public float hpUp;
    public float  expToAfterLvlUp;

    public override bool IsMaxLevel()
    {
        return level >= maxLevel;
    }

    public override void Load()
    {
        if(!string.IsNullOrEmpty(Prefs.playerData))
        {
            JsonUtility.FromJsonOverwrite(Prefs.playerData, this);
        }
    }

    public override void Save()
    {
        Prefs.playerData = JsonUtility.ToJson(this);
    }

    public override void Upgrade(Action Onsucces = null, Action OnFailed = null)
    {
        while(exp >= expToUpLevel && !IsMaxLevel())
        {
            level++;
            exp -= expToUpLevel;
            hp += hpUp * Helper.GetUpgradeFormula(level);
            expToUpLevel += expToAfterLvlUp * Helper.GetUpgradeFormula(level);

            Save();

            Onsucces?.Invoke();
        }

        if(exp < expToUpLevel || IsMaxLevel())
        {
            OnFailed?.Invoke();
        }
    }
}
