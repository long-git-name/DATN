using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill System: ", menuName = "MYDEV/Create Skill Data")]
public class SkillSO : ScriptableObject
{
    public float timeTrigger;
    public float cooldownTime;
    public Sprite skillIcon;
    public AudioClip triggerSound;
}
