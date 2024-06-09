using UnityEngine;

public class FireballSkill : MonoBehaviour
{
    public Transform target;
    private float speed = 10;
    private float damage = 50;
    
    private void Start()
    {
        Destroy(gameObject, 5f);
    }

    private void Update()
    {
        if (target != null)
        { 
            Vector3 direction = (target.transform.position - transform.transform.position).normalized;
            transform.Translate(speed * Time.deltaTime * direction, Space.World);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(TagConsts.ENEMY_TAG))
        {
            Actor enemy = other.gameObject.GetComponent<Actor>();
            enemy.TakeDamage(damage);
            Destroy(gameObject, 0.15f);
        }
    }
}
