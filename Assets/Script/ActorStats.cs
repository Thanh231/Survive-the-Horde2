using System;
using UnityEngine;

public class ActorStats : ScriptableObject
{
    [Header("Base Level")]
    public int level;
    public int hp;
    public int maxLevel;
    public float xpRequireToUgrade;

    [Header("LevelUp Quality")]
    public float addHpWhenLevelUp;
    public float qualityXpWhenLevelUp;

    public float moveSpeed;
    public float knockBackForce;
    public float knockBackTime;
    public float invicibleTime;
}
