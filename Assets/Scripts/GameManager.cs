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
            UIManager.Instance.EnablePowerUpPanel(true);
            EnemySpawnManager.Instance.StartSpawner(true);
            InputDesire.Instance.EnableInput(true);
            EnemyPlane.CanShoot = true;
        });

        EnemyPlane.CanShoot = false;
        InputDesire.Instance.EnableInput(false);
        InputDesire.Instance.PointerDragEvent += player.Move;

        UIManager.Instance.Initialize();
        UIManager.Instance.EnablePowerUpPanel(false);
        UIManager.Instance.SetHealth(player.GetHealthPercentage);
    }

    void Update()
    {
        if (Paused) return;

        if (player.alive)
            UIManager.Instance.UpdatePowerUp(player.MissileRechargeProgress, player.MissileCount, player.ShieldRechargeProgress, player.ShieldCount);

        //update alive bullets
        foreach (Bullet bullet in ObjectPool.Bullets)
        {
            if(bullet.alive)
                bullet.Update();
        }
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
        Bullet bullet = ObjectPool.Instance.GetBullet<PlayerBullet>();
        bullet.Shoot(startPosition, direction);
    }

    public void ShootEnemyBullet(Vector3 startPosition, Vector3 direction)
    {
        Bullet bullet = ObjectPool.Instance.GetBullet<EnemyBullet>();
        bullet.Shoot(startPosition, direction);
    }

    #endregion


    #region CollsionFunctions
    public void MissileHitTarget(Transform target, Vector3 collisionPoint)
    {
        if (target.CompareTag(Tags.EnemyTag))
        {
            EnemyPlane enemy = target.GetComponent<EnemyPlane>();
            enemy.health -= 200f;
            CheckEnemyAfterCollision(enemy);
        }
        else if (target.CompareTag(Tags.PlayerTag))
        {
            if (!player.ShieldActive)
            {
                player.health -= 200;
                CheckPlayerAfterCollision();
                UIManager.Instance.SetHealth(player.GetHealthPercentage);
            }
        }
        CreateExplosion(collisionPoint);
    }

    public void PlayerCollideWithEnemy(EnemyPlane enemy, Vector3 collisionPoint)
    {
        enemy.health -= 100f;
        CheckEnemyAfterCollision(enemy);

        if (!player.ShieldActive)
        {
            player.health -= 40f;
            CheckPlayerAfterCollision();
            UIManager.Instance.SetHealth(player.GetHealthPercentage);
        }

        CreateExplosion(collisionPoint);
    }

    public void PlayerCollideWithBullet(Transform bulletTransform)
    {
        if (!player.ShieldActive)
        {
            player.health -= 20f;
            CheckPlayerAfterCollision();
            UIManager.Instance.SetHealth(player.GetHealthPercentage);
        }

        Bullet bullet = ObjectPool.Bullets.Find(x => x.transform == bulletTransform);        
        CreateExplosion(bullet.transform.position);        
        bullet.Destroy();
    }

    public void EnemyCollideWithBullet(Transform bulletTransform, EnemyPlane enemy)
    {
        if (!enemy.invinsible)
            enemy.health -= 50f;

        Bullet bullet = ObjectPool.Bullets.Find(x => x.transform == bulletTransform);
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
            UIManager.Instance.EnablePowerUpPanel(false);
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
                ChangeLevel();
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
            UIManager.Instance.EnablePowerUpPanel(true);

            EnemySpawnManager.Instance.StartSpawner(true);
            InputDesire.Instance.EnableInput(true);
            EnemyPlane.CanShoot = true;
        });
        EnemyPlane.CanShoot = false;
    }

    private void ChangeLevel()
    {
        Game.LevelHighscore[Game.Level - 1] = collectedCoins * Game.CoinToScoreMultiplier;
        int currentHighscore = 0;
        for (int i = 0; i < Game.Level; i++)
        {
            currentHighscore += Game.LevelHighscore[i];
        }

        int hs = PlayerPrefs.GetInt(Game.Highscore_Key);
        if (currentHighscore > hs)
        {
            PlayerPrefs.SetInt(Game.Highscore_Key, currentHighscore);
        }

        if (Game.Level < Game.LevelMax)
        {
            Game.Level++;
            Invoke("LoadGameLevel", 5f);
        }
        else
            Invoke("ShowGameoverScreen", 3f);
    }

    private void ShowGameoverScreen()
    {
        UIManager.Instance.GameOver();
    }

    private void LoadGameLevel()
    {
        SceneLoader.LoadGameLevel();
        ResetLevel();

        maxCoinsInLevel = EnemySpawnManager.Instance.MaxCoinInLevel(Game.Level - 1);
    }

    public void RestartLevel()
    {
        Paused = false;
        ResetLevel();
    }

    private void ResetLevel()
    {
        DestroyAliveObjects();
        ResetPlayer();

        collectedCoins = 0;
        UIManager.Instance.Initialize();
    }

    private void DestroyAliveObjects()
    {
        foreach (EnemyPlane plane in ObjectPool.EnemyPlanes)
        {
            if (plane.alive) plane.Destroy();
        }

        foreach (Coin coin in ObjectPool.Coins)
        {
            if (coin.gameObject.activeSelf) coin.Destroy();
        }
    }
}
