using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Stats: ", menuName = "MYDEV/Create Enemy Stats")]
public class EnemyStats : ActorStats
{
     [Header("Bonus: ")]
     public float minExpBonus;
     public float maxExpBonus;
     
     [Header("Level Up: ")]
     public float hpUp;
     public float damageUp;
}
