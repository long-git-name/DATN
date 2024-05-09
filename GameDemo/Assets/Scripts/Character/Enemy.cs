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
        var playerStats = player.PlayerStats;

        if(playerStats == null) return;

        float hpUpgrade = enemyStats.hpUp * Helper.GetUpgradeFormula(playerStats.level + 1);
        float damageUpgrade = enemyStats.damageUp * Helper.GetUpgradeFormula(playerStats.level + 1);
        float randomExpBonus = Random.Range(enemyStats.minExpBonus, enemyStats.maxExpBonus);
        
        // Hp, damage và kinh nghiệm của enemy tăng theo cấp độ của player
        CurHp = enemyStats.hp + hpUpgrade;  
        CurDamage = enemyStats.damage + damageUpgrade;
        expBonus = randomExpBonus * Helper.GetUpgradeFormula(playerStats.level + 1); 
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
        var playerStats = player.PlayerStats;

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
            myRigidbody.velocity = playerDir * enemyStats.moveSpeed * Time.deltaTime;
            return;
        }

        myRigidbody.velocity = playerDir * -enemyStats.knockback * Time.deltaTime;
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

