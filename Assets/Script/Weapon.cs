using System;
using UnityEngine;
using UnityEngine.Events;

public class Weapon : MonoBehaviour
{
    public int weaponCurrentLv = 1;
    public int maxLv = 1;
    public int currentBullet;
    public int currentDamage;
    public int maxBullet;
    public float reloadTimer;
    public float fireRateTimer;

    public float currentReloadTime;
    public float currentFirerate;
    public int currentPrice;
    public WeaponStats weaponStats;

    public UnityEvent OnShoot;
    public UnityEvent OnReload;
    public UnityEvent OnReloadDone;

    public GameObject muzzlePrefab;
    public GameObject bulletPrefab;
    public Transform firePos;
    bool isReloading = true;

    void OnEnable()
    {
        EventManager.OnStartGame += ResetData;
    }
    void OnDisable()
    {
        EventManager.OnStartGame -= ResetData;
    }
    void Update()
    {
        ReduceReloadTime();
        ReduceFireRateTime();
    }

    void ResetData()
    {
        Reload();
        LoadStat();
    }

    public bool IsMaxLevel()
    {
        return weaponCurrentLv >= maxLv;
    }


    public int bulletUpInfo { get => weaponStats.bulletUp * (weaponCurrentLv + 1); }
    public int damageUpInfo { get =>  (int)(weaponStats.damageUp * Helper.GetQualityLevelUp(weaponCurrentLv + 1)); }
    public int priceInfo { get =>  (int)(weaponStats.qualityPriceWhenLevelUp * Helper.GetQualityLevelUp(weaponCurrentLv + 1)); }
    public float fireRateUpInfo { get => weaponStats.fireRateUp * Helper.GetQualityLevelUp(weaponCurrentLv + 1); }
    public float reloadTimeUpInfo { get => weaponStats.fireRateUp * Helper.GetQualityLevelUp(weaponCurrentLv + 1); }
    private void ReduceReloadTime()
    {
        reloadTimer -= Time.deltaTime;
        if (reloadTimer < 0 && !isReloading)
        {
            OnReloadDone?.Invoke();
            isReloading = true;
            currentBullet = maxBullet;
        }
    }

    private void ReduceFireRateTime()
    {
        fireRateTimer -= Time.deltaTime;
        if (fireRateTimer > 0) return;
    }
    private void LoadStat()
    {
        maxBullet = weaponStats.bullet;
        currentBullet = weaponStats.bullet;

        currentReloadTime = weaponStats.reloadTime;
        currentFirerate = weaponStats.fireRate;
        fireRateTimer = currentReloadTime;
        reloadTimer = currentFirerate;

        currentDamage = weaponStats.damage;
        maxLv = weaponStats.levelMax;
        currentPrice = weaponStats.priceToUp;
    }
    public void Shoot(float dir)
    {
        if (fireRateTimer > 0||currentBullet <= 0) 
        {
            CheckReLoad();
        }
        else
        {
            if (muzzlePrefab != null)
            {
                GameObject muzzle = Instantiate(muzzlePrefab, firePos.position, firePos.rotation);
                muzzle.transform.SetParent(firePos);
            }
            if (bulletPrefab != null)
            {
                GameObject bullet = Instantiate(bulletPrefab, firePos.position, firePos.rotation);
                Bullet setBullet = bullet.GetComponent<Bullet>();
                if (setBullet != null)
                {
                    OnShoot?.Invoke();
                    setBullet.damage = currentDamage;
                }
            }
            currentBullet--;
            fireRateTimer = currentFirerate;
        }
    }

    private void CheckReLoad()
    {
        if (currentBullet <= 0 && reloadTimer < 0)
        {
            Reload();
            ReduceReloadTime();
        }
    }

    private void Reload()
    {
        OnReload?.Invoke();
        reloadTimer = currentReloadTime;
        isReloading = false;
        ReduceReloadTime();

       // currentBullet = weaponStats.bullet;
    }
    public void SetRotate(float rotate)
    {
        this.transform.eulerAngles = new Vector3(0,0,rotate);
    }
}
