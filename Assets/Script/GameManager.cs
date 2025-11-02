using System;
using System.Collections;
using UnityEngine;


public enum GameState
{
    STARTING,
    PLAYING, 
    PAUSED,
    GAMEOVER
}

public class GameManager : Singleton<GameManager>
{
    // public static GameState state;
    private bool isStartGame = false;
    [SerializeField] private Map mapPrefabs;
    // [SerializeField] private Player playerPrefabs;
    [SerializeField] private Enemy[] enemyPrefabs;
    [SerializeField] private GameObject aiSpawVfx;
    [SerializeField] float aiSpawnTime;
    // [SerializeField] private int playerMaxLife;
    // [SerializeField] private int playerStartLife;

    // private Map currentMap;
    // private int currentlife;
    public Player player;
    // public Player Player { get => m_player; private set => m_player = value; }
    private void Start()
    {
        Init();
    }
    private void Init()
    {
        // state = GameState.STARTING;
        // currentlife = playerStartLife;
        // SpawnPlayer();
        GUIManager.Ins.ShowMainMenu();
    }

    protected override void Awake()
    {
        MakeSingleton(false);
    }

    public void PlayGame()
    {
        // state = GameState.PLAYING;
        isStartGame = true;
        EventManager.OnStartGame?.Invoke();
        SpawnEnemy();
        // currentMap = Instantiate(mapPrefabs, Vector3.zero, Quaternion.identity);
        GUIManager.Ins.PlayGame();
        GUIManager.Ins.UpdateLifeInfo(player.currentLife);
        GUIManager.Ins.UpdateCoinCounting((int)player.currentCoin);
        GUIManager.Ins.UpdateHpInfo(player.currentHP,player.maxHP);
        GUIManager.Ins.UpdateLevelInfo(player.currentlevel, player.currentXp, player.maxExp);
    }

    private void SpawnEnemy()
    {
        var randomEnemy = GetRanDomEnemy();
        if(randomEnemy == null || mapPrefabs == null) return;
        StartCoroutine(SpawnEnemy_Coroutine(randomEnemy));
    }

    IEnumerator SpawnEnemy_Coroutine(Enemy enemy)
    {
        yield return new WaitForSeconds(3f);
        
        while(isStartGame)
        {
            if (mapPrefabs.enemySpawnPosition == null) break;
            Vector3 spawnPos = mapPrefabs.GetEnemyPos.position;
            if (aiSpawVfx)
            {
                Instantiate(aiSpawVfx, spawnPos, Quaternion.identity);
            }
            yield return new WaitForSeconds(0.2f);
            Instantiate(enemy, spawnPos, Quaternion.identity);
            yield return new WaitForSeconds(aiSpawnTime);
            Debug.Log("spawn enemy");
        }
        yield return null;
    }

    private Enemy GetRanDomEnemy()
    {
        if (enemyPrefabs == null || enemyPrefabs.Length < 0 || mapPrefabs == null) return null;
        int random = UnityEngine.Random.Range(0, enemyPrefabs.Length);
        return enemyPrefabs[random];
    }
    public void GameOverChecking(Action OnlostLife = null, Action OnDead = null)
    {
        if (player.currentLife < 0) return;

        player.currentLife--;
        OnlostLife?.Invoke();

        if (player.currentLife < 0)
        {
            Debug.Log("123123");
            isStartGame = false;
            // state = GameState.GAMEOVER;
            OnDead?.Invoke();
            EventManager.OnGameOver?.Invoke();
            GUIManager.Ins.ShowMainMenu();
        }
    }
    

}
