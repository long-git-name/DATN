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

    public int CurBullet { get => curBullet; set => curBullet = value; }
    public float CurFirerate { get => curFirerate; set => curFirerate = value; }
    public float CurReloadTime { get => curReloadTime; set => curReloadTime = value; }

    private void Start()
    {
        LoadStats();
    }

    private void LoadStats()
    {
        if(statData == null) return;
        statData.DefaultStat();
        // statData.Load();

        curFirerate = statData.Firerate;
        curBullet = statData.Bullet;
        curReloadTime = statData.ReloadTime;
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

        isReloading = false;
        OnReloadDone?.Invoke();
        curReloadTime = statData.ReloadTime;
        curBullet = statData.Bullet;
    }

    private void ReduceFirerate()
    {
        if(!isShooted) return;
        curFirerate -= Time.deltaTime;

        if(curFirerate > 0) return;
        curFirerate = statData.Firerate;
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
                projectileComp.Damage = statData.Damage;
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
