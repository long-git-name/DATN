using System.Collections;
using UnityEngine;

public class FireballSpawner : MonoBehaviour
{
    public GameObject fireballPrefab;

    public float spawnHeight = 5f;
    public float spawnWidth = 10f;

    public void SpawnFireball()
    {
        // Debug.Log("akjskdffdf");
        var enemyTarget = GameManager.Ins.Player.enemyPosition;
        // Debug.Log(enemyTarget.position);
        if(enemyTarget == null) return;

        Vector3 spawnPosition = new Vector3(enemyTarget.position.x + spawnWidth, enemyTarget.position.y + spawnHeight, enemyTarget.position.z);

        // Instantiate fireball
        GameObject fireball = Instantiate(fireballPrefab, spawnPosition, Quaternion.identity);

        // Set the target for the fireball
        FireballSkill fireballScript = fireball.GetComponent<FireballSkill>();
        if (fireballScript != null)
        {
            fireballScript.target = enemyTarget;
        }
    }
}
