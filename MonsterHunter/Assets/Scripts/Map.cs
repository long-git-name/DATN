using UnityEngine;

public class Map : MonoBehaviour
{
   public Transform playerSpawnPoint;
   [SerializeField] private Transform[] spawnPoints;

    public Transform RandomSpawnPoints 
    { 
        get
        {
            if(spawnPoints == null || spawnPoints.Length <= 0) return null;
            
            int randomIndex = Random.Range(0, spawnPoints.Length);
            return spawnPoints[randomIndex];
        }
    }
}
