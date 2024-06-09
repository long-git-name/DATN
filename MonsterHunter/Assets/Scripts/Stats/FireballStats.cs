using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Fireball Stats: ", menuName = "MYDEV/Create Fireball Stats")]
public class FireballStats : SkillSO
{
    [SerializeField] private float speed;
    [SerializeField] private float damage;

    public float Speed { get => speed; set => speed = value; }
    public float Damage { get => damage; set => damage = value; }
}
