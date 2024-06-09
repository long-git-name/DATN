using System;
using UnityEngine;

public class ActorStats : Stats
{
    public float Hp { get; set; }
    public float MoveSpeed { get; set; }
    public float Damage { get; set; }
    public float Knockback { get; set; }
    public float KnockBackTime { get; set; }
    public float InvincibleTime { get; set; }

    public override void DefaultStat()
    {
        
    }

    public override bool IsMaxLevel()
    {
        return false;
    }

    public override void Load()
    {
        
    }

    public override void Save()
    {
        
    }

    public override void Upgrade(Action Onsucces = null, Action OnFailed = null)
    {
        
    }
}
