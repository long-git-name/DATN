using UnityEngine;

public class ProjectTile : MonoBehaviour
{
    [Header("Base Setting: ")]
    private float damage;
    [SerializeField] private float speed;
    private float curSpeed;
    [SerializeField] private GameObject bodyHitPrefab;

    public float Damage { get => damage; set => damage = value; }

    private Vector2 lastPosition;
    private RaycastHit2D myRaycastHit;

    private void Start()
    {
        curSpeed = speed;
        RefreshLastPos();
    }

    private void Update()
    {
        transform.Translate(transform.right * curSpeed * Time.deltaTime, Space.World);

        DealDamage();
        RefreshLastPos();
    }

    private void DealDamage()
    {
        Vector2 rayDirection = (Vector2)transform.position - lastPosition;
        myRaycastHit = Physics2D.Raycast(lastPosition, rayDirection, rayDirection.magnitude);

        var collider = myRaycastHit.collider;
        if(!myRaycastHit || collider == null) return;

        if(collider.CompareTag(TagConsts.ENEMY_TAG))
        {
            DealDamageToEnemy(collider);
        }
    }

    private void DealDamageToEnemy(Collider2D collider)
    {
        Actor actorComp = collider.GetComponent<Actor>();

        actorComp?.TakeDamage(damage);

        if(bodyHitPrefab)
        {
            Instantiate(bodyHitPrefab, (Vector3)myRaycastHit.point, Quaternion.identity);
        }
        Destroy(gameObject);
    }

    private void RefreshLastPos()
    {
        lastPosition = (Vector2)transform.position;
    }

    private void OnDisable()
    {
        myRaycastHit = new RaycastHit2D();
    }
}
