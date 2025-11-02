
using UnityEngine;

public class Enemy : Actor
{
    private float currentHP = 0;
    public EnemyStats enemyStats { get; private set; }
    private float m_damage;
    private float m_xpBonus;
    private Player player;
    private float curretDamage;
    void OnEnable()
    {
        EventManager.OnGameOver += GameOver;
    }
    private void OnDisable() {
        EventManager.OnGameOver -= GameOver;
        onDead.RemoveListener(OnspawnCollectable);
        onDead.RemoveListener(OnAddXpToPlayer);
    }

    private void GameOver()
    {
        Destroy(gameObject);
    }

    public override void TakeDamage(float damage)
    {
        if (m_isInvicible) return;
        currentHP -= damage;
         KnockBack();
        if (currentHP <= 0)
        {
            currentHP = 0;
            Die();
        }
        onTakeDamage?.Invoke();
    }
    private void LoadStats()
    {
        if (statsData == null) return;
        enemyStats = (EnemyStats)statsData;
    }
    private void Start()
    {
        player = GameManager.Ins.player;
        Init();
    }
    public override void Init()
    {
        LoadStats();
        StateCaculate();
        onDead.AddListener(() => OnspawnCollectable());
        onDead.AddListener(() => OnAddXpToPlayer());
    }
    public override void Die()
    {
        base.Die();
        m_anim.SetBool(AniimationConstant.dead, true);
        CineController.Ins.ShakeTrigger();
    }
    private void StateCaculate()
    {
        float hpUpdage = enemyStats.hpUp * Helper.GetQualityLevelUp(player.currentlevel);
        float damageUpgrade = enemyStats.damageUp + Helper.GetQualityLevelUp(player.currentlevel);
        float randomXp = Random.Range(enemyStats.minXp, enemyStats.maxXp);

        currentHP = enemyStats.hp + hpUpdage;
        curretDamage = enemyStats.damage + damageUpgrade;
        m_xpBonus = randomXp * Helper.GetQualityLevelUp(player.currentlevel);
    }
    private void OnCollisionEnter2D(Collision2D other) {
         if (other.gameObject.CompareTag(TagConstant.Player_Tag))
        {
            KnockBack();
        }
    }
    private void OnspawnCollectable()
    {
        CollectableManager.Ins.Spawn(transform.position);
    }
    private void OnAddXpToPlayer()
    {
        GameManager.Ins.player.AddXp(m_xpBonus);
    }
    private void FixedUpdate()
    {
        Move();
    }
    protected override void Move()
    {
        if (player == null || IsDead)
        {
            m_rd.linearVelocity = Vector3.zero;
        }
        else
        {
            Vector2 directionPlayer = player.transform.position - transform.position;
            directionPlayer.Normalize();
            if (!m_isKnockBack)
            {
                Filp(directionPlayer);

                m_rd.linearVelocity = directionPlayer * enemyStats.moveSpeed * Time.deltaTime;
                return;
            }
            else
            {
                m_rd.linearVelocity = -directionPlayer * 200 * Time.deltaTime;
            }
        }
    }

    private void Filp(Vector2 directionPlayer)
    {
        if (directionPlayer.x > 0)
        {
            if (transform.localScale.x > 0) return;
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
        else if (directionPlayer.x < 0)
        {
            if(transform.localScale.x < 0) return;
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);

        }
    
    }
}

