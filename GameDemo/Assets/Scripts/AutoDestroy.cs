using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float delay;

    private void OnEnable()
    {
        Destroy(gameObject, delay);
    }
}
