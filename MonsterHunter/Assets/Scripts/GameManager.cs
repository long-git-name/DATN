using System;
using System.Collections;
using UnityEngine;

public enum GameState
{
    STARTING, PLAYING, PAUSED, GAMEOVER
}

public class GameManager : Singleton<GameManager>
{
    public static GameState state;
    [SerializeField] private Map mapPrefab;
    [SerializeField] private Player playerPrefab;
    [SerializeField] private Enemy[] enemyPrefab;
    [SerializeField] private GameObject enemySpawnVfx;
    [SerializeField] private float enemySpawnTime;
    [SerializeField] private int playerMaxLife;
    [SerializeField] private int playerStartingLife;
    [SerializeField] private SkillDrawer skillDrawer;

    private Map map;
    private Player player;
    private int curLife;
    private bool isNewGame = true;
    public Player Player { get => player; private set => player = value; }

    public int CurLife
    {
        get => curLife;
        set
        {
            curLife = value;
            curLife = Mathf.Clamp(curLife, 0, playerMaxLife);
        }
    }

    public SkillDrawer SkillDrawer { get => skillDrawer; }

    protected override void Awake()
    {
        MakeSingleton(false);
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        state = GameState.STARTING;
        //state = GameState.PLAYING;
        SpawnMapAndPlayer();

        GUIManager.Ins.ShowGameGUI(false);
        // PlayGame();
    }


        //tạo map và player từ Prefab
    private void SpawnMapAndPlayer()
    {
        if(mapPrefab == null || playerPrefab == null) return;

        map = Instantiate(mapPrefab, Vector3.zero, Quaternion.identity);
        player = Instantiate(playerPrefab, map.playerSpawnPoint.position, Quaternion.identity);
    }

    private void CheckNewGame()
    {
        if(isNewGame)
        {
            curLife = playerStartingLife;
            Prefs.coins = 0;
        }
        else
        {
            curLife = Prefs.lifeData;
        }
    }

    public void NewGame()
    {
        if(player == null) return;
        isNewGame = true;
        CheckNewGame();
        state = GameState.PLAYING;
        UpdateUI();
        SpawEnemy();
    }

    public void PlayGame()
    {
        if(player == null) return;
        isNewGame = false;
        CheckNewGame();
        state = GameState.PLAYING;

        player.PlayerStats.Load();
        player.weapon.statData.Load();

        SpawEnemy();
        
        SkillManager.Ins.LoadSkillData();
        UpdateUI();
    }

    private void UpdateUI()
    {
        GUIManager.Ins.ShowGameGUI(true);
        GUIManager.Ins.UpdateLifeInfo(curLife);
        GUIManager.Ins.UpdateCoinCount(Prefs.coins);
        GUIManager.Ins.UpdateHpInfo(player.CurHp, player.PlayerStats.Hp);
        GUIManager.Ins.UpdateLevelInfo(player.PlayerStats.Level, player.PlayerStats.Exp, player.PlayerStats.ExpToUpLevel);
        skillDrawer?.DrawSkill();
    }

        //tạo enemy 
    private void SpawEnemy()
    {
        var randomEnemy = GetRandomEnemy();
        if(randomEnemy == null || map == null) return;
        StartCoroutine(SpawnEnemyCourotine(randomEnemy));
    }

        // lấy ngẫu nhiên enemy
    private Enemy GetRandomEnemy()
    {
        if(enemyPrefab == null || enemyPrefab.Length <= 0) return null;
        int randomIndex = UnityEngine.Random.Range(0, enemyPrefab.Length);
        return enemyPrefab[randomIndex];
    }

    private IEnumerator SpawnEnemyCourotine(Enemy randomEnemy)
    {
        yield return new WaitForSeconds(3f);
        //state = GameState.PLAYING;

        while(state == GameState.PLAYING)
        {
            if(map.RandomSpawnPoints == null) break;

            Vector3 spawnPoint = map.RandomSpawnPoints.position;
            if(enemySpawnVfx)
                Instantiate(enemySpawnVfx, spawnPoint, Quaternion.identity);
            yield return new WaitForSeconds(0.2f);

            Instantiate(randomEnemy, spawnPoint, Quaternion.identity);
            yield return new WaitForSeconds(enemySpawnTime);
        }
        yield return null;
    }

    public void SaveGameData()
    {
        Prefs.lifeData = curLife;
        player.PlayerStats.Save();
        player.weapon.statData.Save();
        SkillManager.Ins.SaveSkillData();
        // Debug.Log(Prefs.playerData);
    }

    public void GameOverCheck(Action OnLostLife = null, Action OnDead = null)
    {
        if(curLife <= 0) return;

        curLife--;
        
        OnLostLife?.Invoke();

        if(curLife <= 0)
        {
            state = GameState.GAMEOVER;
            OnDead?.Invoke();
        }
    }
}
