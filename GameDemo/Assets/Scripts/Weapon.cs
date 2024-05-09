using UnityEngine;
using UnityEngine.Events;

public class Weapon : MonoBehaviour
{
    public WeaponStats statData;
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject muzzleFlashPrefab;
    private float curFirerate;
    private int curBullet;
    private float curReloadTime;
    private bool isShooted;
    private bool isReloading;

    [Header("Events: ")]
    public UnityEvent OnShoot;
    public UnityEvent OnReload;
    public UnityEvent OnReloadDone;

    private void Start()
    {
        LoadStats();
    }

    private void LoadStats()
    {
        if(statData == null) return;

        statData.Load();

        curFirerate = statData.firerate;
        curBullet = statData.bullet;
        curReloadTime = statData.reloadTime;
    }

    private void Update()
    {
        ReduceReloadTime();
        ReduceFirerate();
    }

    private void ReduceReloadTime()
    {
        if(!isReloading) return;
        curReloadTime -= Time.deltaTime;

        if(curReloadTime > 0) return;

        LoadStats();
        isReloading = false;
        OnReloadDone?.Invoke();
        curBullet = statData.bullet;
    }

    private void ReduceFirerate()
    {
        if(!isShooted) return;
        curFirerate -= Time.deltaTime;

        if(curFirerate > 0) return;
        curFirerate = statData.firerate;
        isShooted = false;
    }

    public void Shoot(Vector3 targetDirection)
    {
        if(isShooted || shootingPoint == null || curBullet <= 0) return;
        if(muzzleFlashPrefab)
        {
            var muzzleFlashClone = Instantiate(muzzleFlashPrefab, shootingPoint.position, transform.rotation);
            muzzleFlashClone.transform.SetParent(shootingPoint);
        }

        if(bulletPrefab)
        {
            var bulletClone = Instantiate(bulletPrefab, shootingPoint.position, transform.rotation);
            var projectileComp = bulletClone.GetComponent<ProjectTile>();

            if(projectileComp != null)
            {
                projectileComp.Damage = statData.damage;
            }
        }

        curBullet--;
        isShooted = true;
        if(curBullet <= 0)  
        {
            Reload();
        }

        OnShoot?.Invoke();
    }

    private void Reload()
    {
        isReloading = true;

        OnReload?.Invoke();
    }
}
