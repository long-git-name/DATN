using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballSkillCollect : Collectable
{
    // private SkillDrawer skillDrawer;
    public override void Trigger()
    {
        SkillManager.Ins.AddSkill(SkillType.Fireball, bonus);
        GameManager.Ins.SkillDrawer.DrawSkill();
    }
}
