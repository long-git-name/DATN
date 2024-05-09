using UnityEngine;

[System.Serializable]
public class CollectableItem
{
    [Range(0f, 1f)]
    public float spawnRate;
    public int amount;
    public Collectable collectablePrefab;
}

public class CollectableManager : Singleton<CollectableManager>
{
   [SerializeField] private CollectableItem[] items;

   public void SpawnItem(Vector3 position)
   {
        if(items == null || items.Length <= 0) return;

        float spawnRateCheck = Random.value;

        for(int i = 0; i < items.Length; i++)
        {
            var item = items[i];
            if(item == null || item.spawnRate < spawnRateCheck) continue;

            CreateCollectable(position, item);
        }
   }

    private void CreateCollectable(Vector3 spawnPosition, CollectableItem collectableItem)
    {
        if(collectableItem == null) return;

        for(int i = 0; i < collectableItem.amount; i++)
        {
            Instantiate(collectableItem.collectablePrefab, spawnPosition, Quaternion.identity);
        }
    }
}
