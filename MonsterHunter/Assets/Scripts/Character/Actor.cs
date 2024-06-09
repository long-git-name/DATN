using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class Actor : MonoBehaviour
{
    [Header("Common: ")]
    public ActorStats statData;

    [LayerList]
    [SerializeField] private int invincibleLayer;
    [LayerList]
    [SerializeField] private int normalLayer;

    public Weapon weapon;

    protected bool isKnockBack;
    protected bool isInvincible;
    private bool isDead;
    private float curHp;
    
    protected Rigidbody2D myRigidbody;
    protected Animator myAnimator;
    protected Coroutine stopKnockBackCo;
    protected Coroutine invincibleCo;

    [Header("Events: ")]
    public UnityEvent OnInit;
    public UnityEvent OnTakeDamage;
    public UnityEvent OnDead;

    public bool IsDead { get => isDead; set => isDead = value; }
    public float CurHp 
    { 
        get => curHp; 
        set => curHp = value;
    }

    protected virtual void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponentInChildren<Animator>();
    }

    protected virtual void Start()
    {
        Init();
        OnInit?.Invoke();
    }

    public virtual void Init()
    {
        
    }

    public virtual void TakeDamage(float damage)
    {
        if(damage < 0 || isInvincible) return;

        curHp -= damage;
        KnockBack();
        if(curHp <= 0)
        {
            Die();
        }

        OnTakeDamage?.Invoke();
    }
    
    protected void KnockBack()
    {
        if(isInvincible || isKnockBack || isDead) return;

        isKnockBack = true;

        stopKnockBackCo = StartCoroutine(StopKnockBack());
    }

    protected virtual void Die()
    {
        isDead = true;
        curHp = 0;
        myRigidbody.velocity = Vector3.zero;

        OnDead?.Invoke();
        Destroy(gameObject);
    }

    protected void Invincible(float invicibleTime)
    {   
        isKnockBack = false;
        isInvincible = true;
        gameObject.layer = invincibleLayer;

        invincibleCo = StartCoroutine(StopInvicible(invicibleTime));
    }

    private IEnumerator StopKnockBack()
    {
        yield return new WaitForSeconds(statData.KnockBackTime);
        Invincible(statData.InvincibleTime);
    }

    private IEnumerator StopInvicible(float invicibleTime)
    {
        yield return new WaitForSeconds(invicibleTime);

        isInvincible = false;
        gameObject.layer = normalLayer;
    }

    protected virtual void Move()
    {

    }
}
