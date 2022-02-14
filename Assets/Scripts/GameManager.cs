using UnityEngine;


public enum GameState
{
    WaitForPlayerInput,
    Loading,
    Paused,
    InLevel,
    LevelTransition
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get { return instance; } }
    private static GameManager instance;

    public GameObject PlayerPlanePrefab;
    public Transform PlayerTransform { get { return player.transform; }}

    private int[] levelHighscore = new int[Game.LevelMax];
    private PlayerPlane player;
    private int collectedCoins;
    private int maxCoinsInLevel;

    public static bool Paused = false;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        Paused = false;
        collectedCoins = 0;
        maxCoinsInLevel = EnemySpawnManager.Instance.MaxCoinInLevel(Game.Level - 1);        
        
        GameObject go = Instantiate(PlayerPlanePrefab);
        player = go.GetComponent<PlayerPlane>();
        player.SpawnWithCutscene(() => 
        {
            UIManager.Instance.EnableHUD(true);
            EnemySpawnManager.Instance.StartSpawner(true);
            InputDesire.Instance.EnableInput(true);
            EnemyPlane.CanShoot = true;
        });

        EnemyPlane.CanShoot = false;
        InputDesire.Instance.EnableInput(false);
        InputDesire.Instance.PointerDragEvent += player.Move;

        UIManager.Instance.Initialize();
        UIManager.Instance.EnableHUD(false);
        UIManager.Instance.SetHealth(player.GetHealthPercentage);
    }

    void Update()
    {
        if (Paused) return;

        if (player.alive)
            UIManager.Instance.UpdatePowerUp(player.MissileRechargeProgress, player.MissileCount, player.ShieldRechargeProgress, player.ShieldCount);
    }

    public void GamePaused()
    {
        Paused = true;
        InputDesire.Instance.EnableInput(false);
        EnemySpawnManager.Instance.StopSpawner();
    }

    public void ResumeGame()
    {
        Paused = false;
        InputDesire.Instance.EnableInput(true);
        EnemySpawnManager.Instance.StartSpawner();
    }

    public void PlayerActivateShield()
    {
        if (player.ShieldCount > 0)
        {
            player.ActivateShield();
        }
    }

    #region BulletFunctions
    public void PlayerLaunchMissile()
    {
        if (player.MissileCount > 0)
        {
            player.ActivateMissile(() =>
            {
                HomingMissile missile = ObjectPool.Instance.GetMissile();
                missile.LaunchAtTarget(null, player.Weapon_Center);
            });
        }
    }

    public void EnemyLaunchMissile(Transform startAt)
    {
        HomingMissile missile = ObjectPool.Instance.GetMissile();
        missile.LaunchAtTarget(player.transform, startAt);
    }

    public void ShootPlayerBullet(Vector3 startPosition, Vector3 direction)
    {
        Bullet bullet = ObjectPool.Instance.GetPlayerBullet();
        bullet.Shoot(startPosition, direction);
    }

    public void ShootEnemyBullet(Vector3 startPosition, Vector3 direction)
    {
        Bullet bullet = ObjectPool.Instance.GetEnemyBullet();
        bullet.Shoot(startPosition, direction);
    }

    #endregion


    #region CollsionFunctions
    public void MissileHitTarget(Transform target, Vector3 collisionPoint)
    {
        if (target.CompareTag(Tags.EnemyTag))
        {
            EnemyPlane enemy = target.GetComponent<EnemyPlane>();
            enemy.health -= DamageData.FromPlayerMissile;
            CheckEnemyAfterCollision(enemy);
        }
        else if (target.CompareTag(Tags.PlayerTag))
        {
            if (!player.ShieldActive)
            {
                player.health -= DamageData.FromEnemyMissile;
                CheckPlayerAfterCollision();
                UIManager.Instance.SetHealth(player.GetHealthPercentage);
            }
        }
        CreateExplosion(collisionPoint);
    }

    public void PlayerCollideWithEnemy(EnemyPlane enemy, Vector3 collisionPoint)
    {
        enemy.health -= DamageData.FromPlayerPlane;
        CheckEnemyAfterCollision(enemy);

        if (!player.ShieldActive)
        {
            player.health -= DamageData.FromEnemyPlane;
            CheckPlayerAfterCollision();
            UIManager.Instance.SetHealth(player.GetHealthPercentage);
        }

        CreateExplosion(collisionPoint);
    }

    public void PlayerCollideWithBullet(Bullet bullet)
    {
        if (!player.ShieldActive)
        {
            player.health -= DamageData.FromEnemyBullet;
            CheckPlayerAfterCollision();
            UIManager.Instance.SetHealth(player.GetHealthPercentage);
        }      
        CreateExplosion(bullet.transform.position);
        bullet.Destroy();
    }

    public void EnemyCollideWithBullet(Transform bulletTransform, EnemyPlane enemy)
    {
        if (!enemy.invinsible)
            enemy.health -= DamageData.FromPlayerBullet;

        Bullet bullet = bulletTransform.GetComponent<Bullet>();
        CreateExplosion(bullet.transform.position);
        CheckEnemyAfterCollision(enemy);
        bullet.Destroy();
    }

    private void CheckPlayerAfterCollision()
    {
        if (player.health <= 0f)
        {
            player.Destroy();
            EnemySpawnManager.Instance.StopSpawner();
            InputDesire.Instance.EnableInput(false);
            UIManager.Instance.EnableHUD(false);
            Invoke("ShowGameoverScreen", 3f);
        }
    }

    private void CheckEnemyAfterCollision(EnemyPlane enemy)
    {
        if (enemy.health <= 0f)
        {
            enemy.Destroy();

            //burst coins
            CoinValue[] values = enemy.coinValue.coinValue;
            foreach (CoinValue value in values)
            {
                for (int i = 0; i < value.count; i++)
                {
                    ObjectPool.Instance.GetCoin().Burst(value.type, enemy.transform.position);
                }
            }

            if (enemy is BossEnemyPlane)
            {
                EnemySpawnManager.Instance.StopSpawner();
                UIManager.Instance.EnableHUD(false);
                SaveLevelData();
                //Invoke("LoadNextLevel", 5f);
                Invoke("ShowLevelCompleteScreen", 5f);
            }
        }
    }

    private void CreateExplosion(Vector3 explosionPoint)
    {
        //Debug.LogError("Create explosion at position: " + explosionPoint);
    }

    public void PlayerCollectCoin(CoinType type)
    {
        collectedCoins += Coin.GetCoinValue(type);
        UIManager.Instance.SetCoinText(collectedCoins * 100f / maxCoinsInLevel);
    }

    #endregion

    private void ResetPlayer()
    {
        player.SpawnWithCutscene(() => 
        {
            UIManager.Instance.SetHealth(player.GetHealthPercentage);
            UIManager.Instance.SetCoinText(0f);
            UIManager.Instance.EnableHUD(true);

            EnemySpawnManager.Instance.StartSpawner(true);
            InputDesire.Instance.EnableInput(true);
            EnemyPlane.CanShoot = true;
        });
        EnemyPlane.CanShoot = false;
    }

    private void SaveLevelData()
    {
        levelHighscore[Game.Level - 1] = collectedCoins * Game.CoinToScoreMultiplier;
        int currentHighscore = 0;
        for (int i = 0; i < Game.Level; i++)
        {
            currentHighscore += levelHighscore[i];
        }

        int hs = PlayerPrefs.GetInt(Game.Highscore_Key);
        if (currentHighscore > hs)
        {
            PlayerPrefs.SetInt(Game.Highscore_Key, currentHighscore);
        }
    }

    private void ShowLevelCompleteScreen()
    {
        if(player.alive) UIManager.Instance.LevelComplete(levelHighscore[Game.Level - 1]);
    }

    private void ShowGameoverScreen()
    {
        UIManager.Instance.GameOver();
    }

    public void LoadNextLevel()
    {
        Game.Level++;
        if (Game.Level <= Game.LevelMax)
        {
            maxCoinsInLevel = EnemySpawnManager.Instance.MaxCoinInLevel(Game.Level - 1);
            SceneLoader.LoadGameLevelScene();
            ResetLevel();
        }
        else
        {
            Game.Level = Game.LevelMax;
            ShowGameoverScreen();
        }
    }

    public void ResetLevel()
    {
        ObjectPool.Instance.DestroyAliveObjectFromPool();

        Paused = false;
        collectedCoins = 0;
        ResetPlayer();
        UIManager.Instance.Initialize();
    }
}
