using UnityEngine;

[RequireComponent(typeof(Actor))]
public class ActorVisual : MonoBehaviour
{
    private FlashVfx flashVfx;
    protected Actor actor;

    protected virtual void Awake() 
    {
        flashVfx = GetComponent<FlashVfx>();
        actor = GetComponent<Actor>();
    }

        // kích hoạt hiệu ứng khi nhận sát thương
    public virtual void OnTakeDamage()
    {
        if(flashVfx == null || actor == null || actor.IsDead) return;

        flashVfx.Flash(actor.statData.KnockBackTime);
    }
}
