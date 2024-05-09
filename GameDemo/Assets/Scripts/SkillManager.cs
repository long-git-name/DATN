using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillManager : Singleton<SkillManager>
{
    [SerializeField] private SkillController[] skillControllers;
    private Dictionary<SkillType, int> skillCollecteds;

    public Dictionary<SkillType, int> SkillCollecteds { get => skillCollecteds; }

    protected override void Awake()
    {
        base.Awake();
        Initialize();
    }

    private void Initialize()
    {
        skillCollecteds = new Dictionary<SkillType, int>();
        if(skillControllers == null || skillControllers.Length <= 0) return;

        for(int i = 0; i < skillControllers.Length; i++)
        {
            var skillController = skillControllers[i];
            if(skillController == null) continue;
            skillController.LoadSkillStat();
            skillController.OnStopWithType.AddListener(RemoveSkill);
            skillCollecteds.Add(skillController.type, 0);
        }
    }

    public SkillController GetSkillController(SkillType type)
    {
        var findeds = skillControllers.Where(s => s.type == type).ToArray();
        if(findeds == null || findeds.Length <= 0) return null;
        return findeds[0];
    }

    public int GetSkillAmount(SkillType type)
    {
        if(IsSkillExist(type)) return 0;
        return skillCollecteds[type];
    }

    public void AddSkill(SkillType type, int amount = 1)
    {
        if(IsSkillExist(type))
        {
            var currentAmount = skillCollecteds[type];
            currentAmount += amount;
            skillCollecteds[type] = currentAmount;
        }
        else
        {
            skillCollecteds.Add(type, amount);
        }
    }

    public void RemoveSkill(SkillType type, int amount = 1)
    {
        if(!IsSkillExist(type)) return;

        var currentAmount = skillCollecteds[type];
        currentAmount -= amount;
        skillCollecteds[type] = currentAmount;
        if(currentAmount > 0) return;
        skillCollecteds.Remove(type);
    }

    public bool IsSkillExist(SkillType type)
    {
        return skillCollecteds.ContainsKey(type);
    }

    public void StopSkill(SkillType type)
    {
        var skillController = GetSkillController(type);
        if(skillController == null) return;
        skillController.Stop();
    }

    public void StopAllSkills(SkillType type)
    {
        if(skillControllers == null || skillControllers.Length <= 0) return;
        foreach(var skillController in skillControllers)
        {
            if(skillController == null) continue;
            skillController.ForceStop();
        }
    }
}
