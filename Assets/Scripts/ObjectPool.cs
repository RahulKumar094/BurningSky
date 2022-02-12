using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance { get { return instance; } }
    private static ObjectPool instance;

    public int MissileCount = 3;
    public int PlayerBulletCount = 50;
    public int EnemyBulletCount = 50;
    public int CoinCount = 80;
    public GameObject HomingMissilePrefab;
    public GameObject PlayerBulletPrefab;
    public GameObject EnemyBulletPrefab;
    public GameObject CoinPrefab;
    public Transform HomingMissileContainer;
    public Transform PlayerBulletContainer;
    public Transform EnemyBulletContainer;
    public Transform CoinContainer;

    public EnemyPoolInfo[] enemyPoolInfo = new EnemyPoolInfo[5];

    public static List<EnemyPlane> EnemyPlanes { get { return enemyPlanes; } }
    public static List<Bullet> Bullets { get { return bullets; } }

    private static List<HomingMissile> homingMissiles = new List<HomingMissile>();
    private static List<EnemyPlane> enemyPlanes = new List<EnemyPlane>();
    private static List<Bullet> bullets = new List<Bullet>();
    private static List<Coin> coins = new List<Coin>();

    public static List<EnemyPlane> EnemySamples { get { return enemySamples; } }
    private static List<EnemyPlane> enemySamples = new List<EnemyPlane>(); 

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        enemySamples = GetEnemyPlaneSamples();
    }

    void Start()
    {
        CreateMissilePool();
        CreateBulletPool();
        CreateEnemyPool();
        CreateCoinPool();
    }

    void CreateMissilePool()
    {
        for (int i = 0; i < MissileCount; i++)
        {
            GameObject go = Instantiate(HomingMissilePrefab, HomingMissileContainer);
            HomingMissile missile = go.GetComponent<HomingMissile>();
            missile.Initialize();
            homingMissiles.Add(missile);
        }
    }

    void CreateBulletPool()
    {
        for (int i = 0; i < PlayerBulletCount; i++)
        {
            GameObject go = Instantiate(PlayerBulletPrefab, PlayerBulletContainer);
            bullets.Add(new PlayerBullet(go.transform));
        }

        for (int i = 0; i < EnemyBulletCount; i++)
        {
            GameObject go = Instantiate(EnemyBulletPrefab, EnemyBulletContainer);
            bullets.Add(new EnemyBullet(go.transform));
        }
    }

    void CreateEnemyPool()
    {
        for (int i = 0; i < enemyPoolInfo.Length; i++)
        {
            for (int j = 0; j < enemyPoolInfo[i].Count; j++)
            {
                GameObject go = Instantiate(enemyPoolInfo[i].Prefab, enemyPoolInfo[i].Container);
                EnemyPlane plane = go.GetComponent<EnemyPlane>();
                enemyPlanes.Add(plane);
                go.SetActive(false);
            }
        }
    }

    void CreateCoinPool()
    {
        for (int i = 0; i < CoinCount; i++)
        {
            GameObject go = Instantiate(CoinPrefab, CoinContainer);
            Coin coin = go.GetComponent<Coin>();
            go.SetActive(false);
            coins.Add(coin);
        }
    }

    public HomingMissile GetMissile() 
    {
        HomingMissile missile = homingMissiles.Find(x => x.gameObject.activeSelf == false);
        if (missile == null) throw new System.Exception("all missile in pool are in alive. consider increasing the item count or destroy unused items");
        return missile;
    }

    public Bullet GetBullet<T>() where T : Bullet
    {
        Bullet bullet = bullets.Find(x => !x.alive && x is T);

        if (bullet != null) return bullet;
        throw new System.Exception(string.Format("all bullets of type {0} in pool are in alive. consider increasing the item count or destroy unused items", typeof(T)));
    }

    public EnemyPlane GetEnemyPlane(EnemyType type)
    {
        EnemyPlane plane = enemyPlanes.Find(x => !x.alive && x.type == type);

        if (plane != null) return plane;
        throw new System.Exception(string.Format("all plane of type {0} in pool are in alive. consider increasing the item count or destroy unused items", type));
    }

    public Coin GetCoin()
    {
        Coin coin = coins.Find(x => x.gameObject.activeSelf == false);
        if (coin == null) throw new System.Exception("all coins in pool are in alive. consider increasing the item count or destroy unused items");
        return coin;
    }

    public void DestroyBullet(Transform transform)
    {
        Bullet bullet = bullets.Find(x => x.transform == transform);

        if (bullet != null)
            bullet.Destroy();
    }

    private List<EnemyPlane> GetEnemyPlaneSamples()
    {
        List<EnemyPlane> planes = new List<EnemyPlane>();
        foreach (EnemyPoolInfo info in enemyPoolInfo)
        {
            planes.Add(info.Prefab.GetComponent<EnemyPlane>());
        }
        return planes;
    }

}

[System.Serializable]
public struct EnemyPoolInfo
{
    public int Count;
    public GameObject Prefab;
    public Transform Container;
}