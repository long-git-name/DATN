using System;
using UnityEngine;

public abstract class Stats : ScriptableObject
{
    public abstract void Save();
    public abstract void Load();
    public abstract void DefaultStat();
    public abstract void Upgrade(Action Onsucces = null, Action OnFailed = null);
    public abstract bool IsMaxLevel();
}