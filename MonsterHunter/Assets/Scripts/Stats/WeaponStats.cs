using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Stats: ", menuName = "MYDEV/Create Weapon Stats")]
public class WeaponStats : Stats
{
    [Header("Base Stat: ")]
    [SerializeField] private int defaultBullet = 10;
    [SerializeField] private float defaultFirerate = 1;
    [SerializeField] private float defaultReloadTime = 5;
    [SerializeField] private float defaultDamage = 3;

    [Header("Upgrade: ")]
    [SerializeField] private int defaultLevel = 1;
    [SerializeField] private int defaultMaxLevel = 1000;
    [SerializeField] private int defaultBulletUp = 1;
    [SerializeField] private float defaultFirerateUp = 0.2f;
    [SerializeField] private float defaultReloadTimeUp = 0.1f;
    [SerializeField] private float defaultDamageUp = 0.25f;
    [SerializeField] private int defaultPriceToUp = 25;
    [SerializeField] private int defaultUpPrice = 25;

    [Header("Limit: ")]
    [SerializeField] private float defaultMinFirerate = 0.1f;
    [SerializeField] private float defaultMinReloadTime = 0.001f;

    public int Bullet { get; set; }
    public float Firerate { get; set; }
    public float ReloadTime { get; set; }
    public float Damage { get; set; }
    public int Level { get; set; }
    public int MaxLevel { get; set; }
    public int BulletUp { get; set; }
    public float FirerateUp { get; set; }
    public float ReloadTimeUp { get; set; }
    public float DamageUp { get; set; }
    public int PriceToUp { get; set; } 
    public int UpPrice { get; set; } 
    public float MinFirerate { get; set; } 
    public float MinReloadTime { get; set; }

    public int BulletUpInfo{ get => BulletUp * (Level + 1); }
    public float FirerateUpInfo{ get => FirerateUp * Helper.GetUpgradeFormula(Level + 1); }
    public float ReloadTimeUpInfo{ get => ReloadTimeUp * Helper.GetUpgradeFormula(Level + 1); }
    public float DamageUpInfo{ get => DamageUp * Helper.GetUpgradeFormula(Level + 1); }

    public override void DefaultStat()
    {
        Bullet = defaultBullet;
        Firerate = defaultFirerate;
        ReloadTime = defaultReloadTime;
        Damage = defaultDamage;
        Level = defaultLevel;
        MaxLevel = defaultMaxLevel;
        BulletUp = defaultBulletUp;
        FirerateUp = defaultFirerateUp;
        ReloadTimeUp = defaultReloadTimeUp;
        DamageUp = defaultDamageUp;
        PriceToUp = defaultPriceToUp;
        UpPrice = defaultUpPrice;
        MinFirerate = defaultMinFirerate;
        MinReloadTime= defaultMinReloadTime;
    }

    public override bool IsMaxLevel()
    {
        return Level >= MaxLevel;
    }

    public override void Save()
    {
        WeaponStatsData data = new WeaponStatsData
        {
            CurBullet = GameManager.Ins.Player.weapon.CurBullet,
            CurFirerate = GameManager.Ins.Player.weapon.CurFirerate,
            CurReloadTime = GameManager.Ins.Player.weapon.CurReloadTime,
            Bullet = Bullet,
            Firerate = Firerate,
            Level = Level,
            ReloadTime = ReloadTime,
            Damage = Damage,
            PriceToUp = PriceToUp
        };

        Prefs.weaponData = JsonUtility.ToJson(data);
    }

    public override void Load()
    {
        if (PlayerPrefs.HasKey(PrefsConst.WEAPON_DATA_KEY))
        {
            string json = Prefs.weaponData;
            WeaponStatsData data = JsonUtility.FromJson<WeaponStatsData>(json);

            GameManager.Ins.Player.weapon.CurBullet = data.CurBullet;
            GameManager.Ins.Player.weapon.CurFirerate = data.CurFirerate;
            GameManager.Ins.Player.weapon.CurReloadTime = data.CurReloadTime;
            Bullet = data.Bullet;
            Level = data.Level;
            Firerate = data.Firerate;
            ReloadTime = data.ReloadTime;
            Damage = data.Damage;
            PriceToUp = data.PriceToUp;
        }
        else
        {
            DefaultStat(); // Nếu không có dữ liệu, đặt lại giá trị mặc định
        }
    }

    public override void Upgrade(Action Onsucces = null, Action OnFailed = null)
    {
        if(Prefs.IsEnoughCoins(PriceToUp) && !IsMaxLevel())
        {
            Prefs.coins -= PriceToUp;
            Level++;
            Bullet += BulletUp * Level;
            Firerate -= FirerateUp * Helper.GetUpgradeFormula(Level);
            Firerate = Mathf.Clamp(Firerate, MinFirerate, Firerate);

            ReloadTime -= ReloadTimeUp * Helper.GetUpgradeFormula(Level);
            ReloadTime = Mathf.Clamp(ReloadTime, MinReloadTime, ReloadTime);

            Damage += DamageUp * Helper.GetUpgradeFormula(Level);
            PriceToUp += UpPrice * Level;

            //Save();
            Onsucces?.Invoke();
            return;
        }

        OnFailed?.Invoke();
    }
}
