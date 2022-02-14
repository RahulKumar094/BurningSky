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
    public static List<EnemyPlane> EnemySamples { get { return enemySamples; } }
    private static List<EnemyPlane> enemySamples = new List<EnemyPlane>();

    private static IPool<HomingMissile> missilePool;
    private static IPool<Bullet> playerBulletPool;
    private static IPool<Bullet> enemyBulletPool;
    private static IPool<Coin> coinPool;

    //enemies
    private static IPool<EnemyPlane> bossEnemyPool;
    private static IPool<EnemyPlane> mediumBlueEnemyPool;
    private static IPool<EnemyPlane> mediumGreyEnemyPool;
    private static IPool<EnemyPlane> smallRedEnemyPool;
    private static IPool<EnemyPlane> smallGreenEnemyPool;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        enemySamples = GetEnemyPlaneSamples();
        CreatePool();
    }

    void CreatePool()
    {
        missilePool = new GenericPool<HomingMissile>(() => Instantiate(HomingMissilePrefab, HomingMissileContainer).GetComponent<HomingMissile>(), MissileCount, x => x.gameObject.activeSelf);
        
        coinPool = new GenericPool<Coin>(() => Instantiate(CoinPrefab, CoinContainer).GetComponent<Coin>(), CoinCount, x => x.gameObject.activeSelf);

        playerBulletPool = new GenericPool<Bullet>(() => Instantiate(PlayerBulletPrefab, PlayerBulletContainer).GetComponent<Bullet>(), CoinCount, x => x.gameObject.activeSelf);

        enemyBulletPool = new GenericPool<Bullet>(() => Instantiate(EnemyBulletPrefab, EnemyBulletContainer).GetComponent<Bullet>(), CoinCount, x => x.gameObject.activeSelf);

        smallRedEnemyPool = new GenericPool<EnemyPlane>(() => Instantiate(enemyPoolInfo[0].Prefab, enemyPoolInfo[0].Container).GetComponent<EnemyPlane>(), enemyPoolInfo[0].Count, x => x.gameObject.activeSelf);
        smallGreenEnemyPool = new GenericPool<EnemyPlane>(() => Instantiate(enemyPoolInfo[1].Prefab, enemyPoolInfo[1].Container).GetComponent<EnemyPlane>(), enemyPoolInfo[1].Count, x => x.gameObject.activeSelf);
        mediumBlueEnemyPool = new GenericPool<EnemyPlane>(() => Instantiate(enemyPoolInfo[2].Prefab, enemyPoolInfo[2].Container).GetComponent<EnemyPlane>(), enemyPoolInfo[2].Count, x => x.gameObject.activeSelf);
        mediumGreyEnemyPool = new GenericPool<EnemyPlane>(() => Instantiate(enemyPoolInfo[3].Prefab, enemyPoolInfo[3].Container).GetComponent<EnemyPlane>(), enemyPoolInfo[3].Count, x => x.gameObject.activeSelf);
        bossEnemyPool = new GenericPool<EnemyPlane>(() => Instantiate(enemyPoolInfo[4].Prefab, enemyPoolInfo[4].Container).GetComponent<EnemyPlane>(), enemyPoolInfo[4].Count, x => x.gameObject.activeSelf);
    }

    public HomingMissile GetMissile() 
    {
        return missilePool.GetInstance();
    }

    public Bullet GetPlayerBullet()
    {
        return playerBulletPool.GetInstance();
    }

    public Bullet GetEnemyBullet()
    {
        return enemyBulletPool.GetInstance();
    }

    public Coin GetCoin()
    {
        return coinPool.GetInstance();
    }

    public EnemyPlane GetEnemyPlane(EnemyType type)
    {
        if(type == EnemyType.SmallGreen)
            return smallGreenEnemyPool.GetInstance();
        else if (type == EnemyType.MediumBlue)
            return mediumBlueEnemyPool.GetInstance();
        else if (type == EnemyType.MediumGrey)
            return mediumGreyEnemyPool.GetInstance();
        else if (type == EnemyType.Boss)
            return bossEnemyPool.GetInstance();
        else
            return smallRedEnemyPool.GetInstance();
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

    public void DestroyAliveObjectFromPool()
    {
        var pool1 = missilePool as GenericPool<HomingMissile>;
        foreach (HomingMissile hm in pool1.items)
        {
            hm.gameObject.SetActive(false);
        }

        var pool2 = coinPool as GenericPool<Coin>;
        foreach (Coin co in pool2.items)
        {
            if(co.alive) co.Destroy();
        }

        var pool3 = playerBulletPool as GenericPool<Bullet>;
        foreach (Bullet bul in pool3.items)
        {
            if(bul.alive) bul.Destroy();
        }

        var pool4 = enemyBulletPool as GenericPool<Bullet>;
        foreach (Bullet bul in pool4.items)
        {
            if (bul.alive) bul.Destroy();
        }

        var pool5 = smallRedEnemyPool as GenericPool<EnemyPlane>;
        foreach (EnemyPlane plane in pool5.items)
        {
            if (plane.alive) plane.Destroy();
        }

        var pool6 = smallGreenEnemyPool as GenericPool<EnemyPlane>;
        foreach (EnemyPlane plane in pool6.items)
        {
            if (plane.alive) plane.Destroy();
        }

        var pool7 = mediumBlueEnemyPool as GenericPool<EnemyPlane>;
        foreach (EnemyPlane plane in pool7.items)
        {
            if (plane.alive) plane.Destroy();
        }

        var pool8 = mediumGreyEnemyPool as GenericPool<EnemyPlane>;
        foreach (EnemyPlane plane in pool8.items)
        {
            if (plane.alive) plane.Destroy();
        }

        var pool9 = bossEnemyPool as GenericPool<EnemyPlane>;
        foreach (EnemyPlane plane in pool9.items)
        {
            if (plane.alive) plane.Destroy();
        }
    }

}

[System.Serializable]
public struct EnemyPoolInfo
{
    public int Count;
    public GameObject Prefab;
    public Transform Container;
}