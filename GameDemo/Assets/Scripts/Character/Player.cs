using UnityEngine;
using UnityEngine.Events;

public class Player : Actor
{
    [Header("Player Setting:")]
    [SerializeField] private float accelerationSpeed;
    [SerializeField] private float maxMousePosDistance;
    [SerializeField] private Vector2 velocityLimit;
    [SerializeField] private float enemyDetectionRadius;
    [SerializeField] private LayerMask enemyDetectionLayer;

    private float curSpeed;
    private Actor enemyTargeted;
    private Vector2 enemyTargetedDirection;
    private PlayerStats playerStats;

    
    [Header("Player Events:")]
    public UnityEvent OnAddExp;
    public UnityEvent OnLevelUp;
    public UnityEvent OnLostLife;

    public PlayerStats PlayerStats {get => playerStats; private set => playerStats = value; }

    public override void Init()
    {        
        LoadStats();
    }

    private void LoadStats()
    {
        if(statData == null) return;

        playerStats = (PlayerStats)statData;
        playerStats.Load();
        CurHp = playerStats.hp;
    }

    private void Update()
    {
        Move();
    }

    private void FixedUpdate()
    {
        DetectEnemy();
    }

    private void DetectEnemy()
    {
        var enemyFindeds = Physics2D.OverlapCircleAll(transform.position, enemyDetectionRadius, enemyDetectionLayer);
        var finalEnemy = FindNearestEnemy(enemyFindeds);

        if(finalEnemy == null) return;

        enemyTargeted = finalEnemy;

        WeaponHandle();
    }

        //Xoay weapon theo hướng enemy đang tấn công
    private void WeaponHandle()
    {
        if(enemyTargeted == null || weapon == null) return;

        enemyTargetedDirection = enemyTargeted.transform.position - weapon.transform.position; 
        enemyTargetedDirection.Normalize();

        float angle = Mathf.Atan2(enemyTargetedDirection.y, enemyTargetedDirection.x) * Mathf.Rad2Deg;
        weapon.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        
        if(isKnockBack) return;

        weapon.Shoot(enemyTargetedDirection);
    }

    // Tìm enemy gần nhất trong bán kính cho player
    private Actor FindNearestEnemy(Collider2D[] enemyFindeds)
    {
        float minDistance = 0;
        Actor finalEnemy = null;

        if(enemyFindeds == null || enemyFindeds.Length <= 0) return null;
        for(int i = 0; i < enemyFindeds.Length; i++)
        {
            var enemyFinded = enemyFindeds[i];
            if(enemyFinded == null) continue;
            if(finalEnemy == null)
            {
                minDistance = Vector2.Distance(transform.position, enemyFinded.transform.position);
            }
            else
            {
                float distanceTemp = Vector2.Distance(transform.position, enemyFinded.transform.position);
                if(distanceTemp > minDistance) continue;
                minDistance = distanceTemp;                
            }
            finalEnemy = enemyFinded.GetComponent<Actor>();
        }
        return finalEnemy;
    }

    protected override void Move()
    {
        if(IsDead) return;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 movingDir = mousePos - (Vector2)transform.position;
        movingDir.Normalize();
        
        if(!isKnockBack)
        {
            if(Input.GetMouseButton(0))
            {
                Run(mousePos, movingDir);
            }
            else
            {
                BackToIdle();
            }

            return;
        }
        myRigidbody.velocity = enemyTargetedDirection * -statData.knockback * Time.deltaTime;
        myAnimator.SetBool(AnimConst.PLAYER_RUN_PRAM, false);
    }

    // 
    private void BackToIdle()
    {        
        curSpeed -= accelerationSpeed * Time.deltaTime;
        curSpeed = Mathf.Clamp(curSpeed, 0, curSpeed);

        myRigidbody.velocity = Vector2.zero;

        myAnimator.SetBool(AnimConst.PLAYER_RUN_PRAM, false);
    }

    private void Run(Vector2 mousePos, Vector2 movingDir)
    {
        curSpeed += accelerationSpeed * Time.deltaTime;
        curSpeed = Mathf.Clamp(curSpeed, 0, playerStats.moveSpeed);

        float delta = curSpeed * Time.deltaTime;
        float distanceToMousePos = Vector2.Distance(transform.position, mousePos);
        distanceToMousePos = Mathf.Clamp(distanceToMousePos, 0, maxMousePosDistance / 3);
        delta *= distanceToMousePos;

        myRigidbody.velocity = movingDir * delta;

        float velocityLimitX = Mathf.Clamp(myRigidbody.velocity.x, -velocityLimit.x, velocityLimit.x);
        float velocityLimitY = Mathf.Clamp(myRigidbody.velocity.y, -velocityLimit.y, velocityLimit.y);
        
        myRigidbody.velocity = new Vector2(velocityLimitX, velocityLimitY);
       
        myAnimator.SetBool(AnimConst.PLAYER_RUN_PRAM, true);
    }

        //Nhận sát thuong khi bị tấn công
    public override void TakeDamage(float damage)
    {
        if(isInvincible) return;
        
        CurHp -= damage;
        CurHp = Mathf.Clamp(CurHp, 0, playerStats.hp);
        playerStats.Save();

        KnockBack();

        OnTakeDamage?.Invoke();

        if(CurHp > 0) return;

        // Giảm số mạng của Player đang có
        GameManager.Ins.GameOverCheck(OnLostLifeDelegate, OnDeadDelegate);

    }

    private void OnLostLifeDelegate()
    {
        CurHp = playerStats.hp;

        if(stopKnockBackCo != null)
        {
            StopCoroutine(stopKnockBackCo);
        }

        if(invincibleCo != null)
        {
            StopCoroutine(invincibleCo);
        }

        Invincible(2.5f);

        OnLostLife?.Invoke();
    }

    private void OnDeadDelegate()
    {
        CurHp = 0;
        Die();
    }

        //tăng kinh nghiệm khi tiêu diệt enemy
    public void AddExp(float expBonus)
    {
        if(playerStats == null) return;

        playerStats.exp += expBonus;
        playerStats.Upgrade(OnUpgradeStats);

        OnAddExp?.Invoke();

        playerStats.Save();
    }

    private void OnUpgradeStats()
    {
        OnLevelUp?.Invoke();
    }

        //xử lý va chạm giữa enemy và player
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag(TagConsts.ENEMY_TAG))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if(enemy != null)
            {
                TakeDamage(enemy.CurDamage);
            }
        }
        else if(collision.gameObject.CompareTag(TagConsts.COLLECTABLE_TAG))
            {
                Collectable collectable = collision.gameObject.GetComponent<Collectable>();
                collectable.Trigger();
                Destroy(collectable.gameObject);
            }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color32(133, 250, 47, 50);
        Gizmos.DrawSphere(transform.position, enemyDetectionRadius);
    }
}
