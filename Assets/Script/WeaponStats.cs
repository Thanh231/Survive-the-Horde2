using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Stats", menuName = "Create Weapon Stats")]
public class WeaponStats : Stats
{
    [Header("Base Level")]
    public int bullet;
    public int damage;
    public float fireRate;
    public float reloadTime;
    public int currentLevel;
    public int levelMax;

    [Header("Level Up")]
    public int bulletUp;
    public float damageUp;
    public float fireRateUp;
    public float reloadTimeUp;

    [Header("Price")]
    public int priceToUp;
    public int qualityPriceWhenLevelUp;

    [Header("To Avoid Error")]
    public float minFireRate = 0.1f;
    public float minReloadTime = 0.01f;


}
