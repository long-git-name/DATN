using UnityEngine;

public class Enemy : Actor
{
    private Player player;
    private EnemyStats enemyStats;
    private float curDamage;
    private float expBonus;

    public float CurDamage { get => curDamage; private set => curDamage = value; }

    public override void Init()
    {
        player = GameManager.Ins.Player;

        if(statData == null || player == null) return;

        enemyStats = (EnemyStats)statData;
        enemyStats.Load();

        StatsCaculate();
        
        OnDead.AddListener(OnSpawnCollectable);
        OnDead.AddListener(OnAddExpToPlayer);
    }

        //Tăng chỉ số của enemy
    private void StatsCaculate()
    {
        statData.DefaultStat();
        var playerStats = player.PlayerStats;

        if(playerStats == null) return;

        float hpUpgrade = enemyStats.HpUp * Helper.GetUpgradeFormula(playerStats.Level + 1);
        float damageUpgrade = enemyStats.DamageUp * Helper.GetUpgradeFormula(playerStats.Level + 1);
        float randomExpBonus = Random.Range(enemyStats.MinExpBonus, enemyStats.MaxExpBonus);
        
        // Hp, damage và kinh nghiệm của enemy tăng theo cấp độ của player
        CurHp = enemyStats.Hp + hpUpgrade;  
        CurDamage = enemyStats.Damage + damageUpgrade;
        expBonus = randomExpBonus * Helper.GetUpgradeFormula(playerStats.Level + 1); 
    }

    protected override void Die()
    {
        base.Die();

        myAnimator.SetTrigger(AnimConst.ENEMY_DEAD_PRAM);
    }

    private void OnSpawnCollectable()
    {
        CollectableManager.Ins.SpawnItem(transform.position);
    }

    private void OnAddExpToPlayer()
    {
        //var playerStats = player.PlayerStats;

        player.AddExp(expBonus);
    }

    private void FixedUpdate()
    {
        Move();
    }

        //phương thức di chuyển cho enemy
    protected override void Move()
    {
        if(IsDead || player == null) return;

        Vector2 playerDir = player.transform.position - transform.position;
        playerDir.Normalize();

        if(!isKnockBack)
        {
            FlipEnemy(playerDir);
            myRigidbody.velocity = enemyStats.MoveSpeed * Time.deltaTime * playerDir;
            return;
        }

        myRigidbody.velocity = -enemyStats.Knockback * Time.deltaTime * playerDir;
    }

        //
    private void FlipEnemy(Vector2 playerDir)
    {
       if(playerDir.x > 0)
        {
            if(transform.localScale.x > 0) return;
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
        else if(playerDir.x < 0)
        {
            if(transform.localScale.x < 0) return;
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }

    }

    private void OnDisable()
    {
        OnDead.RemoveListener(OnSpawnCollectable);
        OnDead.RemoveListener(OnAddExpToPlayer);
    }
}

