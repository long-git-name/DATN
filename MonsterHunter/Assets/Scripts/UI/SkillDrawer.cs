using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDrawer : MonoBehaviour
{
    [SerializeField] private Transform grid;
    [SerializeField] private SkillButton skillPrefab;
    private Dictionary<SkillType, int> skillCollecteds;
    
    public void DrawSkill()
    {
        Helper.ClearChilds(grid);
        skillCollecteds = SkillManager.Ins.SkillCollecteds;

        if(skillCollecteds == null || skillCollecteds.Count <= 0) return;

        foreach(var skillCollected in skillCollecteds)
        {
            var skillBtnClone = Instantiate(skillPrefab);
            Helper.AssignToRoot(grid, skillBtnClone.transform, Vector3.zero, Vector3.one);
            skillBtnClone.Initialize(skillCollected.Key);
        }
    }
}
