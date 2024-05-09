using System;
using UnityEngine;

public class ActorStats : Stats
{
    [Header("Base Stats: ")]
    public float hp;
    public float moveSpeed;
    public float damage;
    public float knockback;
    public float knockBackTime;
    public float invincibleTime;

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
