using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Stats: ", menuName = "MYDEV/Create Player Stats")]
public class PlayerStats : ActorStats
{
    [Header("Base Stats: ")]
    [SerializeField] private float defaultHP = 25;
    [SerializeField] private float defaultSpeed = 250;
    [SerializeField] private float defaultDamage = 1;
    [SerializeField] private float defaultKnockback = 100;
    [SerializeField] private float defaultKnockBackTime = 0.5f;
    [SerializeField] private float defaultInvincibleTime = 0.3f;

    [Header("Level Up Base: ")]
    [SerializeField] private int defaultLevel = 1;
    [SerializeField] private float defaultMaxLevel = 100;
    [SerializeField] private float defaultExp = 0;
    [SerializeField] private float defaultExpToUpLevel = 25;

    [Header("Level Up: ")]
    [SerializeField] private float defaultHpUp = 10;
    [SerializeField] private float  defaultExpToAfterLvlUp = 50;

    public int Level {get; set;}
    public float MaxLevel {get; set;}
    public float Exp {get; set;}
    public float ExpToUpLevel {get; set;}

    public float HpUp {get; set;}
    public float  ExpToAfterLvlUp {get; set;}

    public override bool IsMaxLevel()
    {
        return Level >= MaxLevel;
    }

    public override void DefaultStat()
    {
        Hp = defaultHP;
        MoveSpeed = defaultSpeed;
        Damage = defaultDamage;
        Knockback = defaultKnockback;
        KnockBackTime = defaultKnockBackTime;
        InvincibleTime = defaultInvincibleTime;
        Level = defaultLevel;
        MaxLevel = defaultMaxLevel;
        Exp = defaultExp;
        ExpToUpLevel = defaultExpToUpLevel;
        HpUp = defaultHpUp;
        ExpToAfterLvlUp = defaultExpToAfterLvlUp;
    }

    public override void Save()
    {
        PlayerStatsData data = new PlayerStatsData
        {
            CurHp = GameManager.Ins.Player.CurHp,
            Hp = Hp,
            Level = Level,
            Exp = Exp,
            ExpToUpLevel = ExpToUpLevel,
        };
        
        string json = JsonUtility.ToJson(data);
        string encryptedJson = AESCrypto.Encrypt(json);
        // Prefs.playerData = JsonUtility.ToJson(data);
        // Debug.Log(encryptedJson);
        Prefs.playerData = encryptedJson;
        
    }

    public override void Load()
    {
        // Debug.Log(PlayerPrefs.HasKey(PrefsConst.PLAYER_DATA_KEY));
        if (PlayerPrefs.HasKey(PrefsConst.PLAYER_DATA_KEY))
        {
            string encryptedJson = Prefs.playerData;
            // Debug.Log(encryptedJson);
            string decryptedJson = AESCrypto.Decrypt(encryptedJson);
            // Debug.Log(decryptedJson);
            PlayerStatsData data = JsonUtility.FromJson<PlayerStatsData>(decryptedJson);
            // Debug.Log(data);
            GameManager.Ins.Player.CurHp = data.CurHp;
            Hp = data.Hp;
            Level = data.Level;
            Exp = data.Exp;
            ExpToUpLevel = data.ExpToUpLevel;
        }
        else
        {
            DefaultStat(); // Nếu không có dữ liệu, đặt lại giá trị mặc định
        }
    }

    public override void Upgrade(Action Onsucces = null, Action OnFailed = null)
    {
        while(Exp >= ExpToUpLevel && !IsMaxLevel())
        {
            Level++;
            Exp -= ExpToUpLevel;
            Hp += HpUp * Helper.GetUpgradeFormula(Level);
            ExpToUpLevel += ExpToAfterLvlUp * Helper.GetUpgradeFormula(Level);

            //Save();

            Onsucces?.Invoke();
        }

        if(Exp < ExpToUpLevel || IsMaxLevel())
        {
            OnFailed?.Invoke();
        }
    }
}
