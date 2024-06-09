using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Stats: ", menuName = "MYDEV/Create Enemy Stats")]
public class EnemyStats : ActorStats
{
     [Header("Base Stats: ")]
     [SerializeField] private float defaultHP = 10;
     [SerializeField] private float defaultSpeed = 80;
     [SerializeField] private float defaultDamage = 3;
     [SerializeField] private float defaultKnockback = 100;
     [SerializeField] private float defaultKnockBackTime = 0.5f;
     [SerializeField] private float defaultInvincibleTime = 0.3f;

     [Header("Bonus: ")]
     [SerializeField] private float defaultMinBonus = 3;
     [SerializeField] private float defaultMaxBonus = 5;
     
     [Header("Level Up: ")]
     [SerializeField] private float defaultHPUp = 8;
     [SerializeField] private float defaultDamageUp = 5;

     public float MinExpBonus { get; set; }
     public float MaxExpBonus { get; set; }
     public float HpUp { get; set; }
     public float DamageUp { get; set; }

     public override void DefaultStat()
     {
          Hp = defaultHP;
          MoveSpeed = defaultSpeed;
          Damage = defaultDamage;
          Knockback = defaultKnockback;
          KnockBackTime = defaultKnockBackTime;
          InvincibleTime = defaultInvincibleTime;
          MinExpBonus = defaultMinBonus;
          MaxExpBonus = defaultMaxBonus;
          HpUp = defaultHPUp;
          DamageUp = defaultDamageUp;
     }
}
