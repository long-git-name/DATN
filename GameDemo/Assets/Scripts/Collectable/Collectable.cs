using System.Collections;
using UnityEngine;

public class Collectable : MonoBehaviour
{
   [SerializeField] private int minBonus;
   [SerializeField] private int maxBonus;
   [SerializeField] private int lifeTime;
   [SerializeField] private int spawnForce;

   private int lifeTimeCounting;
   private Rigidbody2D rb;
   private FlashVfx flashVfx;
   protected int bonus;
   protected Player player;

   private void Awake()
   {
        rb = GetComponent<Rigidbody2D>();
        flashVfx = GetComponent<FlashVfx>();
   }
   
   private void Start()
   {
        lifeTimeCounting = lifeTime;
        player = GameManager.Ins.Player;
        bonus = Random.Range(minBonus, maxBonus) * player.PlayerStats.level;

        Init();
        Explode();
        FlashVfxCompleted();
        StartCoroutine(CountDount());
   }

        // Thời gian Collectable biến mất nếu không nhặt
    private IEnumerator CountDount()
    {
        while(lifeTimeCounting > 0)
        {
            float timeLifeLeftRate = Mathf.Round((float) lifeTimeCounting / lifeTime);
            yield return new WaitForSeconds(0.3f);
            lifeTimeCounting--;
            if(timeLifeLeftRate <= 0.3f && flashVfx != null)
            {
                flashVfx.Flash(lifeTimeCounting);
            }
        }
    }

        //
    private void FlashVfxCompleted()
    {
        if(flashVfx == null) return;
        flashVfx.OnCompleted.RemoveAllListeners();
        flashVfx.OnCompleted.AddListener(OnDestroyCollectable);
    }

    private void OnDestroyCollectable()
    {
        Destroy(gameObject);
    }

        //cách thức Collectable rơi 
    private void Explode()
    {
        if(rb == null) return;

        float randomForceX = Random.Range(-spawnForce, spawnForce);
        float randomForceY = Random.Range(-spawnForce, spawnForce);
        rb.velocity = new Vector2(randomForceX, randomForceY) * Time.deltaTime;
        StartCoroutine(StopMoving());
    }
        //collectable dừng di chuyển sau khoảng thời gian
    private IEnumerator StopMoving()
    {
        yield return new WaitForSeconds(0.8f);
        if(rb != null)
        {
            rb.velocity = Vector2.zero;
        }
    }

    public virtual void Init()
    {
        
    }

    public virtual void Trigger()
    {

    }
}
