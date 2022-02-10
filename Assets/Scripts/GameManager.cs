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

    public int Level { get; private set; }
    public Transform PlayerTransform { get { return player.transform; }}

    public GameObject PlayerPlanePrefab;
    public PlaneAttribute PlayerPlaneAttribute;

    private PlayerPlane player;
    private bool isGamePaused = false;
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        Level = 1;

        GameObject go = Instantiate(PlayerPlanePrefab);
        player = go.GetComponent<PlayerPlane>();
        player.SetAttribute(PlayerPlaneAttribute);
        player.SpawnWithCutscene(() => EnemySpawnManager.Instance.StartSpawner());

        InputDesire.Instance.EnableInput(true);
        InputDesire.Instance.PointerDragEvent += player.Move;
    }

    void Update()
    {
        if (isGamePaused) return;

        //update alive bullets
        foreach (Bullet bullet in ObjectPool.Bullets)
        {
            if(bullet.alive)
                bullet.Update();
        }
    }

    #region BulletFunctions

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
    public void PlayerCollideWithEnemy(EnemyPlane enemy, Vector3 collisionPoint)
    {
        player.health -= 40f;
        enemy.health -= 100f;

        CreateExplosion(collisionPoint);
        CheckPlayerAfterCollision();
        CheckEnemyAfterCollision(enemy);
    }

    public void PlayerCollideWithBullet(Transform bulletTransform)
    {
        player.health -= 20f;

        Bullet bullet = ObjectPool.Bullets.Find(x => x.transform == bulletTransform);        
        CreateExplosion(bullet.transform.position);
        CheckPlayerAfterCollision();
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
            Invoke("RespawnPlayer", 3f);
        }
    }

    private void CheckEnemyAfterCollision(EnemyPlane enemy)
    {
        if (enemy.health <= 0f)
        {
            enemy.Destroy();
            //AddScore(EnemyType type);
        }
    }

    private void CreateExplosion(Vector3 explosionPoint)
    {
        Debug.LogError("Create explosion at position: " + explosionPoint);
    }

    #endregion

    private void RespawnPlayer()
    {
        player.SetAttribute(PlayerPlaneAttribute);
        player.SpawnWithCutscene(() => EnemySpawnManager.Instance.StartSpawner(false));
    }

}

public class Tags
{
    public const string InvinsibilityWall = "InvinsibilityWall";
    public const string PlayerBulletTag = "PlayerBullet";
    public const string EnemyBulletTag = "EnemyBullet";
    public const string PlayerTag = "Player";
    public const string EnemyTag = "Enemy";
}
