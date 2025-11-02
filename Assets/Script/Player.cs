using System;
using UnityEngine;
using UnityEngine.Events;

public class Player : Actor
{
    [Header("Base Level")]
    public int currentlevel = 1;
    private float currentSpeed = 0;
    public float currentXp = 0;
    public float currentCoin = 10;
    public float currentHP = 10;
    public int currentLife = 3;

    public int maxLevel = 100;
    public int maxExp = 0;
    public float maxHP = 0;
    public float increaseSpeed = 0;
    public float maxDistance = 0;
    public float radiusCheckEnemy = 0;

    [Header("Input")]
    public PlayerStates playerStates;
    public PlayerInput input;
    public LayerMask enemyLayer;
    private Vector2 movingDir;
    public bool isStartGame = false;
    public GameObject sprite;

    public UnityEvent OnAddXp;
    public UnityEvent OnLevelUp;
    public UnityEvent OnLostLife;

    void OnEnable()
    {
        EventManager.OnStartGame += StartGame;
        EventManager.OnGameOver += ResetGame;
    }

    void OnDisable()
    {
        EventManager.OnStartGame -= StartGame;
        EventManager.OnGameOver -= ResetGame;
    }

    private void ResetGame()
    {
        isStartGame = false;
        sprite.SetActive(false);
    }

    private void StartGame()
    {
        isStartGame = true;
        LoadStats();
        sprite.SetActive(true);
    }

    public override void Init()
    {
        
    }
    public bool IsMaxLevel()
    {
        return currentlevel >= maxLevel;
    }
    void Update()
    {
        if (!isStartGame) return;
        movingDir = input.GetDirection();
        Move();
        var enemies = Physics2D.OverlapCircleAll(transform.position, radiusCheckEnemy, enemyLayer);
        CheckEnemyAround(enemies);
    }

    private void CheckEnemyAround(Collider2D[] enemies)
    {
        if (enemies.Length == 0 || enemies == null) return;
        // Debug.Log("congthanh " );
        float min = float.MaxValue;
        Actor enemyMin = null;
        foreach (Collider2D enemy in enemies)
        {
            var enemyTemp = enemy.GetComponent<Actor>();
            float minDistanceTemp = Vector2.Distance(transform.position, enemy.transform.position);
            if (minDistanceTemp < min && enemyTemp != enemyMin)
            {
                min = minDistanceTemp;
                enemyMin = enemyTemp;
                ProcessWeapon(enemyMin);
            }
        }
    }

    private void ProcessWeapon(Actor enemy)
    {
        Vector2 direction = enemy.transform.position - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        weapon.SetRotate(angle);
        weapon.Shoot(angle);
    }

    protected override void Move()
    {
        if (movingDir != Vector2.zero)
        {
            currentSpeed += increaseSpeed * Time.deltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, 0, playerStates.moveSpeed);
            m_rd.linearVelocity = movingDir * currentSpeed * Time.deltaTime;
            m_anim.SetBool(AniimationConstant.player_Run, true);
        }
        else
        {
            BackToIdle();
        }
    }

    private void BackToIdle()
    {
        currentSpeed = 0;
        m_rd.linearVelocity = Vector2.zero;
        m_anim.SetBool(AniimationConstant.player_Run, false);
    }

    private void LoadStats()
    {
        if (statsData == null) return;
        playerStates = (PlayerStates)statsData;

        // playerStates.Load();
        currentSpeed = playerStates.moveSpeed;
        currentHP = playerStates.hp;
        maxHP = playerStates.hp;
        currentCoin = 0;
        currentlevel = playerStates.level;
        currentXp = 0;
        maxExp = (int)playerStates.xpRequireToUgrade;
        currentLife = playerStates.defaultLife;

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color32(133, 250, 47, 50);
        Gizmos.DrawSphere(transform.position, radiusCheckEnemy);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(TagConstant.Enemy_Tag))
        {
            var damage = collision.gameObject.GetComponent<Enemy>();
            Vector2 direction = collision.gameObject.transform.position - transform.position;
            TakeDamage(damage.enemyStats.damage);
            if (m_isKnockBack)
            {
                m_rd.linearVelocity = direction * -statsData.knockBackForce * Time.deltaTime;
            }
        }
        else if (collision.gameObject.CompareTag(TagConstant.Collectable_Tag))
        {
            Collectable item = collision.gameObject.GetComponent<Collectable>();
            if (item != null)
            {
                item.Trigger();
            }
            Destroy(collision.gameObject);
        }
    }
    public override void TakeDamage(float damage)
    {
        if (m_isInvicible) return;
        currentHP = Mathf.Clamp(currentHP - damage,0,maxHP);
        KnockBack();
        onTakeDamage?.Invoke();
        if (currentHP > 0) return;
        GameManager.Ins.GameOverChecking(OnLostLifeDelegate, OnDeadDelegate);
    }
    private void OnLostLifeDelegate()
    {
        //CurrentHP = playerStates.addHpWhenLevelUp;
        OnLostLife?.Invoke();
        currentHP = maxHP;
        GUIManager.Ins.UpdateHpInfo(currentHP, maxHP);
        GUIManager.Ins.UpdateLifeInfo(currentLife);
        AudioController.Ins.PlaySound(AudioController.Ins.lostLife);
    }
    private void OnDeadDelegate()
    {
        currentHP = 0;
        Die();
    }
    public void AddXp(float xpBonus)
    {
        if (playerStates == null) return;
        currentXp += xpBonus;
        Upgrade(OnUpgradeState);
        OnAddXp?.Invoke();
    }
    private void OnUpgradeState()
    {
        OnLevelUp?.Invoke();
        AudioController.Ins.PlaySound(AudioController.Ins.levelUp);
        maxExp += (int)(playerStates.qualityXpWhenLevelUp * Helper.GetQualityLevelUp(currentlevel));
        currentHP += playerStates.addHpWhenLevelUp * Helper.GetQualityLevelUp(currentlevel);
        maxHP = currentHP;
        GUIManager.Ins.UpdateHpInfo(currentHP, maxHP);
    }
    
    public void Upgrade(Action onSuccess = null, Action onFailed = null)
    {

        while (currentXp >= maxExp && !IsMaxLevel())
        {
            currentlevel++;
            currentXp -= maxExp;
            onSuccess?.Invoke();
        }
        if (currentlevel < maxLevel || IsMaxLevel())
        {
            onFailed?.Invoke();
        }
        GUIManager.Ins.UpdateLevelInfo(currentlevel,currentXp, maxExp);
    }
}
