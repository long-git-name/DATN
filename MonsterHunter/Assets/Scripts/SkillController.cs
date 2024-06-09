using UnityEngine;
using UnityEngine.Events;

public class SkillController : MonoBehaviour
{
    public SkillType type;
    public SkillSO skillStat;

    protected bool isTriggered;
    private bool isCD;
    protected float triggerTime;
    protected float cdTime;

    public UnityEvent OnTriggerEnter;
    public UnityEvent OnSkillUpdate;
    public UnityEvent OnCooldown;
    public UnityEvent OnStop;
    public UnityEvent<SkillType, int> OnStopWithType;
    public UnityEvent OnCDStop;
    public FireballSpawner fireballSpawner;

    public float cdProgress
    {
        get => cdTime / skillStat.cooldownTime;
    }

    public float triggerProgress
    {
        get => triggerTime / skillStat.timeTrigger;
    }

    public bool IsTriggered { get => isTriggered; }
    public bool IsCD { get => isCD; }
    public float CdTime { get => cdTime; }

    public virtual void LoadSkillStat()
    {
        if(skillStat == null) return;

        cdTime = skillStat.cooldownTime;
        triggerTime = skillStat.timeTrigger;
    }

    public void Trigger()
    {
        if(IsTriggered || isCD) return;
        isTriggered = true;
        isCD = true;
        fireballSpawner.SpawnFireball();
        OnTriggerEnter?.Invoke();
    }

    private void Update()
    {
        CoreHandle();
    }

    private void CoreHandle()
    {
        ReduceTriggerTime();
        ReduceCDTime();
    }

    private void ReduceTriggerTime()
    {
        if(!isTriggered) return;

        triggerTime -= Time.deltaTime;

        if(triggerTime <= 0)
        {
            Stop();
        }
        OnSkillUpdate?.Invoke();
    }

    private void ReduceCDTime()
    {
        if(!isCD) return;

        cdTime -= Time.deltaTime;
        OnCooldown?.Invoke();

        if(cdTime > 0) return;
        isCD = false;
        OnCDStop?.Invoke();

        cdTime = skillStat.cooldownTime;
    }

     public void Stop()
    {
        triggerTime = skillStat.timeTrigger;
        isTriggered = false;

        OnStopWithType?.Invoke(type, 1);
        OnStop?.Invoke();
    }

    public void ForceStop()
    {
        isCD = false;
        isTriggered = false;
        LoadSkillStat();
    }

}
